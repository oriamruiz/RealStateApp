using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class PropertyViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        public string MainImageUrl { get; set; }
        public string? ImageUrl2 { get; set; }
        public string? ImageUrl3 { get; set; }
        public string? ImageUrl4 { get; set; }
        public int SaleTypeId { get; set; }
        public SaleType SaleType { get; set; }
        public ICollection<PropertyImprovement> Improvements { get; set; }
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public double Size { get; set; }
        public string AgentId { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
        public bool IsFavorite { get; set; } = false;


    }
}
