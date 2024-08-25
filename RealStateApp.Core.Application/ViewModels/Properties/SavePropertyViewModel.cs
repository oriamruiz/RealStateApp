using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Helpers.Validations;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class SavePropertyViewModel
    {

        public int Id { get; set; }
        public string? Code { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe colocar el tipo de la propiedad")]
        public int PropertyTypeId { get; set; }
        public List<PropertyType>? PropertyTypes { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe colocar el tipo de venta de la propiedad")]
        public int SaleTypeId { get; set; }
        public List<SaleType>? SaleTypes { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe colocar el precio de la propiedad")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Debe colocar la descripcion de la propiedad")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Debe colocar el tamaño en M")]
        public double Size { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Debe colocar la cantidad de habitaciones")]
        public int Bedrooms { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Debe colocar la cantidad de baños")]
        public int Bathrooms { get; set; }

        [MinMaxLengthListImprovements(1, int.MaxValue, ErrorMessage = "Debe seleccionar al menos una mejora")]
        public List<int> ImprovementsIds { get; set; } = new();
        public List<Improvement>? Improvements { get; set; } 

        public string? MainImageUrl { get; set; }
        
        public List<string>? optionalimages { get; set; }
        public string? ImageUrl2 { get; set; }
        public string? ImageUrl3 { get; set; }
        public string? ImageUrl4 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? MainImageFile { get; set; }

        [DataType(DataType.Upload)]
        [MinMaxLengthListImage(0, 3, ErrorMessage ="no puedes agregar mas de 3")]
        public List<IFormFile>? optionalImagesFile { get; set; } = new();

        public string AgentId { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
