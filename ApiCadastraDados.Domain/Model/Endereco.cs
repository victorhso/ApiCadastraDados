namespace ApiCadastraDados.Domain.Model
{
    public class Endereco : EntityBase
    {
        public string? Rua { get; set; }
        public int Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }
        public string? Pais { get; set; }
        public string? Cep { get; set; }
        public int IdPessoa { get; set; }
    }
}
