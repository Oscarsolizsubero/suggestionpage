using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using WishGrid.ViewModels.Shared;

namespace WishGrid.ViewModels
{
    public class VMTenant : VMBase
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string TenantURL { get; set; }

        public bool Moderation { get; set; }
    }
    public class VMTenantEdit : VMTenant
    {
        public int LoggedUserId { get; set; }
        public int LoggedUserRole { get; set; }
    }

    public class VMTenantImage : VMTenant
    {
        public string URLImage { get; set; }
        public string URLTenant { get; set; }
    }
}
