<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<LISTIT.Models.HomeIndexViewModel>" %>
<%@ Import Namespace="LISTIT.Helpers" %>
<%  /*
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Filename:   ChartItems.ascx
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Purpose:    Renders the chart graph of the items.
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
    */ %>

<header class="chartHeader">
    <h2><%: Model.wlists.First(x=>x.wlistID==Model.currentListID).name.ToUpper() %></h2>
</header>
<canvas id="chartCanvas" height="500" width="500"></canvas>

<script type="text/javascript" src="../../Scripts/chartCanvas.js">
</script>
<%
    StringBuilder builder = new StringBuilder();
    builder.Append("<script type=\"text/javascript\">");
    builder.Append("var names = new Array;");
    foreach(var getName in Model.items){
        builder.Append("names.push('" + getName.name + "');");
    }
    builder.Append("var costs = new Array;");
    foreach (var getCost in Model.items)
    {
        builder.Append("costs.push('"+ getCost.cost + "');");
    }
    builder.Append("var quantities = new Array;");
    foreach (var getQuants in Model.items)
    {
        builder.Append("quantities.push('" + getQuants.quantity + "');");
    }
    builder.Append("var names = new Array;");
    foreach (var getNames in Model.items)
    {
        builder.Append("names.push('" + getNames.name + "');");
    }
    builder.Append("chartPie(costs,quantities,names);");
    builder.Append("</script>");
    Response.Write(builder);
 %>