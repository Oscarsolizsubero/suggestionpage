using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using WishGrid.ViewModels.Shared;

namespace WishGrid.ViewModels
{
    public class VMSuggestion : VMSuggestionAdd
    {
        public VMSuggestion() { }

        public VMSuggestion(VMSuggestionAdd parent)
        {
            Title = parent.Title;
            Description = Description;
        }

        public int Id { get; set; }

        public int QuantityVote { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public VMUserSimple Author { get; set; }

        public override int IdAuthor
        {
            get
            {
                return Author.Id;
            }
            set
            {
                Author = new VMUserSimple();
                Author.Id = value;
            }
        }
        public bool IsVoted { get; set; }

        public VMStatus Status { get; set; }

        public int StatusId
        {
            get
            {
                return Status.Id;
            }
            set
            {
                Status = new VMStatus();
                Status.Id = value;
            }
        }
    }

    public class VMSuggestionAdd : VMBase
    {
        [Required]
        [StringLength(125)]
        public string Title { get; set; }

        [Required]
        [StringLength(8000)]
        public string Description { get; set; }

        [Required]
        public virtual int IdAuthor { get; set; }
    }

    public class VMSuggestionEditStatus
    {
        
        public int Id { get; set; }

        public int StatusId { get; set; }

        public int UserId { get; set; }

    }
}
