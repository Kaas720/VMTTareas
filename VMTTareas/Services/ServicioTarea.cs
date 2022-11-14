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
using Microsoft.Data.SqlClient;

namespace VMTTareas.Services
{
    public class ServicioTarea : Tareas_Interface
    {
            private readonly DataContext _context;

            public ServicioTarea(DataContext context)
            {
                _context = context;
            }

        public async Task<IEnumerable<TareaDto>> getTareaUser(int idUsuario)
        {
            try
            {
                var tarea = await(from p in _context.tarea
                                   join e in _context.tipoTarea on p.idTipoTarea equals e.idTipoTarea
                                   where p.idUsuario == idUsuario && p.estado == "A"
                                   select new TareaDto
                                   {
                                       idTarea = p.idTarea,
                                       decripcionTarea = p.descripcionTarea,
                                       nombreTarea = p.nombreTarea,
                                       codigoreferencia = p.codigoReferencia,
                                       nombreTipotarea = e.descripcionTipoTarea

                                   }).ToListAsync();
                return tarea;
            }
            catch
            {
                throw new Exception("Fallo del servidor");
            }
        }

        public async Task<TareaDtoReturn> postTarea(Tarea tarea)
        {
            try
            {
                TareaDtoReturn tareaDtoReturn = validarCodigoSiExiste(tarea);
                if (tareaDtoReturn.codigo == "404")
                    return tareaDtoReturn;
                else
                {
                    tareaDtoReturn = crearTarea(tarea);
                    return tareaDtoReturn;
                }
                    
            }
            catch
            {
                throw new Exception("Fallo del servidor");
            }
        }
        public async Task<TareaDtoReturn> putTarea(Tarea tarea)
        {
            try
            {
                TareaDtoReturn tareaDtoReturn = editarTarea(tarea);
                return tareaDtoReturn;
            }
            catch
            {
                throw new Exception("Fallo del servidor");
            }
        }
        public TareaDtoReturn validarCodigoSiExiste(Tarea tarea)
        {
            object[] parameters = new object[] {
                    tarea.idTipoTarea,
                    tarea.codigoReferencia,
                    new SqlParameter() { ParameterName = "@codigoRetorno",Direction = System.Data.ParameterDirection.Output, Size = 10 },
                    new SqlParameter() { ParameterName = "@mensajeRetorno",Direction = System.Data.ParameterDirection.Output, Size = 50},
                    new SqlParameter() { ParameterName = "@idTareaRetorno",Direction = System.Data.ParameterDirection.Output, Size = 10}
             };
            List<String> mensajes = _context.DoExecSP("existCodeTarea {0}, {1}, {2} OUT, {3} OUT, {4} OUT", parameters, 2);

            return new TareaDtoReturn { 
                codigo = mensajes[0],
                mensaje = mensajes[1],
                tarea = null
            };
        }
        public async Task<List<TipoTarea>> getTipostareas()
        {
            var ListTipostareas = await _context.tipoTarea.ToListAsync();
            return ListTipostareas;
        }
        public TareaDtoReturn crearTarea(Tarea tarea)
        {
            object[] parameters = new object[] {
                    tarea.idUsuario,
                    tarea.idTipoTarea,
                    tarea.nombreTarea,
                    tarea.descripcionTarea,
                    tarea.codigoReferencia,
                    new SqlParameter() { ParameterName = "@codigoRetorno",Direction = System.Data.ParameterDirection.Output, Size = 10 },
                    new SqlParameter() { ParameterName = "@mensajeRetorno",Direction = System.Data.ParameterDirection.Output, Size = 50},
                    new SqlParameter() { ParameterName = "@idTareaRetorno",Direction = System.Data.ParameterDirection.Output, Size = 10}
             };
            List<String> mensajes = _context.DoExecSP("postTarea {0}, {1}, {2}, {3}, {4}, {5} OUT, {6} OUT, {7} OUT", parameters, 5);
            List<TareaDto> tareaRegisto = new List<TareaDto>();
            tareaRegisto.Add(new TareaDto
            {
                idTarea = Convert.ToInt32( mensajes[2])
            });
            return new TareaDtoReturn
            {
                codigo = mensajes[0],
                mensaje = mensajes[1],
                tarea = tareaRegisto

            };
        }
        public TareaDtoReturn editarTarea(Tarea tarea)
        {
            object[] parameters = new object[] {
                    tarea.idTarea,
                    tarea.nombreTarea,
                    tarea.descripcionTarea,
                    new SqlParameter() { ParameterName = "@codigoRetorno",Direction = System.Data.ParameterDirection.Output, Size = 10 },
                    new SqlParameter() { ParameterName = "@mensajeRetorno",Direction = System.Data.ParameterDirection.Output, Size = 50},
                    new SqlParameter() { ParameterName = "@idTareaRetorno",Direction = System.Data.ParameterDirection.Output, Size = 10}
            };
            List<String> mensajes = _context.DoExecSP("putTarea {0}, {1}, {2},{3} OUT, {4} OUT, {5} OUT", parameters, 3);
            List<TareaDto> tareaRegisto = new List<TareaDto>();
            tareaRegisto.Add(new TareaDto
            {
                idTarea = tarea.idTarea
            });
            return new TareaDtoReturn
            {
                codigo = mensajes[0],
                mensaje = mensajes[1],
                tarea = tareaRegisto

            };
        }

        public async Task<Boolean> existeTarea(int id)
        {
            var tarea = await _context.tarea.FindAsync(id);

            if (tarea == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<TareaDtoReturn> delateTarea(int idTarea)
        {
            var tarea = await _context.tarea.FindAsync(idTarea);
            if (tarea == null)
            {
                return new TareaDtoReturn
                {
                    codigo = "404",
                    mensaje = "Tarea no encontrada",
                    tarea = null

                };
            }
            else
            {
                try
                {
                    tarea.estado = "I";
                    _context.Entry(tarea).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return new TareaDtoReturn
                        {
                            codigo = "200",
                            mensaje = "Tarea eliminada con exito",
                            tarea = null

                        };
                }
                catch(Exception ex)
                {
                    throw new Exception("Fallo del servidor => "+ex.Message);
                }
               
            }
        }
    }
}
