using ApiCadastraDados.Domain.Dtos;
using ApiCadastraDados.Domain.Model;
using ApiCadastraDados.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ApiCadastraDados.Repository
{
    public class TelefoneRepository : ITelefoneRepository
    {
        protected readonly DbContextOptions<Context> _DbOptions;
        protected readonly ConnStrings _connStrings;

        public TelefoneRepository(ConnStrings connStrings)
        {
            _connStrings = connStrings;
            _DbOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(_connStrings.SqlConnectionString).Options;
        }

        public async Task<Telefone> GetTelefoneAsync(int idUser)
        {
            using (var Db = new Context(_DbOptions))
            {
                return await Db.Telefones.FirstOrDefaultAsync(p => p.IdPessoa == idUser);
            }
        }

        public Telefone UpdateTelefone(Telefone telefone)
        {
            using (var Db = new Context(_DbOptions))
            {
                Db.Telefones.Update(telefone);
                Db.SaveChanges();
            }
            return telefone;
        }

        public void DeleteTelefone(Telefone telefone)
        {
            if (telefone != null)
            {
                using (var Db = new Context(_DbOptions))
                {
                    Db.Telefones.Remove(telefone);
                    Db.SaveChanges();
                }
            }
        }
    }
}
