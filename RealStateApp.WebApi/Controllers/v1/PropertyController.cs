using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealStateApp.Core.Application.Features.Properties.Queries.GetProperty;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;


namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "ADMIN, DESARROLLADOR")]
    [SwaggerTag("Mantenimiento de propiedades")]
    public class PropertyController : BaseApiController
    {

        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de propiedades",
            Description = "obtiene todas las propiedades"
            )]
        public async Task<IActionResult> List()
        {
            try
            {
                var response = await Mediator.Send(new GetAllPropertiesQuery());
                
                if(response == null || response.Count == 0)
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

        
        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Busqueda de propiedad",
            Description = "obtiene una propiedad por su id"
            )]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {


                var response = await Mediator.Send(new GetPropertyQuery() { Id = id });
                
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


        [HttpGet("GetByCode")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Busqueda de propiedad",
            Description = "obtiene una propiedad por su codigo"
            )]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                var response = await Mediator.Send(new GetPropertyQuery() { Code = code });
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
    }
}
