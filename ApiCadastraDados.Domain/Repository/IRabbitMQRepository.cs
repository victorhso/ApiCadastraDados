using ApiCadastraDados.Domain.Model;

namespace ApiCadastraDados.Domain.Repository
{
    public interface IRabbitMQRepository
    {
        void SendToQueue(DadosCadastro dadosFila);
        void CloseConnection();
    }
}
