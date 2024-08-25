using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Domain.Entities
{
    public class FavoriteProperty
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
       
    }
}
