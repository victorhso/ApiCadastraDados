using System.Text.Json.Serialization;

namespace ApiCadastraDados.Domain.Dtos
{
    public class TelefoneDto
    {
        public string? Ddi { get; set; }
        public string? Ddd { get; set; }
        public string? NumeroCelular { get; set; }
        public string? NumeroTelefone { get; set; }
        [JsonIgnore]
        public int IdPessoa { get; set; }
    }
}
