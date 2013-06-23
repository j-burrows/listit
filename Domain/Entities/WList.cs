using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table(Name="WLists")]
    public class WList
    {
        [ScaffoldColumn(false)]
        [Column(IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.OnInsert)]
        public int wlistID { get; set; }

        [Column][Required]
        public string userName { get; set; }

        [Column][Required]
        public string name { get; set; }

        [Column][Required]
        public DateTime sundayDate { get; set; }
    }
}
