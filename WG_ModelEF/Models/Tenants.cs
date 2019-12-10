using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace WishGrid.Models
{
    public class Tenants 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string NameTenants { get; set; }

        public string Address { get; set; }

        public string Nit { get; set; }

        public string Phone { get; set; }

        public string URLOrigin { get; set; }

        public bool Status { get; set; }
        public bool Moderation { get; set; }
        public string URLImage { get; set; }
        public string URLTenant { get; set; }

        public User User { get; set; }


    }
}
