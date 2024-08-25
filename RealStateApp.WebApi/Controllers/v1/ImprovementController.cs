using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealStateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementById;
using RealStateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealStateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements;
using RealStateApp.Core.Application.Features.Improvements.Queries.GetImprovementById;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.CreateSaleType;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.DeleteSaleTypeById;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType;
using RealStateApp.Core.Application.Features.SaleTypes.Queries.GetAllSaleTypes;
using RealStateApp.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de mejoras")]
    public class ImprovementController : BaseApiController
    {
        [Authorize(Roles = "ADMIN, DESARROLLADOR")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de mejoras",
            Description = "obtiene todas las mejoras"
            )]
        public async Task<IActionResult> List()
        {
            try
            {
                var response = await Mediator.Send(new GetAllImprovementsQuery());

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Busqueda de mejora",
            Description = "obtiene una mejora por su id"
            )]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await Mediator.Send(new GetImprovementByIdQuery() { Id = id });
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
            Summary = "Creacion de mejora",
            Description = "Recibe los parametros necesarios para crear una mejora"
            )]
        public async Task<IActionResult> Create([FromBody] CreateImprovementCommand command)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizacion de mejora",
            Description = "Recibe los parametros necesarios para actualizar una mejora"
            )]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateImprovementCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (id != command.Id)
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
            Summary = "Eliminacion de mejora",
            Description = "Elimina la mejora mediante su id"
            )]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteImprovementByIdCommand { Id = id });

                
                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }


        }

        
    }
}
