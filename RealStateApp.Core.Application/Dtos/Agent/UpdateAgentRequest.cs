using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Agent
{
    public class UpdateAgentRequest
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public IFormFile ImageFile { get; set; }
        public string? AccountImgUrl { get; set; }
        public List<string> Role { get; set; }
        
    }
}
