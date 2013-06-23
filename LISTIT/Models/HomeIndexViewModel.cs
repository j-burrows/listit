using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;
using System.Data.Linq;

namespace LISTIT.Models
{
    /*
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Class:      HomeIndexViewModel
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Purpose:    Provide all information needed to the view for rendering the application
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     */
    public class HomeIndexViewModel
    {
        public string index{get;set;}          //Determines the focus of the page.

        //The list of items associated with the selected list.
        public List<Item> items { get; set; }

        //The compilation of lists associated with the user in the selected week.
        public List<WList> wlists { get; set; }
        
        //The next name avaliable to name a generated list.
        public string nextAvaliableName { get; set; }

        //Determines which buttons will be made avaliable to the user.
        public bool canAddList { get; set; }
        public bool canRemoveList { get; set; }

        //Will determine the bottom row of buttons for scrolling through dates.
        public DateTime[] prevDates{get;set;}
        public DateTime[] nextDates{get;set;}
        public DateTime currentDate{get;set;}

        //Used to determine the currently selected week.
        public int currentListID{get;set;}

        public HomeIndexViewModel(List<Item> items, List<WList> wlists, int currentListID, string index)
        {
            this.index = index;
            DateTime currentDate = wlists.First(x => x.wlistID == currentListID).sundayDate;
            this.currentListID = currentListID;
            this.items = items;
            this.wlists = wlists;
            int i,j;
            string template= "List ", generate="";

            //The sundays previous to the selected week are found
            prevDates = new DateTime[3] { currentDate.AddDays(-7),currentDate.AddDays(-14),
                                        currentDate.AddDays(-21)};
            //The sundays next to the selected week are found.
            nextDates = new DateTime[3]{currentDate.AddDays(21),currentDate.AddDays(14),
                                        currentDate.AddDays(7)};
            //The current date to be displayed.
            this.currentDate = currentDate;

            //Based on the amount of associated lists with the current week, it is
            //determined if the user has the options to add or remove lists.
            canAddList = (wlists.Count() < 8);
            canRemoveList = (wlists.Count() > 1);

            //The next avaliable name for a list name is found ("List 1", "List 2"...)
            for (i = 1; i < 9 && canAddList; i++) { 
                generate = template + i.ToString();
                if (wlists.Where(x => x.name == generate).Count() == 0){
                    nextAvaliableName = generate;
                    break;
                }
            }
        }
    }
}
