using System.Text.Json.Serialization;

namespace ApiCadastraDados.Domain.Dtos
{
    public class EnderecoDto
    {
        public string? Rua { get; set; }
        public int Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }
        public string? Pais { get; set; }
        public string? Cep { get; set; }
        [JsonIgnore]
        public int IdPessoa { get; set; }
    }
}
