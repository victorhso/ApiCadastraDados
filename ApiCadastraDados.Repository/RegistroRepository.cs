using ApiCadastraDados.Domain.Dtos;
using ApiCadastraDados.Domain.Model;
using ApiCadastraDados.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ApiCadastraDados.Repository
{
    public class RegistroLogErroRepository : IRegistroRepository
    {
        protected readonly DbContextOptions<Context> _DbOptions;
        protected readonly ConnStrings _connStrings;

        public RegistroLogErroRepository(Context context, ConnStrings connStrings)
        {
            _connStrings = connStrings;
            _DbOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(_connStrings.SqlConnectionString).Options;
        }

        public void RegistrarErro(int codErr, string msgErr, string metodo, string? entrada = null, Exception? ex = null)
        {
            RegistroErro regristoErro = new RegistroErro()
            {
                CodErr = codErr,
                MsgErr = msgErr,
                Metodo = metodo,
                Entrada = entrada,
                DataHorAtualizacao = DateTime.Now,
                Exception = ex != null ? $"##{ex.Message} ## {ex.StackTrace}" : String.Empty
            };

            using (var Db = new Context(_DbOptions))
            {
                Db.Add(regristoErro);
                Db.SaveChanges();
            }
        }
    }
}
