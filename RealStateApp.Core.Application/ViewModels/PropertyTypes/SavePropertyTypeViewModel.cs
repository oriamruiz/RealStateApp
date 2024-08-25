using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.PropertyTypes
{
    public class SavePropertyTypeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Debe ingresar la descripcion")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }

    }
}
