using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Users
{
    public class CreateAgentOrClientViewModel
    {
        public string? Id { get; set; }

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
        public IFormFile ImageFile { get; set; }
        public string? AccountImgUrl { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Debe ingresar el email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas deben coincidir")]
        [Required(ErrorMessage = "Debe repetir la contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de usuario")]
        [DataType(DataType.Text)]
        public string Role {  get; set; } 
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
