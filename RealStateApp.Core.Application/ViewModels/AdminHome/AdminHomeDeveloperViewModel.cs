using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.AdminHome
{
    public class AdminHomeDeveloperViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
