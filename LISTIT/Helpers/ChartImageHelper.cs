using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;
using LISTIT.Models;
using System.Text;

namespace LISTIT.Helpers
{
    public static class ChartImageHelper{
       /*
        private const string ImgFormat = "<img class=\"chartImageStyle\"src=\"{0}\"  />";

        public static MvcHtmlString ChartImage(this HtmlHelper html){
            //An image tag is created for the chart image.
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            //A url to the constructed chart image is taken
            //return MvcHtmlString.Create(info.items.Count().ToString());
            List<int> a= new List<int>{0,1};
            string url = urlHelper.Action("Render", "ChartImage", a=a);
            //The image tag is formated to include the url
            string htmlToDisplay = string.Format(ImgFormat,url);

            //The finished image tag is returned in HTML format.
            return MvcHtmlString.Create(htmlToDisplay);
        }*/

        public static MvcHtmlString ChartImage(this HtmlHelper html, List<Item> toDisplay)
        {/*
            StringBuilder builder = new StringBuilder();
            builder.Append("<script type=\"text/javascript\">");
            builder.Append("var names = new Array;");
            foreach(Item getName in toDisplay){
                builder.Append("names.push('" + getName.name + "');");
            }
            builder.Append("var costs = new Array;");
            foreach(Item getCost in toDisplay){
                builder.Append("costs.push('"+ getCost.cost + "');");
            }
            builder.Append("var quantities = new Array;");
            foreach (Item getQuants in toDisplay)
            {
                builder.Append("quantities.push('" + getQuants.quantity + "');");
            }
            builder.Append("chartPie(costs,quantities);");
            builder.Append("</script>");
            return MvcHtmlString.Create(builder.ToString());*/
            return MvcHtmlString.Create("<a>Hello, world!</a>");
        }
    }
}