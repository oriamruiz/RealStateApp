using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Core.Application.ViewModels.Users
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Debe ingresar el apellido")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe ingresar su cedula")]
        [DataType(DataType.Text)]
        public string DNI { get; set; }

        [Required(ErrorMessage = "Debe ingresar el email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas deben coincidir")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public decimal? AditionalAmmount { get; set; } = 0;
        public bool HasError { get; set; }
        public string? Error { get; set; }
        public List<string> Roles { get; set; }
    }
}
