using ApiCadastraDados.Domain.Dtos;

namespace ApiCadastraDados.Domain.Services
{
    public interface IHttpFactoryService
    {
        Task<ResponseBuscaEnderecoPorCepDto> BuscarEnderecoPorCep(string cep);
    }
}
