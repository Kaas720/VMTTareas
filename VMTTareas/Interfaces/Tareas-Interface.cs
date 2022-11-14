namespace VMTTareas.Interfaces;
    using VMTTareas.DTOS;
using VMTTareas.Models;

public interface Tareas_Interface
    {
        public Task<IEnumerable<TareaDto>> getTareaUser(int idUsuario);
        public Task<TareaDtoReturn> postTarea(Tarea tarea);
        public Task<List<TipoTarea>> getTipostareas();
        public Task<TareaDtoReturn> putTarea(Tarea tarea);
        public Task<Boolean> existeTarea(int id);
        public Task<TareaDtoReturn> delateTarea(int idTarea);
}

