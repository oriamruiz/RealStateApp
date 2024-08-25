using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class HomePropertyViewModel
    {
        public int Id { get; set; }
        public string Cod { get; set; }
        public int ProductTypeId { get; set; }
        //public ProductType? ProductType { get; set; }
        public decimal balance { get; set; }
        public bool IsPrincipal { get; set; }
        public decimal Limit { get; set; }
        public decimal DebtAmmount { get; set; }
        public string ApplicationUserId { get; set; }
        public string UserName { get; set; }

    }
}
