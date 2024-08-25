using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Agents
{
    public class DeleteAgentViewModel
    {
        public string Id { get; set; }
        public List<int> Properties { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
