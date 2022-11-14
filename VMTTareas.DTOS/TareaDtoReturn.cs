using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTTareas.DTOS
{
    public class TareaDtoReturn
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public List<TareaDto> tarea { get; set; }
    }
}
