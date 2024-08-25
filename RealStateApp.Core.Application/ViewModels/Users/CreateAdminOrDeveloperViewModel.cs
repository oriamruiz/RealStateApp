using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Users
{
    /// <summary>
    /// parametros para crear un agente/admin
    /// </summary>
    public class CreateAdminOrDeveloperViewModel
    {
        /// <example>
        /// oriam
        /// </example>
        [SwaggerParameter(Description = "nombre")]
        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        /// <example>
        /// ruiz
        /// </example>
        [SwaggerParameter(Description = "apellido de usuario")]
        [Required(ErrorMessage = "Debe ingresar el apellido")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        /// <example>
        /// 45875843
        /// </example>
        [SwaggerParameter(Description = "nombre de usuario")]

        [Required(ErrorMessage = "Debe ingresar su cedula")]
        [DataType(DataType.Text)]
        public string Dni { get; set; }
        /// <example>
        /// oriam_22@gmail.com
        /// </example>
        [SwaggerParameter(Description = "correo")]

        [Required(ErrorMessage = "Debe ingresar el email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        /// <example>
        /// oriam_22
        /// </example>
        [SwaggerParameter(Description = "nombre de usuario")]

        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        /// <example>
        /// 123Pa$$word!
        /// </example>
        [SwaggerParameter(Description = "contrasenia de usuario")]

        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas deben coincidir")]
        [Required(ErrorMessage = "Debe repetir la contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        [JsonIgnore]
        public string? Role { get; set; }
        [JsonIgnore]
        public bool HasError { get; set; }
        [JsonIgnore]
        public string? Error { get; set; }
    }
}
