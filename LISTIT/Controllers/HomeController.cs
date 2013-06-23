using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using System.Web.UI;
using Domain.Entities;
using System.Web.UI.WebControls;
using LISTIT.Models;

namespace LISTIT.Controllers
{
    [HandleError][Authorize]
    public class HomeController : Controller
    {
        private IItemRepository itemRepository;
        private IWListRepository listRepository;
        private const string Chart="chart",LList="list", Edit="edit";

        public HomeController(IItemRepository itemsRepository, 
                              IWListRepository listRepository) {
            this.itemRepository = itemsRepository;
            this.listRepository = listRepository;
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Method:     Index
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Commands all actions required to set up data for the application.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public ActionResult Index(DateTime? focus, int wlistID=0, string index=Chart) {
            //Determines how the view will be displayed on load.
            List<WList> wlists;
            List<Item> items;
            DateTime refocus;

            refocus = focus?? DateTime.Now.Date;
            //The date is has a validility check by making sure it is on a sunday date.
            refocus = getClosestSunday(refocus.Date);

            //If the id for the list to be displayed has not been given, one is chosen.
            if (wlistID == 0) {
                //A list id for the user's week is chosen to be displayed.
                if ((wlists = listRepository.wlists.Where(x => x.sundayDate == refocus
                                                          && x.userName==User.Identity.Name)
                                                          .ToList<WList>()).Count() > 0){
                    //There is at least one list saved for this week, the first is chosen.
                    wlistID = wlists.First().wlistID;
                }
                else {
                    //There is no list for this week, one is generated and chosen.
                    listRepository.saveList(new WList{
                        userName = User.Identity.Name,
                        sundayDate = refocus,
                        name = "List 1"
                    });
                    wlistID = listRepository.wlists.First(x => x.sundayDate == refocus
                                                        && x.userName == User.Identity.Name).wlistID;
                }
            }

            //A compilation of wlists for this users chosen week is chosen
            wlists = listRepository.wlists.Where(x => x.sundayDate == refocus 
                                        && x.userName==User.Identity.Name).ToList<WList>();

            //The list of items for the week is generated.
            items = itemRepository.items.Where(x=>x.wlistID==wlistID).ToList<Item>();

            //A model is generated with the above data and an view is rendered.
            return View(new HomeIndexViewModel(items, wlists, wlistID, index));
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Method:     getClosestSunday
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Finds the closest previous day which was a sunday.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [NonAction]
        private DateTime getClosestSunday(DateTime given){
            //Validates the given date by casting it as a date.
            given = given.Date;
            //Scrolls through the previous dates until a sunday is found.
            while(given.DayOfWeek!=DayOfWeek.Sunday){
                given = given.AddDays(-1);
            }
            return given;
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Method:     saveItem
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Makes a change to the respository for a given item
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [HttpPost]
        public ActionResult saveItem(Item saving) {
            WList returnTo, validateList;   //Used to temporarily hold lists.

            //The item is validated to be the actual user's item
            if(saving.itemID!=0){
                //This is not a new item.
                validateList = listRepository.wlists
                                        .FirstOrDefault(x=>x.wlistID==saving.wlistID);
                if(validateList.userName != User.Identity.Name){
                    //The item is being added to a different user's list, Security breach 
                    //detected, return to main view.
                    TempData["message"]="Item failed to save, security access denied.";
                    return RedirectToAction("Index", new { index = Edit });
                }
            }

            //The item is saved to the repository and a success message is given.
            itemRepository.saveItem(saving);
            TempData["message"] = saving.name + " has been saved.";

            //The user is returned to the edit view of the list they were on.
            returnTo = listRepository.wlists.FirstOrDefault(x=>x.wlistID==saving.wlistID);
            return RedirectToAction("Index",new{
                                        focus=returnTo.sundayDate,
                                        wlistID=returnTo.wlistID,
                                        index=Edit
                                    });
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Method:     deleteItem
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    A given item is removed from the repository.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [HttpPost]
        public ActionResult deleteItem(int itemID) {
            WList validateList;             //Detects security breaches.
            Item validateItem;
            //Placeholders used to return the user to the correct spot.
            Item deleting;
            DateTime returnDate;
            string deletedName;
            int returnID;

            //Item is validated to exist, associated list exists, and associated user 
            //is current user.
            if((validateItem=itemRepository.items.FirstOrDefault(x=>x.itemID==itemID))
               !=null){
                //The item exists in the repository
                if((validateList=listRepository.wlists.
                                FirstOrDefault(x=>x.wlistID==validateItem.wlistID))!=null){
                    //The associated list exists
                    if(validateList.userName!=User.Identity.Name){
                        //item is being removed from a different user's list, security
                        //breach detected, return to screen.
                        TempData["message"]="Item failed to delete, security access denied";
                        return RedirectToAction("Index",new{index=Edit});
                    }
                }else{
                    //The associated list does not exist, users returned to main screen.
                    TempData["message"] = "Item failed to delete, list does not exist.";
                    return RedirectToAction("Index", new { index = Edit });
                }
            }else{
                //The given item does not exists, the user is returned to main screen.
                TempData["message"] = "Item failed to delete, does not exist.";
                return RedirectToAction("Index", new { index = Edit });
            }

            //The return information is saved
            deleting = itemRepository.items.FirstOrDefault(x=>x.itemID==itemID);
            returnID = deleting.wlistID;
            returnDate = listRepository.wlists
                            .FirstOrDefault(x=>x.wlistID==deleting.wlistID).sundayDate;
            deletedName = deleting.name;

            //The item is removed from the repository and a success message is generated.
            itemRepository.deleteItem(deleting);
            TempData["message"] = deletedName + " has been deleted.";

            //The user is returned to the edit view of the first list at the date.
            return RedirectToAction("Index",new{
                                            focus=returnDate,
                                            wlistID=returnID,
                                            index=Edit});
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Method:     editListName
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    The name of a given list is edited.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [HttpPost]
        public ActionResult editListName(WList editing) {
            //Validility check to assure current user is authentic
            if(editing.userName!=User.Identity.Name){
                //Name is being changed for a different user, security breach detected, user
                //is returned to the main screen.
                TempData["message"]="Failed to change name, security breach detected.";
                return RedirectToAction("Index", new { index = Edit });
            }
            
            string originalName = listRepository.wlists.First(x=>x.wlistID==editing.wlistID).name;
            
            listRepository.saveList(editing);

            TempData["message"] = originalName + "'s name has been changed to \"" + editing.name +"\".";
            
            //The user is directed to the index of the list being edited.
            return RedirectToAction("Index",new {
                                                    focus=editing.sundayDate,
                                                    wlistID=editing.wlistID,
                                                    index=Edit
                                                });
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Method:     deleteList
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Removes a list and all it's contents.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [HttpPost]
        public ActionResult deleteList(int listID){
            WList validateList;             //Checks for security breaches.
            //Placeholders for the return information.
            DateTime returnDate;
            string deletedName;
            WList deleting;

            //Validility check to make sure list exists, and no security breaches.
            if((validateList=listRepository.wlists.FirstOrDefault(x=>x.wlistID==listID))
                !=null){
                //The list exists.
                if(validateList.userName!=User.Identity.Name){
                    //Another user's list is being deleted, security breach detected, return
                    //to the index
                    TempData["message"]="Could not delete list, security access denied.";
                    return RedirectToAction("Index",new{index=Edit});
                }
            }else{
                //The list does not exist, the user is returned to the index.
                TempData["message"]="Could not remove list, does not exist.";
                return RedirectToAction("Index",new{index=Edit});
            }

            //Details about the list are saved for the return.
            deleting = listRepository.wlists.First(x=>x.wlistID==listID);

            //All items associated with the list are deleted.
            List<Item> listContents = itemRepository.items.Where(x=>x.wlistID==listID).ToList<Item>();
            foreach(var item in listContents){
                itemRepository.deleteItem(item);
            }

            //The information about where to return in the index is saved
            returnDate = deleting.sundayDate;
            deletedName =  deleting.name;
            
            //The list is removed from the repository.
            listRepository.removeList(deleting);

            //A confirmation message is sent to the user.
            TempData["message"] = deletedName + " and all contents have been deleted.";

            //The user is redirected to the index for editing the first list at the date.
            return RedirectToAction("Index",new{focus=returnDate, index=Edit});
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Method:     addList
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    A list is created for the user.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public ActionResult addList(DateTime sundayDate, string name){
            int returnID;                   //Returns the user to the created list.

            //Validility check to make sure there are not already nine lists for user week.
            if(listRepository.listsAtUserDate(User.Identity.Name,sundayDate).Count() >= 8){
                //No lists can be added, the user is returned with an error message.
                TempData["message"]= "Could not create list, already at max.";
                return RedirectToAction("Index",new{focus=sundayDate,index=Chart});
            }

            //A new list with given information is added to the repository
            listRepository.saveList(new WList{userName=User.Identity.Name,sundayDate=sundayDate,name=name});

            //The primary key of the newly added list is reverse looked up
            returnID = listRepository.wlists.First(
                                                x=>x.userName==User.Identity.Name
                                                && x.sundayDate==sundayDate
                                                && x.name==name).wlistID;

            TempData["message"]= name + " was created.";

            //User is redirected to the index focused on the newly created list.
            return RedirectToAction("Index", new{
                                                focus=sundayDate, 
                                                wlistID=returnID,
                                                index=Chart
                                            });
        }
    }
}
