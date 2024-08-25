using RealStateApp.Core.Domain.Entities;
using System.Text.Json.Serialization;

namespace RealStateApp.Core.Application.Dtos.Property
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [JsonIgnore]
        public int PropertyTypeId { get; set; }

        public RealStateApp.Core.Domain.Entities.PropertyType PropertyType { get; set; }
        [JsonIgnore]
        public int SaleTypeId { get; set; }
        public RealStateApp.Core.Domain.Entities.SaleType SaleType { get; set; }
        public decimal Price { get; set; }
        public double Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Description { get; set; }
        public ICollection<RealStateApp.Core.Domain.Entities.Improvement> PropertyImprovements { get; set; } = new List<RealStateApp.Core.Domain.Entities.Improvement>();
        [JsonIgnore]
        public ICollection<PropertyImprovement> Improvements { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }

    }
}
