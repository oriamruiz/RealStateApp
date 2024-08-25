using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Agent
{
    public class GetActiveAgentResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string? AccountImgUrl { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
