using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VMTTareas.Data;
using VMTTareas.Models;
using VMTTareas.DTOS;
using VMTTareas.Interfaces;

namespace VMTTareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly Tareas_Interface _tarea;

        public TareasController(Tareas_Interface tarea)
        {
            _tarea = tarea;
        }
        [HttpGet("/buscartareas/{idUsuario}")]
        public async Task<IActionResult> getTareas(int idUsuario)
        {
            try
            {
                List<TareaDto> tareasUser = (List<TareaDto>)await _tarea.getTareaUser(idUsuario);
                if (tareasUser.Count != 0)
                    return Ok(
                        new TareaDtoReturn()
                        {
                            codigo = "200",
                            mensaje = "La consulta fue exitosa",
                            tarea = tareasUser
                        }) ;
                else
                    return NotFound(new TareaDtoReturn()
                    {
                        codigo = "404",
                        mensaje = "El usuario no posee ninguna tarea",
                        tarea = null
                    });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new TareaDtoReturn()
                {
                    codigo = "500",
                    mensaje = ex.Message,
                    tarea = null
                });
            }
        }
        [HttpGet("/buscarTiposTareas")]
        public async Task<IActionResult> getTipos_tareas()
        {
            try
            {
                List<TipoTarea> tareasUser = (List<TipoTarea>)await _tarea.getTipostareas();
                if (tareasUser.Count != 0)
                    return Ok(tareasUser);
                else
                    return NotFound("No existe Tipos de tareas");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new TareaDtoReturn()
                {
                    codigo = "500",
                    mensaje = ex.Message,
                    tarea = null
                });
            }
        }
        [HttpPost("/creartarea")]
        public async Task<IActionResult> postTarea(Tarea tarea)
        {
            TareaDtoReturn tarea_post = await _tarea.postTarea(tarea);
            if (tarea_post.codigo == "404")
                return NotFound(tarea_post);
            else
            {
                if (tarea_post.codigo == "500")
                    return StatusCode(500, tarea_post);
                else
                    return Ok(tarea_post);
            }        
        }
        [HttpPut("/EditarTarea")]
        public async Task<IActionResult> putTarea(Tarea tarea)
        {
            if (! await _tarea.existeTarea(tarea.idTarea))
            {
                return NotFound(
                    new TareaDtoReturn()
                    {
                        codigo = "404",
                        mensaje = "Tarea no encontrada",
                        tarea = null
                    });
            }
            else
            {
                TareaDtoReturn tarea_put = await _tarea.putTarea(tarea);
                if (tarea_put.codigo == "500")
                    return StatusCode(500, tarea_put.tarea[0]);
                else
                    return Ok(tarea_put);
            }
        }
        [HttpPost ("/enviarDatos")]
        public async Task<IActionResult> enviarDatos(Tarea tarea)
        {
            if (tarea.idTarea == 0)
                return await postTarea(tarea);
            else
                return await putTarea(tarea);
        }
        [HttpDelete("/Eliminar/{id}")]
        public async Task<IActionResult> eliminarTarea(int id)
        {
            try
            {
                TareaDtoReturn tarea = await _tarea.delateTarea(id);
                if (tarea.codigo == "404")
                    return NotFound(tarea);
                else

                    return Ok(tarea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
