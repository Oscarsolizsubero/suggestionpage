using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WishGrid.ViewModels.Shared;

namespace WishGrid.ViewModels
{
    public class VMStatus : VMBase
    {
        [Required]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
