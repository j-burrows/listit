using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Abstract
{
    /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Interface:  IItemRepository
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Purpose:    Create an abstract class to decouple associations later and make
     |              dependancy injection possible.
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     */
    public interface IItemRepository
    {
        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     items
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    An enumerable object containing all items in the database, can be 
         |              operated on using ADO functions.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        IQueryable<Item> items { get; }

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     saveItem
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Either add or save changes to an item. If the given item has an ID 
         |              of 0, then it will be added as a new entry, otherwise saved.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        void saveItem(Item adding);

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     deleteItem
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    removes an instance of an item from the database.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        void deleteItem(Item removing);

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     itemsFromList
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    returns an enumerable object of items in a given list.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        List<Item> itemsFromList(int listID);
    }
}
