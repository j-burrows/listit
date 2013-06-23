<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
        Inherits="System.Web.Mvc.ViewPage<LISTIT.Models.HomeIndexViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <%: Model.currentDate.ToString("MMMM dd").ToUpper()%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%  /*
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Filename:   Index.aspx
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Purpose:    The main page to be rendered, calls all other partial views, and 
     |              additional buttons for general behaviours.
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
    */ %>

<%/*The button for revealing the first article, and the first article iteself is generated,
    if the page index is set to zero it is chosen to be displayed.*/ %>
<div class="button articleButton">
    <label class="buttonName">CHART<br>VIEW</label>
</div>
<article class="articleStyle <%if(Model.index=="chart"){%>selected<%}%>">
    <% Html.RenderPartial("ChartItems"); %>
</article>

<%/*The button for revealing the second article, and the second article iteself is generated
    if the page index is set to one it is chosen to be displayed.*/ %>
<div class="button articleButton">
    <label class="buttonName">LIST<br>VIEW</label>
</div>
<article class="articleStyle <%if(Model.index=="list"){%>selected<%}%>">
    <% Html.RenderPartial("ListItems"); %>
</article>

<%/*The button for revealing the third article, and the third article iteself is generated,
    if the page index is set to one it is chosen to be displayed.*/ %>
<div class="button articleButton">
    <label class="buttonName">EDIT<br>VIEW</label>
</div>
<article class="articleStyle <%if(Model.index=="edit"){%>selected<%}%>">
    <% Html.RenderPartial("EditList"); %>
</article>

<%//If an add list operation is valid, an add list button is generated for that action.%>
<% if(Model.canAddList){ %>
    <a class="button" href="<%:Url.Action("addList","Home",new{sundayDate=Model.currentDate,
                                            name=Model.nextAvaliableName}) %>" >
        <label class="buttonName">ADD<br>LIST</label>                                        
    </a>
<%} %>

<%//If there are other lists in association with the week, a change list button is made.%>
<% if(Model.canRemoveList){ %>
    <div class="listButtonContainer">
    <%foreach(var list in Model.wlists){ %>
        <a class="listButton" href="<%: Url.Action("Index","Home",new{focus=Model.currentDate,wlistID=list.wlistID,index="chart"}) %>">
            <label class="buttonName"><%: list.name %></label>
        </a>

    <%} %>
    </div>
<% } %>

<%//A row of buttons is created for scrolling through the previous and next dates. %>
<div class="dateBar">
    <%//Buttons for each of the previous dates are generated.%>
    <% foreach(var date in Model.nextDates){ %>
        <a class="dateButton" href="<%: Url.Action("Index","Home",new{focus=date}) %>">
            <label class="buttonName"><%: date.ToString("MMMM dd").ToUpper() %></label>
        </a>
    <% } %>
    <%//A placeholder is created for the user to know what date is currently selected %>
    <a class="dateButton currentDate" href="<% Url.Action("Index","Home",new{focus=Model.currentDate}); %>">
      <label class="buttonName"><%:Model.currentDate.ToString("MMMM dd").ToUpper()%></label>
    </a>    

    <%//Buttons for each of the next dates are generated. %>
    <% foreach(var date in Model.prevDates){ %>
        <a class="dateButton" href="<%: Url.Action("Index","Home",new{focus=date}) %>">
            <label class="buttonName"><%: date.ToString("MMMM dd").ToUpper()%></label>
        </a>
    <% } %>
</div>

</asp:Content>
