using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using LISTIT.Helpers;
using LISTIT.Models;

namespace LISTIT.Controllers
{
    public class ChartImageController : Controller
    {
        private const int ImageWidth = 500, ImageHeight=500;
        private const string FontFamily="Rockwell";
        private static readonly Color BackgroundColor = ColorTranslator.FromHtml("#EFEFEF");
        //
        // GET: /ChartImage/
        public void Render(List<int> a){
            int pieWidth = 400, pieHeight=400;
            using(Bitmap bmp = new Bitmap(ImageWidth, ImageHeight))
            using(Graphics g = Graphics.FromImage(bmp))
            using(Font font = new Font(FontFamily, 1f)){
                g.Clear(BackgroundColor);

                GraphicsPath path = new GraphicsPath();
                //return Content(a.Count().ToString());
                if (a[0]==0){
                    //TODO: add text that chart is empty.
                    SolidBrush p = new SolidBrush(ColorTranslator.FromHtml("#40BF40"));
                    path.AddEllipse((ImageWidth - pieWidth) / 2, (ImageHeight - pieHeight) / 2, pieWidth, pieHeight);
                    g.FillPath(p, path);
                }
                

                g.Flush();
                Response.ContentType = "image/png";
                using(var memoryStream = new MemoryStream()){
                    bmp.Save(memoryStream, ImageFormat.Png);
                    memoryStream.WriteTo(Response.OutputStream);
                }
            }
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
