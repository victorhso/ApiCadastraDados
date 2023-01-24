using ApiCadastraDados.Domain.Dtos;

namespace ApiCadastraDados.Domain.Services
{
    public interface IDadosService
    {
        Task<Dictionary<int, PessoaDto>> GetLstUsers();
        Task<DadosCadastroDto> GetUser(int idUser);
        DadosCadastroDtoPost InsertUser(DadosCadastroDtoPost dadosCadastroDto);
        Task<DadosCadastroDtoPost> UpdateUser(DadosCadastroDtoPost dadosCadastroDto, int idUser);
        public void DeleteUser(int idUser);
    }
}
