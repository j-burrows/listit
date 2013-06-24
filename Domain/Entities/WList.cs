using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Class:      WList
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Purpose:    A LINQ object for a list that is associated to a certain week, the list
     |              may contain items which have an association with the list.
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     */
    [Table(Name="WLists")]
    public class WList
    {
        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     wlistID
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Primary key for identifying the week list
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [ScaffoldColumn(false)]
        [Column(IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.OnInsert)]
        public int wlistID { get; set; }

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     userName
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Forieng key for identifying user the list belongs to.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [Column][Required]
        public string userName { get; set; }

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     name
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
        */
        [Column][Required]
        public string name { get; set; }

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     sundayDate
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    The closest previous sunday date that the list belongs to.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
       */
        [Column][Required]
        public DateTime sundayDate { get; set; }
    }
}
