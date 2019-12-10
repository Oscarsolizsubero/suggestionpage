using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WishGrid.ViewModels.Shared;

namespace WishGrid.ViewModels
{
    public class VMUser : VMUserSimple
    {
        public VMUser() { }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters.")]

        public string Password { get; set; }
        public int IdTenant { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public override string FullName
        {
            get
            {
                return Name;
            }
            set
            {
                value = Name;
            }
        }
    }

    public class VMUserEncritpted
    {
        public VMUser User { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public VMUserEncritpted(VMUser user)
        {
            User = user;
        }
    }

    public class VMUserSimple : VMBase
    {
        public VMUserSimple() { }
        public VMUserSimple(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public virtual string FullName { get; set; }
    }

    public class VMUserforList : VMUserSimple
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public bool Moderator { get; set; }
    }

    public class VMUserforListEdit : VMUserforList
    {
        public int LoggedUserId { get; set; }
        public int LoggedUserRole { get; set; }
        public string LoggedUserTenant { get; set; }
    }

    public class VMUserAdd : VMUserforList
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Tenant { get; set; }
    }

    public class VMUserPassword : VMUserSimple
    {
        public string oldPassword { get; set; }
        public string password { get; set; }
    }
}
