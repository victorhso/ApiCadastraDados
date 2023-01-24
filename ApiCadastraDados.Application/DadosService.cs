using ApiCadastraDados.Domain.Dtos;
using ApiCadastraDados.Domain.Enums;
using ApiCadastraDados.Domain.Model;
using ApiCadastraDados.Domain.Repository;
using ApiCadastraDados.Domain.Services;
using AutoMapper;
using Newtonsoft.Json;

namespace ApiCadastraDados.Application
{
    public class DadosService : IDadosService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ITelefoneRepository _telefoneRepository;
        private readonly IRegistroRepository _registroRepository;
        private readonly IRabbitMQRepository _rabbitMQRepository;
        private readonly IHttpFactoryService _httpFactoryService;
        private readonly IMapper _mapper;

        public DadosService(IEnderecoRepository enderecoRepository, IPessoaRepository pessoaRepository, ITelefoneRepository telefoneRepository, IRegistroRepository registroRepository, IMapper mapper, IRabbitMQRepository rabbitMQRepository, IHttpFactoryService httpFactoryService)
        {
            _enderecoRepository = enderecoRepository;
            _pessoaRepository = pessoaRepository;
            _telefoneRepository = telefoneRepository;
            _registroRepository = registroRepository;
            _rabbitMQRepository = rabbitMQRepository;
            _httpFactoryService = httpFactoryService;
            _mapper = mapper;
        }

        public async Task<Dictionary<int, PessoaDto>> GetLstUsers()
        {
            try
            {
                Dictionary<int, PessoaDto>? result = null;

                var lstResult = await _pessoaRepository.GetAllPessoa();
                result = _mapper.Map<Dictionary<int, Pessoa>, Dictionary<int, PessoaDto>>(lstResult);

                if (result is null)
                    _registroRepository.RegistrarErro((int)CodigoErro.Falha, "Nenhum registro encontrado", "GetLstUsers()");

                return result;
            }
            catch (Exception ex)
            {
                _registroRepository.RegistrarErro((int)CodigoErro.Falha, "Ocorreu um erro no método.", "GetLstUsers()", String.Empty, ex);
                throw;
            }
        }

        public async Task<DadosCadastroDto> GetUser(int idUser)
        {
            try
            {
                var pessoa = await _pessoaRepository.GetPessoaAsync(idUser);
                var endereco = await _enderecoRepository.GetEnderecoAsync(idUser);
                var telefone = await _telefoneRepository.GetTelefoneAsync(idUser);

                DadosCadastroDto dadosCadastroDto = new DadosCadastroDto()
                {
                    Pessoa = _mapper.Map<Pessoa, PessoaDto>(pessoa),
                    Endereco = _mapper.Map<Endereco, EnderecoDto>(endereco),
                    Telefone = _mapper.Map<Telefone, TelefoneDto>(telefone)
                };

                return dadosCadastroDto;
            }
            catch (Exception ex)
            {
                _registroRepository.RegistrarErro((int)CodigoErro.Falha, "Ocorreu um erro no método.", "GetUser()", String.Empty, ex);
                throw;
            }
        }

        public DadosCadastroDtoPost InsertUser(DadosCadastroDtoPost dadosCadastroDto)
        {
            try
            {
                _rabbitMQRepository.SendToQueue(_mapper.Map<DadosCadastroDtoPost, DadosCadastro>(dadosCadastroDto));

                _registroRepository.RegistrarErro((int)CodigoErro.Sucesso, "O registro foi mandado para o WorkerService CadastraDados com sucesso", "InsertUser()");

                return dadosCadastroDto;
            }
            catch (Exception ex)
            {
                _registroRepository.RegistrarErro((int)CodigoErro.Falha, "Ocorreu um erro no método.", "InsertUser()", String.Empty, ex);
                throw;
            }
        }

        public async Task<DadosCadastroDtoPost> UpdateUser(DadosCadastroDtoPost dadosCadastroDto, int idUser)
        {
            try
            {
                var pessoa = await _pessoaRepository.GetPessoaAsync(idUser);
                var endereco = await _enderecoRepository.GetEnderecoAsync(idUser);
                var telefone = await _telefoneRepository.GetTelefoneAsync(idUser);

                var searchAddress = await _httpFactoryService.BuscarEnderecoPorCep(dadosCadastroDto.Endereco.Cep);

                if (pessoa != null && endereco != null && telefone != null)
                {
                    var pessoaAtt = _mapper.Map<PessoaDto, Pessoa>(dadosCadastroDto.Pessoa);
                    var enderecoAtt = _mapper.Map<EnderecoDtoPost, Endereco>(dadosCadastroDto.Endereco);
                    var telefoneAtt = _mapper.Map<TelefoneDto, Telefone>(dadosCadastroDto.Telefone);

                    enderecoAtt = PreencheEndereco(dadosCadastroDto.Endereco, searchAddress, idUser, dadosCadastroDto.Endereco.Numero);

                    pessoaAtt.ID = pessoa.ID;
                    enderecoAtt.ID = endereco.ID;
                    telefoneAtt.ID = telefone.ID;

                    _pessoaRepository.UpdatePessoa(pessoaAtt);
                    _enderecoRepository.UpdateEndereco(enderecoAtt);
                    _telefoneRepository.UpdateTelefone(telefoneAtt);

                    return dadosCadastroDto;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                _registroRepository.RegistrarErro((int)CodigoErro.Falha, "Ocorreu um erro no método.", "UpdateUser()", String.Empty, ex);
                throw;
            }
        }

        public async void DeleteUser(int idUser)
        {
            try
            {
                var pessoa = await _pessoaRepository.GetPessoaAsync(idUser);
                var telefone = await _telefoneRepository.GetTelefoneAsync(idUser);
                var endereco = await _enderecoRepository.GetEnderecoAsync(idUser);

                _telefoneRepository.DeleteTelefone(telefone);
                _enderecoRepository.DeleteEndereco(endereco);
                _pessoaRepository.DeletePessoa(pessoa);

                _registroRepository.RegistrarErro((int)CodigoErro.Sucesso, "Registros excluídos com sucesso!", "DeleteUser()", $"#Pessoa: {JsonConvert.SerializeObject(pessoa)} #Endereco: {JsonConvert.SerializeObject(endereco)} $Telefone: {JsonConvert.SerializeObject(telefone)}");
            }
            catch (Exception ex)
            {
                _registroRepository.RegistrarErro((int)CodigoErro.Falha, "Ocorreu um erro no método.", "DeleteUser()", String.Empty, ex);
                throw;
            }
        }

        private Endereco PreencheEndereco(EnderecoDtoPost endereco, ResponseBuscaEnderecoPorCepDto searchAddress, int idUser, int numero)
        {
            endereco.Rua = searchAddress.logradouro;
            endereco.Numero = numero;
            endereco.Bairro = searchAddress.bairro;
            endereco.Cidade = searchAddress.localidade;
            endereco.UF = searchAddress.uf;
            endereco.Pais = "Brasil";
            endereco.IdPessoa = idUser;

            return _mapper.Map<EnderecoDtoPost, Endereco>(endereco);
        }
    }
}
