using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Users
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }

    }
}
