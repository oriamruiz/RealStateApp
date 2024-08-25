using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Account
{
    public class UpdateRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
