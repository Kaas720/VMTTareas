
using System.ComponentModel.DataAnnotations;

namespace VMTTareas.Models
{
    public class TipoTarea
    {
        [Key]
        public int idTipoTarea { get; set; }
        public string descripcionTipoTarea { get; set; }
        public string estado { get; set; }
    }
}
