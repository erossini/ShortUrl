using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Entities
{
    public class Category
    {
        [Key]
        [Column("IDCategory")]
        public int IDCategory { get; set; }

        [Required]
        [Column("CategoryName")]
        public string CategoryName { get; set; }

        [Column("Parent")]
        public int? ParentID { get; set; }

        [Column("Username")]
        public string Username { get; set; }
    }
}