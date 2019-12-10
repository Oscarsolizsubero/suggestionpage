using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WishGrid.ViewModels.Shared;

namespace WishGrid.ViewModels
{
    public class VMReply : VMReplyAdd
    {
        public VMReply() { }

        public VMReply(VMReplyAdd parent)
        {
            Description = parent.Description;
        }

        [Required]
        public int Id { get; set; }

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
    }

    public class VMReplyAdd : VMBase
    {
        [Required]
        [StringLength(8000)]
        public string Description { get; set; }

        [Required]
        public virtual int IdAuthor { get; set; }

        [Required]
        public int IdSuggestion { get; set; }
    }
}
