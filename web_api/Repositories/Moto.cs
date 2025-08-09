using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using web_api.Models;

namespace web_api.Repositories
{
    public class Moto
    {
        readonly SqlConnection conn;
        readonly SqlCommand cmd;

        public Moto(string connectionString)
        {
            conn = new SqlConnection(connectionString);

            cmd = new SqlCommand();
            cmd.Connection = conn;
        }

        public async Task Add(Models.Moto moto)
        {
            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "insert into Moto (nome, valor) values (@nome, @valor); select scope_identity();";
                    AdicionalparametrosPadrao(cmd, moto);
                    moto.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
            }
        }

        private void AdicionalparametrosPadrao(SqlCommand cmd, Models.Moto moto)
        {
            cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = moto.Nome;
            cmd.Parameters.Add(new SqlParameter("@valor", SqlDbType.Decimal)).Value = moto.Valor;
        }
    }
}