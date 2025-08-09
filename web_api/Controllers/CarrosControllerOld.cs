using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using System.Threading.Tasks;



namespace web_api.Controllers
{
    public class CarrosController : ApiController
    {
        readonly string connectionString;
        readonly string logPath;

        public CarrosController()
        {
            //connectionString = @"Server=TRADER-STT\SQLEXPRESS;Database=web-api;Trusted_Connection=True;";
            //connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["web_api"].ConnectionString;
            connectionString = Configurations.Config.GetConnectionString();

            //logPath = System.Configuration.ConfigurationManager.AppSettings["logPath"];
            logPath = Configurations.Config.GetLogPath();
        }

        // GET: api/Carros
        //[HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<Models.Carro> carros = new List<Models.Carro>();

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select Id, Nome, Valor from Carro;";
                        SqlDataReader dr = await cmd.ExecuteReaderAsync();

                        while (await dr.ReadAsync())
                        {
                            Models.Carro carro = new Models.Carro();
                            carro.Id = (int)dr["Id"];
                            carro.Nome = dr["Nome"].ToString();
                            carro.Valor = Convert.ToDouble(dr["Valor"]);

                            carros.Add(carro);
                        }

                    } //Dispose feito pelo using                    

                } //Close e Dispose feitos pelo using.

                return Ok(carros);
            }
            catch (Exception ex)
            {
                await Utils.Logger.Log(logPath, ex);
            }
            return InternalServerError();

            //return InternalServerError(ex);
        }


        // GET: api/Carros/5
        public async Task<IHttpActionResult> Get(int id)
        {
            Models.Carro carro = new Models.Carro();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select Id, Nome, Valor from Carro where Id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;
                        SqlDataReader dr = await cmd.ExecuteReaderAsync();

                        if (await dr.ReadAsync())
                        {
                            carro.Id = (int)dr["Id"];
                            carro.Nome = dr["Nome"].ToString();
                            carro.Valor = Convert.ToDouble(dr["Valor"]);
                        }
                    }
                }
                if (carro.Id == 0)
                    return NotFound();

                return Ok(carro);
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(logPath, ex);
                return InternalServerError();

            }
        }


        // GET: api/Carros/Nome/nome
        [Route("api/carros/{nome:alpha}")]
        public async Task<IHttpActionResult> Get(string nome)
        {
            Models.Carro carro = new Models.Carro();
            try
            {
                if (nome.Length < 3)
                    return BadRequest("Informe no mínimo 3 caracteres para pesquisar um carro!");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select Id, Nome, Valor from Carro where Nome like @nome;";
                        cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = $"%{nome}%";
                        SqlDataReader dr = await cmd.ExecuteReaderAsync();

                        if (await dr.ReadAsync())
                        {
                            carro.Id = (int)dr["Id"];
                            carro.Nome = dr["Nome"].ToString();
                            carro.Valor = Convert.ToDouble(dr["Valor"]);
                        }
                    }
                }
                if (carro.Id == 0)
                    return NotFound();

                return Ok(carro);
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(logPath, ex);
                return InternalServerError();
            }
        }


        // POST: api/Carros
        public async Task<IHttpActionResult> Post([FromBody] Models.Carro carro)
        {
            try
            {
                if (carro == null)
                    return BadRequest("Os dados do carro não foram enviados corretamente!");
                if (carro.Id == 0)
                    return BadRequest("O Id do carro não pode ser Nulo");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into Carro (Id, Nome, Valor) values (@id, @nome, @valor); select scope_identity();";
                        cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = carro.Id;
                        cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = carro.Nome;
                        cmd.Parameters.Add(new SqlParameter("@valor", System.Data.SqlDbType.Decimal)).Value = carro.Valor;
                        //    cmd.CommandText = $"insert into Carro (Id, Nome, Valor) values ({carro.Id},'{carro.Nome}', {carro.Valor});"; select scope_identity();";
                        await cmd.ExecuteNonQueryAsync();
                        //  carro.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                return Ok(carro);
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(logPath, ex);
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

            int linhasAfetadas = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "update Carro set Nome = @nome, Valor = @valor where Id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;
                        cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = carro.Nome;
                        cmd.Parameters.Add(new SqlParameter("@valor", System.Data.SqlDbType.Decimal)).Value = carro.Valor;

                        linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                    }
                }
                if (linhasAfetadas == 0)
                    return NotFound();

                return Ok(carro);
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(logPath, ex);
                return InternalServerError();
            }

        }

        // DELETE: api/Carros/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            int linhasAfetadas = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "delete from Carro where Id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;
                        linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                    }
                }

                if (linhasAfetadas == 0)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(logPath, ex);
                return InternalServerError();
            }

        }
    }
}
