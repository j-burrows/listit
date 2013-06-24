using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Abstract
{
    /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Interface:  IWListRepository
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Purpose:    Create an abstract class to decouple associations later and make
     |              dependancy injection possible.
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     */
    public interface IWListRepository
    {
        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     wlists
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    An enumerable object containing all week lists in the database, can
         |              be operated on using ADO functions.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        IQueryable<WList> wlists{get;}

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     saveList
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Either add or save changes to a list. If the given week list has
         |              an ID of 0, then it will be added as a new entry, otherwise saved.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        void saveList(WList saving_wlist);

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     removeList
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    removes an instance of a week list from the database.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        void removeList(WList removing_wlist);

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     listsAtUserDate
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    returns an enumerable object of week lists at a given sunday.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        List<WList> listsAtUserDate(string username, DateTime date);
    }
}
