using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetProperty
{
    /// <example>
    /// parametros para filtrar propiedades
    /// </example>
    public class GetPropertyParameter
    {
        [SwaggerParameter(Description = "colocar el Id de la propiedad ")]
        public int? Id { get; set; }
        [SwaggerParameter(Description = "colocar el codigo de la propiedad")]
        public string? Code { get; set; }
    }
}
