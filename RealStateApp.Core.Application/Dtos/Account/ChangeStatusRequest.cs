using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Account
{
    public class ChangeStatusRequest
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
