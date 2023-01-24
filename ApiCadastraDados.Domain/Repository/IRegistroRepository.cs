namespace ApiCadastraDados.Domain.Repository
{
    public interface IRegistroRepository
    {
        void RegistrarErro(int codErr, string msgErr, string metodo, string? entrada = null, Exception? ex = null);
    }
}
