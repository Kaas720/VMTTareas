using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VMTTareas.Models
{
    public class Tarea
    {
        [Key]
        public int idTarea { get; set; }
        [ForeignKey("Usuario")]
        public int idUsuario { get; set; }
        [ForeignKey("TipoTarea")]
        public int idTipoTarea { get; set; }
        public string nombreTarea { get; set; }
        public string descripcionTarea { get; set; }
        public string codigoReferencia { get; set; }
        public string estado { get; set; }
    }
}
