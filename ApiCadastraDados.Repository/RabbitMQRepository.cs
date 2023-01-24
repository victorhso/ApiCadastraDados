using ApiCadastraDados.Domain.Dtos;
using ApiCadastraDados.Domain.Model;
using ApiCadastraDados.Domain.Repository;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ApiCadastraDados.Repository
{
    public class RabbitMQRepository : IRabbitMQRepository
    {
        private RabbitMQConfiguration _rabbitMqOptions;
        private IConnection _connection;

        private readonly string _hostname;
        private readonly string _password;
        private readonly string _routeKey;
        private readonly string _userName;
        private readonly string _clientProviderName;
        private readonly string _exchangeName;
        private readonly int _port;

        public RabbitMQRepository(IOptions<RabbitMQConfiguration> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.HOST_MQ;
            _password = rabbitMqOptions.Value.PWD_MQ;
            _routeKey = rabbitMqOptions.Value.CADASTRAR_DADOS;
            _userName = rabbitMqOptions.Value.USER_NAME_MQ;
            _clientProviderName = rabbitMqOptions.Value.CLIENT_PROVIDER_MQ;
            _exchangeName = rabbitMqOptions.Value.EXCHANGE_NAME_MQ;
            _port = int.Parse(rabbitMqOptions.Value.PORT_MQ);

            CreateConnection();
        }

        public void SendToQueue(DadosCadastro dadosFila)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    string exchangeName = _exchangeName;
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true, autoDelete: false);

                    var routingKey = _routeKey ?? _exchangeName;
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dadosFila));

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: exchangeName,
                        routingKey: routingKey,
                        basicProperties: properties,
                        body: body);
                }
            }
        }

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }

        private void CreateConnection()
        {
            try
            {
                ConnectionFactory factoryMQ = new ConnectionFactory()
                {
                    HostName = _hostname,
                    UserName = _userName,
                    Password = _password,
                    Port = _port,
                    ClientProvidedName = _clientProviderName
                };
                _connection = factoryMQ.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}