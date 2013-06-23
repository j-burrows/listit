using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table(Name="Items")]
    public class Item
    {
        [ScaffoldColumn(false)][Column(IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.OnInsert)]
        public int itemID { get; set; }

        [ScaffoldColumn(false)][Column]
        public int wlistID { get; set; }
        
        [Required(ErrorMessage="Please enter the items name.")][Column]
        public string name { get; set; }

        [Required(ErrorMessage="Please enter an item cost.")]
        [Range(0.01,double.MaxValue,ErrorMessage="Please enter a positive non-zero price.")]
        [Column]
        public decimal cost { get; set; }

        [Required(ErrorMessage="Please enter the quantity of the item.")][Column]
        public int quantity { get; set; }
    }
}
