using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Domain.Entities;
using LISTIT.Controllers;
using System.Web.Mvc;

namespace UnitTests
{
    [TestFixture]
    public class EditItemTests
    {
        [Test]
        public void valid_change_is_recorded(){
            //Arrange:  A repository with one item and a controller are created.
            WList list = new WList { wlistID = 1, name = "list", userName = "user",
                                    sundayDate = DateTime.Now.Date };
            Item item = new Item { wlistID = 1, name = "item",
                                    cost = 1, quantity = 1};
            var itemRepository = UnitTestHelpers.MockItemRepository(item);
            var listRepository = UnitTestHelpers.MockWListRepository(list);
            HomeController controller = UnitTestHelpers.HomeControllerForUser(itemRepository,listRepository,"user");

            //Act:      the item is edited
            Item edited = new Item{wlistID=1,name="item",cost=2,quantity=1,itemID=item.itemID};
            controller.saveItem(edited);

            controller.User.Identity.Name.ShouldEqual("user");
            
            //Assert:  there is only one item in the repository, and it has a cost of 2.
            itemRepository.items.Count().ShouldEqual(1);
            itemRepository.items.First().cost.ShouldEqual(2);
        }

        [Test]
        public void changes_to_other_users_items_dont_save(){
            //Arrange:  A repository with one item and a controller are created.
            WList list = new WList { wlistID = 1, name = "list", userName = "user",
                                    sundayDate = DateTime.Now.Date };
            Item item = new Item { wlistID = 1, name = "item", cost = 1, quantity = 1 };
            var itemRepository = UnitTestHelpers.MockItemRepository(item);
            var listRepository = UnitTestHelpers.MockWListRepository(list);
            var controller = UnitTestHelpers.HomeControllerForUser(itemRepository,
                                                        listRepository, "differentUser");

            //Act:      The item is edited by the different user.
            //item.cost
            //controller.saveItem(new Item);

        }
    }
}
