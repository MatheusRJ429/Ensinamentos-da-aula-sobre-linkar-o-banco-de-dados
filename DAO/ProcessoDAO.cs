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
                    (numero_pro, data_pro, interessado_pro,
                     assunto_pro, descricao_pro, situacao_pro)
                    VALUES
                    (@numero, @data, @interessado,
                     @assunto, @descricao, @situacao)";

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

        public Processo? ObterPorId(int id)
        {
            using var con = _conexao.GetConnection();

            const string sql = @"SELECT id_pro, numero_pro, data_pro,
                    interessado_pro, assunto_pro, descricao_pro, situacao_pro
                    FROM processos
                    WHERE id_pro = @id";

            using var comando = con.CreateCommand();
            comando.CommandText = sql;
            comando.Parameters.AddWithValue("@id", id);

            using var leitor = comando.ExecuteReader();

            if (!leitor.Read())
            {
                return null;
            }

            return new Processo
            {
                Id = Convert.ToInt32(leitor["id_pro"]),
                Numero = leitor.GetString("numero_pro"),
                Data = DateOnly.FromDateTime(
                    Convert.ToDateTime(leitor["data_pro"])
                ),
                Interessado = leitor.GetString("interessado_pro"),
                Assunto = leitor.GetString("assunto_pro"),
                Descricao = leitor.IsDBNull(
                    leitor.GetOrdinal("descricao_pro")
                ) ? string.Empty : leitor.GetString("descricao_pro"),
                Situacao = leitor.GetString("situacao_pro")
            };
        }

        public void Atualizar(Processo processo)
        {
            using var con = _conexao.GetConnection();

            const string sql = @"UPDATE processos
                    SET numero_pro = @numero,
                        data_pro = @data,
                        interessado_pro = @interessado,
                        assunto_pro = @assunto,
                        descricao_pro = @descricao,
                        situacao_pro = @situacao
                    WHERE id_pro = @id";

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
            comando.Parameters.AddWithValue("@id", processo.Id);

            comando.ExecuteNonQuery();
        }
    }
}
