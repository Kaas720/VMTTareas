using System.ComponentModel.DataAnnotations;

namespace VMTTareas.Models
{
    public class Usuario
    {
        [Key]
        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public string passwordUser { get; set; }
        public string estado { get; set; }

    }
}
