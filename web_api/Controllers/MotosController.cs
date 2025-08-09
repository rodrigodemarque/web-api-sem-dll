using System;
using System.Threading.Tasks;
using System.Web.Http;
using web_api.Utils.Log;

namespace web_api.Controllers
{
    public class MotosController : ApiController
    {
        //readonly string logPath;
        readonly Repositories.Moto repository;
        readonly Logger logger;
        public MotosController()
        {
            repository = new Repositories.Moto(Configurations.Config.GetConnectionStringSQLServer());
            logger = new Logger(Configurations.Config.GetLogPath());
        //logPath = Configurations.Config.GetLogPath();
        }
/*
        // GET: api/Motos
        //[HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await repository.GetAll());
            }
            catch (Exception ex)
            {
                await Utils.Logger.Log(logPath, ex);

                return InternalServerError();
            }
        }

        // GET: api/Motos/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                Models.Moto moto = await repository.GetById(id);

                if (carro.Id == 0)
                    return NotFound();

                return Ok(carro);
            }
            catch (Exception ex)
            {
                await Utils.Logger.Log(logPath, ex);

                return InternalServerError();
            }
        }

        [Route("api/motos/{nome:alpha}")]
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
                await Utils.Logger.Log(logPath, ex);

                return InternalServerError();
            }
        }
*/
        // POST: api/Motos
        public async Task<IHttpActionResult> Post([FromBody] Models.Moto moto)
        {
            if (!ModelState.IsValid || moto is null)
                return BadRequest("Os dados da moto não foram enviados corretamente!");

            try
            {
                await repository.Add(moto);
                return Ok(moto);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);

                return InternalServerError();
            }
        }
/*
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
                await Utils.Logger.Log(logPath, ex);

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
                await Utils.Logger.Log(logPath, ex);

                return InternalServerError();
            }
        }
*/
    }
}
