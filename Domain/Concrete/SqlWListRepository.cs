using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Abstract;
using Domain.Entities;
using System.Data.Linq;

namespace Domain.Concrete
{
    public class SqlWListRepository:IWListRepository
    {
        private string dbConnection;        //The connection to the database.
        private Table<WList> wlistsTable;   //Communicates with the database.

        public SqlWListRepository(string dbConnection) {
            this.dbConnection = dbConnection;
            //A connection to the database is established.
            wlistsTable = (new DataContext(dbConnection)).GetTable<WList>();
        }

        //Displays the contents of the database.
        public IQueryable<WList> wlists {
            get { return wlistsTable; }
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: saveList
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    to update changes, or insert lists into the database.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public void saveList(WList saving_wlist) {
            //The lists name is checked to be of valid length, and truncated if not.
            saving_wlist.name=(saving_wlist.name.Length<20)? 
                            saving_wlist.name:
                            saving_wlist.name.Substring(0,20);

            if(saving_wlist.wlistID==0){
                //The given list is not in table and to be added.
                wlistsTable.InsertOnSubmit(saving_wlist);
            }
            else if(wlistsTable.GetOriginalEntityState(saving_wlist)==null){
                //The given object changed if it is already in the table.
                WList updating = wlistsTable.FirstOrDefault(x=>x.wlistID==saving_wlist.wlistID);
                updating.name = saving_wlist.name;
                updating.sundayDate = saving_wlist.sundayDate;
            }

            //Any changes are saved to the table and the respected database table.
            wlistsTable.Context.SubmitChanges();
        }


        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Subroutine: removeList
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    to remove a given list from the dase.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public void removeList(WList removing_list) {
            //The lambda method for removing the wlist from the table is declared.
            wlistsTable.DeleteOnSubmit(removing_list);
            //The changes to the table and respected database are made.
            wlistsTable.Context.SubmitChanges();
        }

        /*
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Method:     listsAtUserDate
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    to compile a list of lists associated with a user and a date.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        public List<WList> listsAtUserDate(string username, DateTime date) {
            return wlistsTable.Where(x=>x.sundayDate==date && x.userName==username)
                              .ToList<WList>();
        }
    }
}
