using ApiCadastraDados.Domain.Model;

namespace ApiCadastraDados.Domain.Repository
{
    public interface IEnderecoRepository
    {
        Task<Endereco> GetEnderecoAsync(int IdUsuario);
        Endereco UpdateEndereco(Endereco endereco);
        void DeleteEndereco(Endereco endereco);
    }
}
