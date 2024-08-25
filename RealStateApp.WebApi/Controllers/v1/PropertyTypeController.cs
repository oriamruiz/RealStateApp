using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealStateApp.Core.Application.Features.Properties.Queries.GetProperty;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypesQuery;
using RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeByIdQuery;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de tipos de propiedades")]
    public class PropertyTypeController : BaseApiController
    {

        [Authorize(Roles = "ADMIN, DESARROLLADOR")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de tipos de propiedades",
            Description = "obtiene todos los tipos de propiedades"
            )]
        public async Task<IActionResult> List()
        {
            try
            {
                var response = await Mediator.Send(new GetAllPropertyTypesQuery());

                if (response == null || response.Count == 0)
                {
                    return NoContent();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }


        }

        [Authorize(Roles = "ADMIN, DESARROLLADOR")]
        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Busqueda de tipo de propiedade",
            Description = "obtiene un tipo de propiedad por su id"
            )]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await Mediator.Send(new GetPropertyTypeByIdQuery() { Id = id });
                if (response == null)
                {
                    return NoContent();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }


        }

        [Authorize(Roles = "ADMIN")]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Creacion de tipo de propiedad",
            Description = "Recibe los parametros necesarios para crear un tipo de propiedad"
            )]
        public async Task<IActionResult> Create([FromBody] CreatePropertyTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await Mediator.Send(command);


                return StatusCode(StatusCodes.Status201Created);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }


        }


        [Authorize(Roles = "ADMIN")]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizacion de tipo de propiedad",
            Description = "Recibe los parametros necesarios para actualizar un tipo de propiedad"
            )]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if(id != command.Id)
                {
                    return BadRequest();
                }

                var response = await Mediator.Send(command);

                return Ok(response);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }


        }


        [Authorize(Roles = "ADMIN")]
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Eliminacion de tipo de propiedad",
            Description = "Elimina el tipo de propiedad su id, al eliminarlo se elimina todas las propiedades asociadas con este"
            )]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var propertiesid = await Mediator.Send(new DeletePropertyTypeByIdCommand{ Id = id});

                if (propertiesid != null && propertiesid.Count > 0)
                {
                   
                    DeleteImagesDirectory(propertiesid);
                    
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }


        }

        #region private methods


        private void DeleteImagesDirectory(IList<int> ids)
        {

            foreach (var id in ids)
            {
                
                string path = $"C:/Users/oriam/Desktop/programacion3/RealStateApp/RealStateWebApp/wwwroot/images/Properties/{id}";

                if (Directory.Exists(path))
                {
                    DirectoryInfo directoryinfo = new DirectoryInfo(path);

                    foreach (FileInfo file in directoryinfo.GetFiles())
                    {
                        file.Delete();
                    }

                    foreach (DirectoryInfo folder in directoryinfo.GetDirectories())
                    {
                        folder.Delete(true);
                    }

                    Directory.Delete(path);
                }
            }
            

        }
        #endregion
    }
}
