using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Web.Security;
using System.Web.Mvc;
using Domain.Abstract;
using Moq;
using LISTIT.Controllers;

namespace UnitTests
{
    public class CurrentUser
    {
        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: HomeControllerForUser
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Instantiates a controller for a given username.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public static Controller CreateHomeControllerForUser(IItemRepository itemRepository,
                                                IWListRepository listRepository,
                                                string userName)
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User.Identity.Name).Returns(userName);
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            Controller controller = new HomeController(itemRepository, listRepository);
            controller.ControllerContext = mock.Object;

            return controller;
        }
    }
}
