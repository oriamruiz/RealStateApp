using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Agents
{
    public class UpdateAgentViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe ingresar el apellido")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe ingresar su telefono")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }
        public string? AccountImgUrl { get; set; }

        public List<string>? Role { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
