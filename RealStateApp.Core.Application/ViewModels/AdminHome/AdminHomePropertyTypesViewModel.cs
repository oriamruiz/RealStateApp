using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.AdminHome
{
    public class AdminHomePropertyTypesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Propertiesquantity { get; set; } = 0;
        public ICollection<Property>? Properties { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
