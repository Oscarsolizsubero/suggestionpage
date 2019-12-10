using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WishGrid.Models
{
    public class Suggestion
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int QuantityVote { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public ICollection<Votes> VotesUserSuggestions { get; set; }

        public ICollection<Reply> Replies { get; set; }

        public bool Deleted { get; set; }

        public Status Status { get; set; }

        public int StatusId { get; set; }
    }
}
