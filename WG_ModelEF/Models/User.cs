using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WishGrid.Models
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }
        
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool Validation { get; set; }

        public ICollection<Suggestion> Suggestions { get; set; }

        public ICollection<Votes> VotesUserSuggestions { get; set; }

        public ICollection<Reply> Replies { get; set; }

        public Tenants Tenant { get; set; }

        public int TenantId { get; set; }

        public Role Role { get; set; }
        public int RoleId { get; set; }

    }
}
