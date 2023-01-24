namespace ApiCadastraDados.Domain.Dtos
{
    public class DadosCadastroDtoPost
    {
        public PessoaDto? Pessoa { get; set; }
        public EnderecoDtoPost? Endereco { get; set; }
        public TelefoneDto? Telefone { get; set; }
    }
}
