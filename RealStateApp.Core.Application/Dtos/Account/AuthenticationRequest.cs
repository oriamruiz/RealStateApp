using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parametros para autenticarse
    /// </summary>
    public class AuthenticationRequest
    {
        /// <example>
        /// oriam_22
        /// </example>
        [SwaggerParameter(Description = "nombre de usuario")]
        [Required(ErrorMessage = "Debe colocar el nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        

        [SwaggerParameter(Description = "contrasenia")]
        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [DataType(DataType.Password)]
        public  string Password { get; set; }


    }
}
