using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace web_api.Repositories
{
    public class Funcionario
    {
        readonly SqlConnection conn;
        readonly SqlCommand cmd;

        public Funcionario(string connectionString)
        {
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.Connection = conn;
        }

        public async Task<List<Models.Funcionario>> GetAll()
        {
            List<Models.Funcionario> listaDeFuncionarios = new List<Models.Funcionario>();


            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select Codigo, CodigoDepartamento, PrimeiroNome, SegundoNome, UltimoNome, DataNascimento, CPF, RG, Endereco, CEP, Cidade, Fone, Funcao, Salario from Funcionario;";
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    while (await dr.ReadAsync())
                    {
                        Models.Funcionario funcionario = new Models.Funcionario();

                        Mapper(funcionario, dr);

                        listaDeFuncionarios.Add(funcionario);
                    }

                }
            }
            return listaDeFuncionarios;
        }

        public async Task<Models.Funcionario> GetById(int codigo)
        {
            Models.Funcionario funcionario = new Models.Funcionario();

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select Codigo, CodigoDepartamento, PrimeiroNome, SegundoNome, UltimoNome, DataNascimento, CPF, RG, Endereco, CEP, Cidade, Fone, Funcao, Salario from Funcionario where Codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", SqlDbType.Int)).Value = codigo;

                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    if (await dr.ReadAsync())
                    {
                        Mapper(funcionario, dr);
                    }
                }
            }
            return funcionario;
        }

        public async Task<List<Models.Funcionario>> GetByName(string nome)
        {
            List<Models.Funcionario> listaDeFuncionarios = new List<Models.Funcionario>();

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select Codigo, CodigoDepartamento, PrimeiroNome, SegundoNome, UltimoNome, DataNascimento, CPF, RG, Endereco, CEP, Cidade, Fone, Funcao, Salario from Funcionario where PrimeiroNome like @primeironome;";
                    cmd.Parameters.Add(new SqlParameter("@primeironome", SqlDbType.VarChar)).Value = $"%{nome}%";

                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    while (await dr.ReadAsync())
                    {
                        Models.Funcionario funcionario = new Models.Funcionario();

                        Mapper(funcionario, dr);

                        listaDeFuncionarios.Add(funcionario);
                    }

                }
            }
            return listaDeFuncionarios;
        }

        public async Task Add(Models.Funcionario funcionario)
        {
            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "insert into Funcionario (CodigoDepartamento, PrimeiroNome, SegundoNome, UltimoNome, DataNascimento, CPF, RG, Endereco, CEP, Cidade, Fone, Funcao, Salario)" +
                        "values (@codigoDepartamento, @primeiroNome, @segundoNome, @ultimoNome, @dataNascimento, @CPF, @RG, @endereco, @CEP, @cidade, @fone, @funcao, @salario); select scope_identity();";
                    cmd.Parameters.Add(new SqlParameter("@codigoDepartamento", SqlDbType.Int)).Value = funcionario.CodigoDepartamento;
                    cmd.Parameters.Add(new SqlParameter("@PrimeiroNome", SqlDbType.VarChar)).Value = funcionario.PrimeiroNome;
                    cmd.Parameters.Add(new SqlParameter("@SegundoNome", SqlDbType.VarChar)).Value = funcionario.SegundoNome;
                    cmd.Parameters.Add(new SqlParameter("@UltimoNome", SqlDbType.VarChar)).Value = funcionario.UltimoNome;
                    cmd.Parameters.Add(new SqlParameter("@DataNascimento", SqlDbType.Date)).Value = funcionario.DataNascimento;
                    cmd.Parameters.Add(new SqlParameter("@CPF", SqlDbType.Char)).Value = funcionario.CPF;
                    cmd.Parameters.Add(new SqlParameter("@RG", SqlDbType.Char)).Value = funcionario.RG;
                    cmd.Parameters.Add(new SqlParameter("@Endereco", SqlDbType.VarChar)).Value = funcionario.Endereco;
                    cmd.Parameters.Add(new SqlParameter("@CEP", SqlDbType.VarChar)).Value = funcionario.CEP;
                    cmd.Parameters.Add(new SqlParameter("@Cidade", SqlDbType.VarChar)).Value = funcionario.Cidade;
                    cmd.Parameters.Add(new SqlParameter("@Fone", SqlDbType.VarChar)).Value = funcionario.Fone;
                    cmd.Parameters.Add(new SqlParameter("@Funcao", SqlDbType.VarChar)).Value = funcionario.Funcao;
                    cmd.Parameters.Add(new SqlParameter("@Salario", SqlDbType.Decimal)).Value = funcionario.Salario;

                    funcionario.Codigo = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
            }
        }

        public async Task Add(List<Models.Funcionario> listaDeFuncionarios)
        {
            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    foreach (var item in listaDeFuncionarios)
                    {
                        cmd.CommandText = "insert into Funcionario (CodigoDepartamento, PrimeiroNome, SegundoNome, UltimoNome, DataNascimento, CPF, RG, Endereco, CEP, Cidade, Fone, Funcao, Salario)" +
                         "values (@codigoDepartamento, @primeiroNome, @segundoNome, @ultimoNome, @dataNascimento, @CPF, @RG, @endereco, @CEP, @cidade, @fone, @funcao, @salario); select scope_identity();";
                        cmd.Parameters.Add(new SqlParameter("@codigoDepartamento", SqlDbType.Int)).Value = item.CodigoDepartamento;
                        cmd.Parameters.Add(new SqlParameter("@PrimeiroNome", SqlDbType.VarChar)).Value = item.PrimeiroNome;
                        cmd.Parameters.Add(new SqlParameter("@SegundoNome", SqlDbType.VarChar)).Value = item.SegundoNome;
                        cmd.Parameters.Add(new SqlParameter("@UltimoNome", SqlDbType.VarChar)).Value = item.UltimoNome;
                        cmd.Parameters.Add(new SqlParameter("@DataNascimento", SqlDbType.Date)).Value = item.DataNascimento;
                        cmd.Parameters.Add(new SqlParameter("@CPF", SqlDbType.Char)).Value = item.CPF;
                        cmd.Parameters.Add(new SqlParameter("@RG", SqlDbType.Char)).Value = item.RG;
                        cmd.Parameters.Add(new SqlParameter("@Endereco", SqlDbType.VarChar)).Value = item.Endereco;
                        cmd.Parameters.Add(new SqlParameter("@CEP", SqlDbType.VarChar)).Value = item.CEP;
                        cmd.Parameters.Add(new SqlParameter("@Cidade", SqlDbType.VarChar)).Value = item.Cidade;
                        cmd.Parameters.Add(new SqlParameter("@Fone", SqlDbType.VarChar)).Value = item.Fone;
                        cmd.Parameters.Add(new SqlParameter("@Funcao", SqlDbType.VarChar)).Value = item.Funcao;
                        cmd.Parameters.Add(new SqlParameter("@Salario", SqlDbType.Decimal)).Value = item.Salario;
                        item.Codigo = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        cmd.Parameters.Clear();
                    }
                }
            }
          }



        public async Task<bool> Update(int codigo, Models.Funcionario funcionario)
        {
            int linhasAfetadas = 0;

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "update Funcionario set CodigoDepartamento = @codigoDepartamento, PrimeiroNome = @primeiroNome, SegundoNome = @segundoNome, UltimoNome = @ultimoNome, DataNascimento = @dataNascimento," +
                        "CPF = @CPF, RG = @RG, Endereco = @Endereco, CEP = @CEP, Cidade = @cidade, Fone = @fone, Funcao = @funcao, Salario = @salario where Codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", SqlDbType.Int)).Value = codigo;
                    cmd.Parameters.Add(new SqlParameter("@codigoDepartamento", SqlDbType.Int)).Value = funcionario.CodigoDepartamento;
                    cmd.Parameters.Add(new SqlParameter("@PrimeiroNome", SqlDbType.VarChar)).Value = funcionario.PrimeiroNome;
                    cmd.Parameters.Add(new SqlParameter("@SegundoNome", SqlDbType.VarChar)).Value = funcionario.SegundoNome;
                    cmd.Parameters.Add(new SqlParameter("@UltimoNome", SqlDbType.VarChar)).Value = funcionario.UltimoNome;
                    cmd.Parameters.Add(new SqlParameter("@DataNascimento", SqlDbType.Date)).Value = funcionario.DataNascimento;
                    cmd.Parameters.Add(new SqlParameter("@CPF", SqlDbType.Char)).Value = funcionario.CPF;
                    cmd.Parameters.Add(new SqlParameter("@RG", SqlDbType.Char)).Value = funcionario.RG;
                    cmd.Parameters.Add(new SqlParameter("@Endereco", SqlDbType.VarChar)).Value = funcionario.Endereco;
                    cmd.Parameters.Add(new SqlParameter("@CEP", SqlDbType.VarChar)).Value = funcionario.CEP;
                    cmd.Parameters.Add(new SqlParameter("@Cidade", SqlDbType.VarChar)).Value = funcionario.Cidade;
                    cmd.Parameters.Add(new SqlParameter("@Fone", SqlDbType.VarChar)).Value = funcionario.Fone;
                    cmd.Parameters.Add(new SqlParameter("@Funcao", SqlDbType.VarChar)).Value = funcionario.Funcao;
                    cmd.Parameters.Add(new SqlParameter("@Salario", SqlDbType.Decimal)).Value = funcionario.Salario;

                    linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                }
            }
            return linhasAfetadas == 1;
        }

        public async Task<bool> Delete(int codigo)
        {

            int linhasAfetadas = 0;

            using (conn)
            {
                await conn.OpenAsync();
                using (cmd)
                {
                    cmd.CommandText = "delete from Funcionario where Codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", SqlDbType.Int)).Value = codigo;
                    linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                }
            }
            return (linhasAfetadas == 1);
        }


        private void Mapper(Models.Funcionario funcionario, SqlDataReader dr)
        {
            funcionario.Codigo = (int)dr["Codigo"];
            funcionario.CodigoDepartamento = (int)dr["CodigoDepartamento"];
            funcionario.PrimeiroNome = dr["PrimeiroNome"].ToString();
            funcionario.SegundoNome = dr["SegundoNome"].ToString();
            funcionario.UltimoNome = dr["UltimoNome"].ToString();
            funcionario.DataNascimento = (DateTime)dr["DataNascimento"];
            funcionario.CPF = dr["CPF"].ToString();
            funcionario.RG = dr["RG"].ToString();
            funcionario.Endereco = dr["Endereco"].ToString();
            funcionario.CEP = dr["CEP"].ToString();
            funcionario.Cidade = dr["Cidade"].ToString();
            funcionario.Fone = dr["Fone"].ToString();
            funcionario.Funcao = dr["Funcao"].ToString();
            funcionario.Salario = Convert.ToDouble(dr["Salario"]);
        }


    }

}