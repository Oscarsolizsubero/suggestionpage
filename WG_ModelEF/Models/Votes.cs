using System;
using System.Collections.Generic;
using System.Text;
using WishGrid.Models;

namespace WishGrid.Models
{
    public class Votes
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int SuggestionId { get; set; }
        public Suggestion Suggestion { get; set; }
    }
}
