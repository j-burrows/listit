using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Domain.Abstract;
using Domain.Entities;
using Moq;
using System.Web.Mvc;
using System.Web.Routing;
using LISTIT.Controllers;

namespace UnitTests
{
    public static class UnitTestHelpers
    {
        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: ShouldEqual
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    A helper method to cause an interrupt if two values are not equal.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public static void ShouldEqual<T>(this T actualValue, T expectedValue){
            Assert.AreEqual(expectedValue, actualValue);
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Function:   MockWListRepository
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    To create a concrete instance of a abstract wlist repository for 
         |              testing.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public static IWListRepository MockWListRepository(params WList[] lists){
            var mockWListRepository = new Mock<IWListRepository>();
            mockWListRepository.Setup(x=>x.wlists).Returns(lists.AsQueryable());

            return mockWListRepository.Object;
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Function:   MockItemRepository
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    To create a concrete instance of a abstract items repository for 
         |              testing.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public static IItemRepository MockItemRepository(params Item[] items){
            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(x=>x.items).Returns(items.AsQueryable());

            return mockItemRepository.Object;
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: ShouldBeRedirectedTo
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    A helper method to cause an interrupt if a page is not redirected
         |              to an expected route.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public static void ShouldBeRedirectedTo(this ActionResult actionResult, object expectedRouteValues){
            var actualValues=((RedirectToRouteResult)actionResult).RouteValues;
            var expectedValues=new RouteValueDictionary(expectedRouteValues);

            foreach(string key in expectedValues.Keys){
                actualValues[key].ShouldEqual(expectedValues[key]);
            }
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: ShouldBeView
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    A helper method to cause an interrupt if a page is not on a set
         |              view.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public static void ShouldBeView(this ActionResult actionResult, string viewName){
            Assert.IsInstanceOf<ViewResult>(actionResult);
            ((ViewResult)actionResult).ViewName.ShouldEqual(viewName);
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: ShouldBeDefaultView
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    A helper method to cause an interrupt if a page is not the index
         |              view.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public static void ShouldBeDefaultView(this ActionResult actionResult){
            actionResult.ShouldBeView(string.Empty);
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: HomeControllerForUser
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Instantiates a controller for a given username.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public static HomeController HomeControllerForUser(IItemRepository itemRepository, 
                                                IWListRepository listRepository, 
                                                string userName){
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(p=>p.HttpContext.User.Identity.Name).Returns(userName);
            mock.SetupGet(p=>p.HttpContext.Request.IsAuthenticated).Returns(true);

            var controller = new HomeController(itemRepository,listRepository);
            controller.ControllerContext = mock.Object;

            return controller;
        }
    }
}
