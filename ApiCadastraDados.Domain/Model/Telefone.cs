namespace ApiCadastraDados.Domain.Model
{
    public class Telefone : EntityBase
    {
        public string? Ddi { get; set; }
        public string? Ddd { get; set; }
        public string? NumeroCelular { get; set; }
        public string? NumeroTelefone { get; set; }
        public int IdPessoa { get; set; }
    }
}
