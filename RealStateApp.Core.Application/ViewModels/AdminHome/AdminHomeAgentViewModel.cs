using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.AdminHome
{
    public class AdminHomeAgentViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Properties { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
