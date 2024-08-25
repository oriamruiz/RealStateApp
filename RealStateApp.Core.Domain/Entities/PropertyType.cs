using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RealStateApp.Core.Domain.Entities
{
    public class PropertyType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        [JsonIgnore]
        public ICollection<Property>? Properties { get; set; }
    }
}
