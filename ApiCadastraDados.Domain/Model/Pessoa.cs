namespace ApiCadastraDados.Domain.Model
{
    public class Pessoa : EntityBase
    {
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public string? Sexo { get; set; }
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
