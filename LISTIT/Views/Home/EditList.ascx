<%@ Control Language="C#" 
            Inherits="System.Web.Mvc.ViewUserControl<LISTIT.Models.HomeIndexViewModel>" %>
<%  /*
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Filename:   EditList.ascx
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Purpose:    Generate forms for editing the properities of the selected list.
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
    */ %>

<%//A form for deleting the selected list is generated.%>
<% using(Ajax.BeginForm("deleteList","Home",  null, new{@class="needsConfirmation"})){%>
   <%: Html.Hidden("listID",Model.currentListID) %>
   <input type="submit" value="Delete List"  />
<%} %>

<%//The form for editing the list's name is generated. %>
<% using (Html.BeginForm("editListName", "Home")){ %>
    <%:Html.Hidden("wlistID",Model.currentListID) %>
    <%:Html.Hidden("userName", Model.wlists.First(x => x.wlistID == Model.currentListID)
                                            .userName)%>
    <%:Html.Hidden("sundayDate", Model.wlists.First(x => x.wlistID == Model.currentListID)
                                            .sundayDate)%>
    
    <label for="name">Change List Name</label>
    <%:Html.TextBoxFor(x=>x.wlists.First(y=>y.wlistID==x.currentListID).name,
                        new{maxlength="20",required="true"}) %>
    <input type="submit" value="Save Name" />
<%} %>

<h2>Enter a New Item</h2>
<%//A form for generating a form for creating a new item is generated. %>
<% using(Html.BeginForm("saveItem","Home")){%>
    <fieldset>
        <legend>New Item</legend>
        
        <%: Html.Hidden("wlistID",Model.currentListID) %>

        <label for="name">Item name</label>
        <input type=text id="name" name="name" maxlength="20" required/>

        <label for="cost">Cost $</label>
        <input type="number" id="cost" name="cost" step="0.01" min="0.01" required/>

        <label for="quantity">Quantity</label>
        <input type="number" id="quantity" name="quantity" step="1" min="1" required/>
        <input type="submit" value="Add Item"  />
    </fieldset>
<%} %>

<%//If there are any items, a form for editing the properties of each is generated %>
<% if(Model.items.Count > 0){ %>
<h2>Edit Items</h2>
<%} %>
<% foreach(var item in Model.items){ %>
    <%//A form containing options to edit the name, cost, and quantity is generated. %>
    
    <% using(Html.BeginForm("saveItem","Home")){%>
        <%: Html.Hidden("itemID",item.itemID) %>
        <%: Html.Hidden("wlistID",Model.currentListID) %>
        <fieldset>
        <legend><%: item.name %></legend>
        <label for="name">Item name</label>
        <input type=text id="Text1" name="name" value="<%: item.name %>" required/>

        <label for="cost">Cost $</label>
        <input type="number" id="Number1" name="cost" step="0.01" min="0.01" value="<%: item.cost.ToString() %>" required/>

        <label for="quantity">Quantity</label>
        <input type="number" id="Number2" name="quantity" step="1" min="1" value="<%: item.quantity.ToString() %>" required/>
        <input type="submit" value="Submit Changes"  />
    <%} %>
    
    <%//A form for deleting the option, and requiring confirmation, is generated. %>
    <% using(Ajax.BeginForm("deleteItem","Home",  null, new{@class="needsConfirmation"})){%>
       <%: Html.Hidden("itemID",item.itemID) %>
       <input type="submit" value="Delete Item"  />
    <%} %>
    </fieldset>
<%} %>