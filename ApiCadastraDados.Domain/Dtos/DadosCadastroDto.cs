namespace ApiCadastraDados.Domain.Dtos
{
    public class DadosCadastroDto
    {
        public PessoaDto? Pessoa { get; set; }
        public EnderecoDto? Endereco { get; set; }
        public TelefoneDto? Telefone { get; set; }
    }
}
