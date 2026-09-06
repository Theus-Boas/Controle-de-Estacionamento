using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Mysqlx.Datatypes;
namespace Placa
{
    internal class Functions
    {
        string sqlconect = "server=localhost;database=estacionamento_veiculos;uid=root;pwd=";
        public async Task ReservarVaga(string script, string placa)
        {
            try {
                using (var con = new MySqlConnection(sqlconect))
                {
                    con.Open();
                    MySqlCommand add = new MySqlCommand(script, con);
                    add.Parameters.AddWithValue("@placa", placa);
                    add.ExecuteNonQuery();
                    Console.WriteLine($"Vaga reservada para o veículo {placa}!!\n");
                    await Task.Delay(2000);
                }
            }
            catch (Exception erro) {
                Console.WriteLine("Falha na conexao com o banco de dados!");
                Console.WriteLine($"Erro: {erro}");
            }

        }
        public async Task RetirarCarro(string script, string placa, int id)
        {
            Console.Clear();
            try {
                using (var conn = new MySqlConnection(sqlconect)) {
                    conn.Open();
                    MySqlCommand delet = new MySqlCommand(script, conn);
                    delet.Parameters.AddWithValue("@id", id);
                    delet.ExecuteNonQuery();
                    Console.WriteLine("Vaga liberada com sucesso!!");
                    await Task.Delay(2000);
                }
            }
            catch (Exception erro) {
                Console.WriteLine("Falha na conexao com o banco de dados!");
                Console.WriteLine($"Erro: {erro}");
            }
        }
    }
}