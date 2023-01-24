using System.Text.Json.Serialization;

namespace ApiCadastraDados.Domain.Dtos
{
    public class EnderecoDtoPost
    {
        [JsonIgnore]
        public string Rua { get; set; } = String.Empty;
        public int Numero { get; set; }
        [JsonIgnore]
        public string Bairro { get; set; } = String.Empty;
        [JsonIgnore]
        public string Cidade { get; set; } = String.Empty;
        [JsonIgnore]
        public string UF { get; set; } = String.Empty;
        [JsonIgnore]
        public string Pais { get; set; } = String.Empty;
        public string? Cep { get; set; }
        [JsonIgnore]
        public int IdPessoa { get; set; }
    }
}
