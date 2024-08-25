using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.SaleType;
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
    [SwaggerTag("Mantenimiento de tipos de ventas")]

    public class SaleTypeController : BaseApiController
    {
        [Authorize(Roles = "ADMIN, DESARROLLADOR")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de tipos de ventas",
            Description = "obtiene todos los tipos de ventas"
            )]
        public async Task<IActionResult> List()
        {
            try
            {
                var response = await Mediator.Send(new GetAllSaleTypesQuery());

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Busqueda de tipo de ventas",
            Description = "obtiene un tipo de venta por su id"
            )]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await Mediator.Send(new GetSaleTypeByIdQuery() { Id = id });
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
            Summary = "Creacion de tipo de venta",
            Description = "Recibe los parametros necesarios para crear un tipo de venta"
            )]
        public async Task<IActionResult> Create([FromBody] CreateSaleTypeCommand command)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizacion de tipo de venta",
            Description = "Recibe los parametros necesarios para actualizar un tipo de venta"
            )]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSaleTypeCommand command)
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
            Summary = "Eliminacion de tipo de venta",
            Description = "Elimina el tipo de venta su id, al eliminarlo se elimina todas las propiedades asociadas con este"
            )]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var propertiesremovedid = await Mediator.Send(new DeleteSaleTypeByIdCommand { Id = id });

                if (propertiesremovedid != null && propertiesremovedid.Count > 0)
                {

                    DeleteImagesDirectory(propertiesremovedid);

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
