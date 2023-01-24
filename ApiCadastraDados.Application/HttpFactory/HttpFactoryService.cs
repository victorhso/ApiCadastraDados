using ApiCadastraDados.Domain.Dtos;
using ApiCadastraDados.Domain.Services;
using Newtonsoft.Json;

namespace ApiCadastraDados.Application.HttpFactory
{
    public class HttpFactoryService : IHttpFactoryService
    {
        private static readonly HttpClient client = new HttpClient();
        private static string BaseUri = "https://viacep.com.br/ws";

        public async Task<ResponseBuscaEnderecoPorCepDto> BuscarEnderecoPorCep(string cep)
        {
            string result = String.Empty;
            string url = string.Format("{0}/{1}", BaseUri, $"{cep}/json");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();

                var buscaEnderecoPorCepDto = JsonConvert.DeserializeObject<ResponseBuscaEnderecoPorCepDto>(result);
                return buscaEnderecoPorCepDto;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException($"Erro ao realizar a consulta na URL {url} - #Exception: {ex.Message}");
            }
        }
    }
}
