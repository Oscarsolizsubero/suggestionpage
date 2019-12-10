using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WishGrid.ViewModels.Shared;

namespace WishGrid.ViewModels
{
    public class VMVote 
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int SuggestionId { get; set; }

    }
}
