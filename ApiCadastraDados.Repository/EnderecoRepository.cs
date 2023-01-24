using ApiCadastraDados.Domain.Dtos;
using ApiCadastraDados.Domain.Model;
using ApiCadastraDados.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ApiCadastraDados.Repository
{
    public class EnderecoRepository : IEnderecoRepository
    {
        protected readonly DbContextOptions<Context> _DbOptions;
        protected readonly ConnStrings _connStrings;

        public EnderecoRepository(ConnStrings connStrings)
        {
            _connStrings = connStrings;
            _DbOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(_connStrings.SqlConnectionString).Options;
        }

        public async Task<Endereco> GetEnderecoAsync(int IdUsuario)
        {
            using (var Db = new Context(_DbOptions))
            {
                return await Db.Enderecos.FirstOrDefaultAsync(p => p.IdPessoa == IdUsuario);
            }
        }

        public Endereco UpdateEndereco(Endereco endereco)
        {
            using (var Db = new Context(_DbOptions))
            {
                Db.Enderecos.Update(endereco);
                Db.SaveChanges();
            }
            return endereco;
        }

        public void DeleteEndereco(Endereco endereco)
        {
            if (endereco != null)
            {
                using (var Db = new Context(_DbOptions))
                {
                    Db.Enderecos.Remove(endereco);
                    Db.SaveChanges();
                }
            }
        }
    }
}
