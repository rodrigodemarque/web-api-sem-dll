using System.Web.Http;
using System;
using System.Threading.Tasks;
using web_api.Utils.Log;
using web_api.Interfaces;

namespace web_api.Controllers
{
    public class CarrosController : ApiController
    {
        readonly Logger logger;
        readonly IRepository<Models.Carro> repository;
        //readonly Repositories.Carro repository;
        public CarrosController()
        {
            logger = new Logger(Configurations.Config.GetLogPath());
            repository = new Repositories.SQLServer.Carro(Configurations.Config.GetConnectionStringSQLServer());
            repository.CacheExpirationTime = Configurations.Config.GetCacheExpirationTimeInSeconds("cacheExpirationTimeInSeconds");
        }

        // GET: api/Carros
        //[HttpGet]
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

        // GET: api/Carros/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                Models.Carro carro = await repository.GetById(id);

                if (carro.Id == 0)
                    return NotFound();

                return Ok(carro);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);

                return InternalServerError();
            }
        }

        [Route("api/carros/{nome:alpha}")]
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

        // POST: api/Carros
        public async Task<IHttpActionResult> Post([FromBody] Models.Carro carro)
        {
            if (carro == null)
                return BadRequest("Os dados do carro não foram enviados corretamente!");

            try
            {
                await repository.Add(carro);
                return Ok(carro);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);

                return InternalServerError();
            }
        }

        // PUT: api/Carros/5
        public async Task<IHttpActionResult> Put(int id, [FromBody] Models.Carro carro)
        {
            if (carro == null)
                return BadRequest("Os dados do carro não foram enviados corretamente!");

            if (carro.Id != id)
                return BadRequest("O id da rota não corresponde ao id do carro!");

            try
            {
                bool resposta = await repository.Update(carro);

                if (!resposta)
                    return NotFound();

                return Ok(carro);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);

                return InternalServerError();
            }
        }

        // DELETE: api/Carros/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                bool resposta = await repository.Delete(id);

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
