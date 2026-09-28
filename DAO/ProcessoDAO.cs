using Seila.Components;
using Seila.Configs;
using Seila.Model;

namespace Seila.Model
{
    public class ProcessoDAO
    {
        private readonly Conexao _conexao;

        public ProcessoDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        public List<Processo> Listar()
        {
            try
            {
                var lista = new List<Processo>();

                using var con = _conexao.GetConnection();

                string sql = "SELECT * FROM processos";
                using var comando = con.CreateCommand();
                comando.CommandText = sql;

                using var leitor = comando.ExecuteReader();

                while (leitor.Read())
                {
                    var processo = new Processo();

                    processo.Id = Convert.ToInt32(leitor["id_pro"]);
                    processo.Numero = leitor.GetString("numero_pro");
                    processo.Interessado = leitor.GetString("interessado_pro");
                    processo.Assunto = leitor.GetString("assunto_pro");
                    processo.Descricao = leitor.GetString("descricao_pro");
                    processo.Situacao = leitor.GetString("situacao_pro");

                    lista.Add(processo);
                }

                return lista;
            }
            catch
            {
                throw;
            }
        }

        public void Inserir(Processo processo)
        {
            try
            {
                using var con = _conexao.GetConnection();

                string sql = @"INSERT INTO processos
                    (numero_pro, data_pro, interessado_pro, assunto_pro, descricao_pro, situacao_pro)
                    VALUES
                    (@numero, @data, @interessado, @assunto, @descricao, @situacao)";

                using var comando = con.CreateCommand();
                comando.CommandText = sql;

                comando.Parameters.AddWithValue("@numero", processo.Numero);
                comando.Parameters.AddWithValue(
                    "@data",
                    processo.Data!.Value.ToDateTime(TimeOnly.MinValue)
                );
                comando.Parameters.AddWithValue("@interessado", processo.Interessado);
                comando.Parameters.AddWithValue("@assunto", processo.Assunto);
                comando.Parameters.AddWithValue("@descricao", processo.Descricao);
                comando.Parameters.AddWithValue("@situacao", processo.Situacao);

                comando.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
    }
}