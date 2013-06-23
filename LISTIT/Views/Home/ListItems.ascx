<%@ Control Language="C#" 
            Inherits="System.Web.Mvc.ViewUserControl<LISTIT.Models.HomeIndexViewModel>" %>
<%  /*
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Filename:   ListItems.ascx
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Purpose:    Renders a copy pastable list of items and their properties.
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
    */ %>

<header class="chartHeader">
    <h2><%: Model.wlists.First(x=>x.wlistID==Model.currentListID).name.ToUpper() %></h2>
</header>
<% decimal totalCost=0;
   foreach(var item in Model.items){
        totalCost+=item.cost * item.quantity;
} Response.Write("Total Cost of list: "+totalCost.ToString("C"));%>

<%//If there are items in the list, a special message is displayed. %>
<% if(Model.items.Count() == 0){ %>
    <br><br>
    The list is empty, go to edit list to add items.
<% } %>

<%//The name, cost, quantity, and total cost of every item is displayed. %>
<% foreach(var item in Model.items){ %>
    <br>
    <hr /><br>
    <%: item.name %>
    <br>
    <%: item.cost.ToString("c") %> (<%: item.quantity.ToString() %>)
    <br>
    Total: <%: (item.cost * item.quantity).ToString("c") %>
<%} %>