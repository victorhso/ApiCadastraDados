using System.Text.Json.Serialization;

namespace ApiCadastraDados.Domain.Dtos
{
    public class PessoaDto
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public string? Sexo { get; set; }
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
