using ApiCadastraDados.Domain.Model;

namespace ApiCadastraDados.Domain.Repository
{
    public interface IPessoaRepository
    {
        Task<Dictionary<int, Pessoa>> GetAllPessoa();
        Task<Pessoa> GetPessoaAsync(int idPessoa);
        Pessoa UpdatePessoa(Pessoa pessoa);
        void DeletePessoa(Pessoa pessoa);
    }
}