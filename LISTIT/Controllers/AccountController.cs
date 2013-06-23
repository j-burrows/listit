using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using LISTIT.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using Captcha.Helpers;

namespace LISTIT.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {
        //information about the captcha to be rendered
        private const int ImageWidth = 200, ImageHeight = 70;
        private const string FontFamily = "Rockwell";
        private readonly static Brush Foreground = Brushes.Navy;
        private readonly static Color Background = Color.Silver;

        // Control how the captcha image will be distorted when rendered.
        private const int WarpFactor = 5;
        private const Double xAmp = WarpFactor * ImageWidth / 100;
        private const Double yAmp = WarpFactor * ImageHeight / 85;
        private const Double xFreq = 2 * Math.PI / ImageWidth;
        private const Double yFreq = 2 * Math.PI / ImageHeight;
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        TempData["message"] = "Welcome.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model, string myCaptcha, string attempt){
            if (ModelState.IsValid){
                if (CaptchaHelper.VerifyAndExpireCaptcha(HttpContext, myCaptcha, attempt)){
                    // Attempt to register the user
                    MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                    if (createStatus == MembershipCreateStatus.Success){
                        FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                        return RedirectToAction("Index", "Home");
                    }else{
                        ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                    }
                }else{
                    ModelState.AddModelError("attempt", "Incorrect-please try again");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /*
        --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
        |  Subroutine: Render
        --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
        |  Purpose:    To render an image for the captcha icon given the text at a given 
        |              key in session[]
        --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
        |  @param:     challengeGuid   The key of session[] where the captcha text is kept.
        --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
        */
        public void Render(string challengeGuid)
        {
            //The solution text is retrieved from session[]
            string key = CaptchaHelper.SessionKeyPrefix + challengeGuid;
            string solution = (string)HttpContext.Session[key];

            if (solution != null)
            {
                using (Bitmap bmp = new Bitmap(ImageWidth, ImageHeight))
                using (Graphics g = Graphics.FromImage(bmp))
                using (Font font = new Font(FontFamily, 1f))
                {
                    g.Clear(Background);

                    //The best font size is determined
                    SizeF finalSize;
                    SizeF textSize = g.MeasureString(solution, font);
                    float bestFontSize = Math.Min(ImageWidth / textSize.Width,
                                                   ImageHeight / textSize.Height) * 0.95f;

                    using (Font finalFont = new Font(FontFamily, bestFontSize))
                    {
                        finalSize = g.MeasureString(solution, finalFont);
                    }

                    //The path is placed to start in the top right from center
                    g.PageUnit = GraphicsUnit.Point;
                    PointF textTopLeft = new PointF((ImageWidth - finalSize.Width) / 2,
                                                    (ImageHeight - finalSize.Height) / 2);


                    using (GraphicsPath path = new GraphicsPath())
                    {
                        //The solution is added to the path
                        path.AddString(solution, new FontFamily(FontFamily), 0,
                                        bestFontSize, textTopLeft, StringFormat.GenericDefault);

                        //The path is rendered onto the bitmap
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.FillPath(Foreground, DeformPath(path));
                        g.Flush();

                        //The image is sent to the stream in png format
                        Response.ContentType = "image/png";
                        using (var memoryStream = new MemoryStream())
                        {
                            bmp.Save(memoryStream, ImageFormat.Png);
                            memoryStream.WriteTo(Response.OutputStream);
                        }
                    }
                }

            }
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Function:   DeformPath
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Deforms a path of text to give a distored image so they cant be
         |              guessed by optical character recognision. Essentially, a surface
         |              is deformed by sin waves before the path is copied over it.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [NonAction]
        public GraphicsPath DeformPath(GraphicsPath path)
        {
            //A path to hold the deformed version of the given path is made.
            PointF[] deformed = new PointF[path.PathPoints.Length];
            Random random = new Random();
            //Random radian angles
            Double xSeed = random.NextDouble() * 2 * Math.PI;
            Double ySeed = random.NextDouble() * 2 * Math.PI;
            int xOffset, yOffset;
            int i;                          //Loop control.

            for (i = 0; i < path.PathPoints.Length; i++)
            {
                PointF original = path.PathPoints[i];
                //The periodic value of the x and y points are added together, then an x and
                //y offset is determined with the randomly created distortion seed, followed
                //by copying the original with the resulting offset into the deformed.
                Double val = xFreq * original.X + yFreq * original.Y;
                xOffset = (int)(xAmp * Math.Sin(val + xSeed));
                yOffset = (int)(yAmp * Math.Sin(val + ySeed));
                deformed[i] = new PointF(original.X + xOffset, original.Y + yOffset);
            }

            //The deformed path is initialised as the given type and returned.
            return new GraphicsPath(deformed, path.PathTypes);
        }

        public ActionResult SubmitRegistration(string myCaptcha, string attempt)
        {
            if (CaptchaHelper.VerifyAndExpireCaptcha(HttpContext, myCaptcha, attempt))
            {
                return Content("Pass");
            }
            else
            {
                ModelState.AddModelError("attempt", "Incorrect-please try again");
                return View("Index");
            }
        }

    }
}
