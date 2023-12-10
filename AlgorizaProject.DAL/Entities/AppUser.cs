using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorizaProject.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? imageUrl { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
    }
}
