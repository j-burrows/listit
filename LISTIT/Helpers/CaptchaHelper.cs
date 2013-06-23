using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Captcha.Helpers
{
    public static class CaptchaHelper
    {
        internal const string SessionKeyPrefix = "__Captcha";
        private const string ImgFormat = "<img src=\"{0}\" />"
                            + @"<input type=""hidden"" name=""{1}"" value=""{2}""/>";

        public static MvcHtmlString Captcha(this HtmlHelper html, string name)
        {
            //A Guid key is received for the captcha
            string challengeGuid = Guid.NewGuid().ToString();

            //A random solution text is generated and stored
            var session = html.ViewContext.HttpContext.Session;
            session[SessionKeyPrefix + challengeGuid] = MakeRandomSolution();

            //An image tag is created for the distorted text, along with a hidden field
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string url = urlHelper.Action("Render", "Account", new { challengeGuid });
            string htmlToDisplay = string.Format(ImgFormat, url, name, challengeGuid);
            return MvcHtmlString.Create(htmlToDisplay);
        }

        /*
         --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Function:   MakeRandomSolution
         --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Generates a string of 5-7 random characters for the captcha
         --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        private static string MakeRandomSolution()
        {
            Random random = new Random();
            int length = random.Next(5, 7);
            int i;

            char[] buf = new char[length];
            for (i = 0; i < length; i++)
            {
                buf[i] = (char)('a' + random.Next(26));
            }
            return new string(buf);
        }

        /*
         --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Function:   verifyAndExpireCaptcha
         --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Check if a captcha submission passes the test, and expires it.
         --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public static bool VerifyAndExpireCaptcha(HttpContextBase context,
                                                  string challegeGuid,
                                                  string attempt)
        {
            //The solution is removed from session data to prevent repeated attempts
            string solution = (string)context.Session[SessionKeyPrefix + challegeGuid];
            context.Session.Remove(SessionKeyPrefix + challegeGuid);

            //The results of the attempt are returned
            return ((solution != null) && (attempt == solution));
        }
    }
}
