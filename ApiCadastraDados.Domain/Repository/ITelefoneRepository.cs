using ApiCadastraDados.Domain.Model;

namespace ApiCadastraDados.Domain.Repository
{
    public interface ITelefoneRepository
    {
        Task<Telefone> GetTelefoneAsync(int idUser);
        Telefone UpdateTelefone(Telefone telefone);
        void DeleteTelefone(Telefone telefone);
    }
}
