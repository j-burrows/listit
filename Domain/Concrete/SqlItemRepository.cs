using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Abstract;
using Domain.Entities;
using System.Data.Linq;

namespace Domain.Concrete
{
    public class SqlItemRepository:IItemRepository
    {
        private string dbConnection;        //The connection to the database.
        private Table<Item> itemsTable;     //Communicates with the database.

        public SqlItemRepository(string dbConnection) {
            this.dbConnection = dbConnection;
            //A connection to the database is established.
            itemsTable = (new DataContext(dbConnection)).GetTable<Item>();
        }

        //Used to view the contents of the database.
        public IQueryable<Item> items {
            get { return itemsTable; }
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: saveItem
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    to update changes, or insert lists into the database.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public void saveItem(Item saving_item)
        {
            //The list name is made sure to be of a correct length:
            saving_item.name = (saving_item.name.Length< 20)? 
                                saving_item.name:
                                saving_item.name.Substring(0,20);

            if (saving_item.itemID == 0){ 
                //The given item is to be added to the table
                itemsTable.InsertOnSubmit(saving_item);
            }
            else if (itemsTable.GetOriginalEntityState(saving_item) == null){
                //The item is already in the table, and is to be edited (doesnt save if 
                // there are no changes).
                Item changing = items.FirstOrDefault(x=>x.itemID==saving_item.itemID);
                changing.name = saving_item.name;
                changing.cost = saving_item.cost;
                changing.quantity = saving_item.quantity;
            }

            //Any to the table are made and sent to the respected database table.
            itemsTable.Context.SubmitChanges();
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: deleteItem
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    to remove a given item from the database.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public void deleteItem(Item removing_item)
        {
            //A labmda method is made to remove the remove the item on submit
            itemsTable.DeleteOnSubmit(removing_item);
            //The changes to the table are made and sent to the respected database table.
            itemsTable.Context.SubmitChanges();
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Method:     itemsFromList
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Compiles a list of items from a given listIID
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public List<Item> itemsFromList(int wlistID)
        {
            //Finds all items in the table that are associated with the given list.
            return itemsTable.Where(x=>x.wlistID==wlistID).ToList<Item>();
        }
    }
}
