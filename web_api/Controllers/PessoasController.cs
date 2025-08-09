using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using web_api.Utils.Log;

namespace web_api.Controllers
{
    public class PessoasController : ApiController
    {
        readonly string connectionString;
        readonly Logger logger;

        public PessoasController()
        {
            //connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["web_api"].ConnectionString;
            connectionString = Configurations.Config.GetConnectionStringSQLServer();
            logger = new Logger(Configurations.Config.GetLogPath());
            //logPath = System.Configuration.ConfigurationManager.AppSettings["logPath"];

        }

        // GET: api/Pessoas
        //[HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                List<Models.Pessoa> pessoas = new List<Models.Pessoa>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select Id, Nome, Idade from Pessoa;";
                        SqlDataReader dr = await cmd.ExecuteReaderAsync();

                        while (await dr.ReadAsync())
                        {
                            Models.Pessoa pessoa = new Models.Pessoa();
                            pessoa.Id = (int)dr["Id"];
                            pessoa.Nome = dr["Nome"].ToString();
                            pessoa.Idade = (int)dr["Idade"];

                            pessoas.Add(pessoa);
                        }

                    } //Dispose feito pelo using                    

                } //Close e Dispose feitos pelo using.

                return Ok(pessoas);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();
            }
        }

        // GET: api/Pessoas/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                Models.Pessoa pessoa = new Models.Pessoa();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select Id, Nome, Idade from Pessoa where Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                        SqlDataReader dr = await cmd.ExecuteReaderAsync();

                        if (await dr.ReadAsync())
                        {
                            pessoa.Id = (int)dr["Id"];
                            pessoa.Nome = dr["Nome"].ToString();
                            pessoa.Idade = (int)dr["Idade"];
                        }
                    }
                }

                if (pessoa.Id == 0)
                    return NotFound();

                return Ok(pessoa);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();
            }
        }

        [Route("api/pessoas/{nome:alpha}")]
        public async Task<IHttpActionResult> Get(string nome)
        {
            if (nome.Length < 3)
                return BadRequest("Informe o mínimo de 3 caracteres no nome do Pessoa.");

            try
            {
                List<Models.Pessoa> pessoas = new List<Models.Pessoa>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select Id, Nome, Idade from Pessoa where Nome like @nome;";
                        cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";

                        SqlDataReader dr = await cmd.ExecuteReaderAsync();

                        while (await dr.ReadAsync())
                        {
                            Models.Pessoa pessoa = new Models.Pessoa();
                            pessoa.Id = (int)dr["Id"];
                            pessoa.Nome = dr["Nome"].ToString();
                            pessoa.Idade = (int)dr["Idade"];

                            pessoas.Add(pessoa);
                        }

                    } //Dispose feito pelo using                    

                } //Close e Dispose feitos pelo using.

                return Ok(pessoas);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();
            }
        }

        // POST: api/Pessoas
        public async Task<IHttpActionResult> Post([FromBody] Models.Pessoa pessoa)
        {
            if (pessoa == null)
                return BadRequest("Os dados do Pessoa não foram enviados corretamente!");

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;

                        cmd.CommandText = "insert into Pessoa (nome, idade) values (@nome, @idade); select scope_identity();";
                        cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = pessoa.Nome;
                        cmd.Parameters.Add(new SqlParameter("@idade", SqlDbType.Int)).Value = pessoa.Idade;

                        pessoa.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    }
                }

                return Ok(pessoa);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();
            }
        }

        // PUT: api/Pessoas/5
        public async Task<IHttpActionResult> Put(int id, [FromBody] Models.Pessoa pessoa)
        {
            if (pessoa == null)
                return BadRequest("Os dados do Pessoa não foram enviados corretamente!");

            if (pessoa.Id != id)
                return BadRequest("O id da rota não corresponde ao id do Pessoa!");

            try
            {
                int linhasAfetadas = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "update Pessoa set nome = @nome, idade = @idade where Id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = pessoa.Nome;
                        cmd.Parameters.Add(new SqlParameter("@idade", SqlDbType.Int)).Value = pessoa.Idade;
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                        linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                    }
                }

                if (linhasAfetadas == 0)
                    return NotFound();

                return Ok(pessoa);
            }
            catch (Exception ex)
            {
                await logger.Log(ex);
                return InternalServerError();
            }
        }

        // DELETE: api/Pessoas/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                int linhasAfetadas = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "delete from Pessoa where Id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                        linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                    }
                }

                if (linhasAfetadas == 0)
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
