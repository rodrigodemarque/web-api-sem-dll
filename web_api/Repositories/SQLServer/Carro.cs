using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using web_api.Interfaces;
using web_api.Utils.Cache;


namespace web_api.Repositories.SQLServer
{
    public class Carro : IRepository<Models.Carro>
    {
        readonly SqlConnection conn;
        readonly SqlCommand cmd;
        readonly Interfaces.ICacheService cacheService;
        readonly string KeyCache;

        public int CacheExpirationTime { get; set; }
        
        public Carro(string connectionString) 
        {
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cacheService = new MemoryCacheService();
            KeyCache = "carros";
          //  CacheExpirationTime = 30;
        }

        public async Task<List<Models.Carro>> GetAll()
        {
            List<Models.Carro> carros;
            carros = cacheService.Get<List<Models.Carro>>(KeyCache);

            if (carros != null)
                return (carros);

           carros = new List<Models.Carro>();

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {   
                    cmd.CommandText = "select Id, Nome, Valor from Carro;";
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    while (await dr.ReadAsync())
                    {
                        Models.Carro carro = new Models.Carro();

                        Mapper(carro, dr);

                        carros.Add(carro);
                    }

                }    //Dispose feito pelo using                    

            }    //Close e Dispose feitos pelo using.
            cacheService.Set(KeyCache,carros,CacheExpirationTime);
            return carros;
        }

        public async Task<Models.Carro> GetById(int id)
        {
            Models.Carro carro = new Models.Carro();

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select Id, Nome, Valor from Carro where Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    if (await dr.ReadAsync())
                    {
                        Mapper(carro, dr);
                    }
                }
            }

            return carro;
        }

        public async Task<List<Models.Carro>> GetByName(string nome)
        {
            List<Models.Carro> carros = new List<Models.Carro>();

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select Id, Nome, Valor from Carro where Nome like @nome;";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";

                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    while (await dr.ReadAsync())
                    {
                        Models.Carro carro = new Models.Carro();

                        Mapper(carro, dr);

                        carros.Add(carro);
                    }

                } //Dispose feito pelo using                    

            } //Close e Dispose feitos pelo using.

            return carros;
        }

        public async Task Add(Models.Carro carro)
        {
            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {   
                    cmd.CommandText = "insert into Carro (nome, valor) values (@nome, @valor); select scope_identity();";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = carro.Nome;
                    cmd.Parameters.Add(new SqlParameter("@valor", SqlDbType.Decimal)).Value = carro.Valor;

                    carro.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                    cacheService.Remove(KeyCache);
                }
            }

        }

        public async Task<bool> Update(Models.Carro carro)
        {
            int linhasAfetadas = 0;

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "update Carro set nome = @nome, valor = @valor where Id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = carro.Nome;
                    cmd.Parameters.Add(new SqlParameter("@valor", SqlDbType.Decimal)).Value = carro.Valor;
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = carro.Id;

                    linhasAfetadas = await cmd.ExecuteNonQueryAsync();

                }
            }

            if (linhasAfetadas == 0)
                return false;

            cacheService.Remove(KeyCache);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            int linhasAfetadas = 0;

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {                 
                    cmd.CommandText = "delete from Carro where Id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                }
            }

            if (linhasAfetadas == 0)
                return false;

            cacheService.Remove(KeyCache);
            return true;
        }

        private void Mapper(Models.Carro carro, SqlDataReader dr)
        {
            carro.Id = (int)dr["Id"];
            carro.Nome = dr["Nome"].ToString();
            carro.Valor = Convert.ToDouble(dr["Valor"]);
        }
    }
}