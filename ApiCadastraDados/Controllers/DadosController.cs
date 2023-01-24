using ApiCadastraDados.Domain.Dtos;
using ApiCadastraDados.Domain.Enums;
using ApiCadastraDados.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiCadastraDados.Controllers
{
    [ApiController]
    [Route("api/dataUser")]
    public class DadosController : ControllerBase
    {
        private readonly IDadosService _dadosService;
        public DadosController(IDadosService dadosService)
        {
            _dadosService = dadosService;
        }
        /// <summary>
        /// Busca todos os usuários cadastrados na base.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _dadosService.GetLstUsers();

                if (result != null && result.Count > 0)
                    return Ok(result);
                else
                {
                    RetornoApi retornoAPI = new RetornoApi()
                    {
                        CODIGO = (int)CodigoErro.Falha,
                        RETORNO = "NÃO FORAM ENCONTRADOS REGISTROS"
                    };
                    return NotFound(retornoAPI);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Busca o registro de um único usuário.
        /// </summary>
        [HttpGet("{idUser}")]
        public async Task<IActionResult> GetUser(int idUser)
        {
            try
            {
                var result = await _dadosService.GetUser(idUser);

                if (result != null)
                    return Ok(result);
                else
                {
                    RetornoApi retornoAPI = new RetornoApi()
                    {
                        CODIGO = (int)CodigoErro.Falha,
                        RETORNO = $"NÃO FORAM ENCONTRADOS REGISTROS PARA O USUÁRIO {idUser}"
                    };
                    return NotFound(retornoAPI);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cria um novo registro de usuário.
        /// </summary>
        [HttpPost]
        public IActionResult InsertRegister([FromBody] DadosCadastroDtoPost dadosCadastroDtoPost)
        {
            try
            {
                var result = _dadosService.InsertUser(dadosCadastroDtoPost);

                if (result != null)
                    return Ok(result);
                else
                {
                    RetornoApi retornoAPI = new RetornoApi()
                    {
                        CODIGO = (int)CodigoErro.Falha,
                        RETORNO = $"NÃO FOI POSSÍVEL REALIZAR O CADASTRO DO USUÁRIO. ENTRE EM CONTATO COM O ADMINISTRADOR DO SISTEMA."
                    };
                    return BadRequest(retornoAPI);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza o registro de um usuário.
        /// </summary>
        [HttpPut("{idUser}")]
        public async Task<IActionResult> UpdateRegister(int idUser, [FromBody] DadosCadastroDtoPost dadosCadastroDto)
        {
            try
            {
                var result = await _dadosService.UpdateUser(dadosCadastroDto, idUser);

                if (result != null)
                    return Ok(result);
                else
                {
                    RetornoApi retornoAPI = new RetornoApi()
                    {
                        CODIGO = (int)CodigoErro.Falha,
                        RETORNO = $"NÃO FOI POSSÍVEL REALIZAR A ATUALIZAÇÃO DO USUÁRIO. ENTRE EM CONTATO COM O ADMINISTRADOR DO SISTEMA."
                    };
                    return BadRequest(retornoAPI);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta o registro de usuário.
        /// </summary>
        [HttpDelete("{idUser}")]
        public IActionResult DeleteRegister(int idUser)
        {
            try
            {
                _dadosService.DeleteUser(idUser);

                RetornoApi retornoAPI = new RetornoApi()
                {
                    CODIGO = (int)CodigoErro.Sucesso,
                    RETORNO = $"USUÁRIO EXCLUÍDO COM SUCESSO!"
                };
                return Ok(retornoAPI);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
