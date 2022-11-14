using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VMTTareas.Models;

namespace VMTTareas.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Usuario> usuario { get; set; }
        public DbSet<Tarea> tarea { get; set; }
        public DbSet<TipoTarea> tipoTarea { get; set; }
        public List<String> DoExecSP(string nombre_sp, object[] parametros, int indexParametroOut)
        {
            List<String> informacion = new List<String>();
            this.Database.ExecuteSqlRaw(nombre_sp, parametros.ToArray());
            SqlParameter codigoRetorno = (SqlParameter)parametros[indexParametroOut]; 
            SqlParameter mensajeRetorno = (SqlParameter)parametros[indexParametroOut+1];
            SqlParameter idRetorno = (SqlParameter)parametros[indexParametroOut + 2];
            informacion.Add(codigoRetorno.Value.ToString());
            informacion.Add(mensajeRetorno.Value.ToString());
            informacion.Add(idRetorno.Value.ToString());
            return informacion;
        }
    }
}
