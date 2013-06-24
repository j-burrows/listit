using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Class:      Item
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     |  Purpose:    A LINQ object for retrieving an item from the database
     +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
     */
    [Table(Name="Items")]
    public class Item
    {
        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     itemID
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Primary key for identifying item
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [ScaffoldColumn(false)]
        [Column(IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.OnInsert)]
        public int itemID { get; set; }

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     wlistID
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Purpose:    Foreign key to the associated week list, item belongs to that list.
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [ScaffoldColumn(false)][Column]
        public int wlistID { get; set; }

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     name
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [Required(ErrorMessage="Please enter the items name.")][Column]
        public string name { get; set; }

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     cost
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Note:       Must be above or $0.01
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [Required(ErrorMessage="Please enter an item cost.")]
        [Range(0.01,double.MaxValue,ErrorMessage="Please enter a positive non-zero price.")]
        [Column]
        public decimal cost { get; set; }

        /*-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         |  Member:     quantity
         +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
         */
        [Required(ErrorMessage="Please enter the quantity of the item.")][Column]
        public int quantity { get; set; }
    }
}
