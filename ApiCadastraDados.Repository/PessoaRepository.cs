using ApiCadastraDados.Domain.Dtos;
using ApiCadastraDados.Domain.Model;
using ApiCadastraDados.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ApiCadastraDados.Repository
{
    public class PessoaRepository : IPessoaRepository
    {
        protected readonly DbContextOptions<Context> _DbOptions;
        protected readonly ConnStrings _connStrings;

        public PessoaRepository(ConnStrings connStrings)
        {
            _connStrings = connStrings;
            _DbOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(_connStrings.SqlConnectionString).Options;
        }

        public async Task<Pessoa> GetPessoaAsync(int idPessoa)
        {
            using (var Db = new Context(_DbOptions))
            {
                return await Db.Pessoas.FirstOrDefaultAsync(p => p.ID == idPessoa);
            }
        }

        public async Task<Dictionary<int, Pessoa>> GetAllPessoa()
        {
            using (var Db = new Context(_DbOptions))
            {
                return await Db.Pessoas.ToDictionaryAsync(p => p.ID);
            }
        }

        public Pessoa UpdatePessoa(Pessoa pessoa)
        {
            using (var Db = new Context(_DbOptions))
            {
                Db.Pessoas.Update(pessoa);
                Db.SaveChanges();
            }
            return pessoa;
        }

        public void DeletePessoa(Pessoa pessoa)
        {
            if (pessoa != null)
            {
                using (var Db = new Context(_DbOptions))
                {
                    Db.Pessoas.Remove(pessoa);
                    Db.SaveChanges();
                }
            }
        }
    }
}
