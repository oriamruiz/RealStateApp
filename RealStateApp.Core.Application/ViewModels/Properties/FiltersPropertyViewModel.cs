using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class FiltersPropertyViewModel
    {
        public int PropertyTypeIdfilter { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int BedromsQuantity { get; set; }
        public int BathroomsQuantity { get; set; }
    }
}
