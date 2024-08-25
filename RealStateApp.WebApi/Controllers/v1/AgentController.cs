using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Features.Agents.Commands.ChangeAgentStatusById;
using RealStateApp.Core.Application.Features.Agents.Queries.GetAgentById;
using RealStateApp.Core.Application.Features.Agents.Queries.GetAllAgents;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAgentProperties;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealStateApp.Core.Application.Features.Properties.Queries.GetProperty;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de agentes")]
    public class AgentController : BaseApiController
    {
        [Authorize(Roles = "ADMIN, DESARROLLADOR")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary ="Listado de agentes",
            Description ="obtiene todos los agentes"
            )]
        public async Task<IActionResult> List()
        {
            try
            {
                var response = await Mediator.Send(new GetAllAgentsQuery());

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
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Busqueda de agente",
            Description = "obtiene un agente por su id"
            )]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {

                var response = await Mediator.Send(new GetAgentByIdQuery() { Id = id });

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

        [Authorize(Roles = "ADMIN, DESARROLLADOR")]
        [HttpGet("GetAgentProperty/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de propiedades de agente",
            Description = "obtiene todos las propiedades de un agente mediante su id (del agente)"
            )]
        public async Task<IActionResult> GetAgentProperty(string id)
        {
            try
            {

                var response = await Mediator.Send(new GetAgentPropertiesQuery() { Id = id });

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

        [Authorize(Roles = "ADMIN")]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut("ChangeStatus/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Cambiar estado de agente",
            Description = "cambia el estado del agente mediante su id"
            )]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody] ChangeAgentStatusByIdCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    return BadRequest();
                }

                await Mediator.Send(command);

                return NoContent();


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }


        }

    }
}
