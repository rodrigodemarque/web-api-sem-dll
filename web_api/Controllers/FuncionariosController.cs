using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using web_api.Utils.Log;


namespace web_api.Controllers
{
    public class FuncionariosController : ApiController
    {
        readonly Repositories.Funcionario repository;
        readonly Logger logger;
        public FuncionariosController()
        {
            logger = new Logger(Configurations.Config.GetLogPath());
            //connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["web_api"].ConnectionString;
            //connectionString = Configurations.Config.GetConnectionString();
            repository = new Repositories.Funcionario(Configurations.Config.GetConnectionStringSQLServer());

            //logPath = System.Configuration.ConfigurationManager.AppSettings["logPath"];
            
        }

        // GET: api/Funcionarios
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await repository.GetAll());
            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();
            }
        }

        // GET: api/Funcionarios/5
        [Route("api/Funcionarios/{codigo}")]
        public async Task<IHttpActionResult> Get(int codigo)
        {
            try
            {
                Models.Funcionario funcionario = await repository.GetById(codigo);
                
                if (funcionario.Codigo == 0)
                    return NotFound();

                return Ok(funcionario);

            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();
            }

        }

        [Route("api/Funcionarios/{nome:alpha}")]
        public async Task<IHttpActionResult> Get(string nome)
        {
            if (nome.Length < 3)
                return BadRequest("Informe o mínimo de 3 caracteres no nome do carro.");

            try
            {
                return Ok(await repository.GetByName(nome));
            }
            catch (Exception ex)
            {
                await logger.Log(ex);

                return InternalServerError();
            }
        }


        // POST: api/Funcionarios
        public async Task<IHttpActionResult> Post([FromBody] Models.Funcionario funcionario)
        {
            if (funcionario == null)
                return BadRequest("Os dados do Funcionário não foram enviados corretamente!");

            try
            {
                await repository.Add(funcionario);  
                return Ok(funcionario);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();
            }
        }

        // POST: api/Funcionarios/batch
        [Route("api/Funcionarios/batch")]
        public async Task<IHttpActionResult> Post([FromBody] List<Models.Funcionario> listaDeFuncionarios)
        {
            try
            {
                await repository.Add(listaDeFuncionarios);
                return Ok(listaDeFuncionarios);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();
            }
        }


        // PUT: api/Funcionarios/5
        [Route("api/Funcionarios/{codigo}")]
        public async Task<IHttpActionResult> Put(int codigo, [FromBody] Models.Funcionario funcionario)
        {
            if (funcionario == null)
            {
                return BadRequest("Os dados do funcionário não foram  enviados corretamente!");
            }

            if (funcionario.Codigo != codigo)
            {
                return BadRequest("O código da rota não corresponde ao código do Funcionário");
            }

            try
            {
               bool resposta = await repository.Update(codigo, funcionario);

                if (!resposta)
                    return NotFound();

                return Ok(funcionario);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();

            }

        }

        // DELETE: api/Funcionarios/5
        [Route("api/Funcionarios/{codigo}")]
        public async Task<IHttpActionResult> Delete(int codigo)
        {
            try
            {
                bool resposta = await repository.Delete(codigo);

                if (!resposta)
                    return NotFound();

                return Ok();

            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();

            }
        }
    }
}
