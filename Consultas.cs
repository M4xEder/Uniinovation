using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace ProjUninove
{
    public partial class Consultas : Form
    {
        DataTable dt = new DataTable();
        public Consultas()
        {
            InitializeComponent();
            AtualizarDataGridView();
        }
        private void buttonExcluir_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtém o valor da coluna "cpf" da linha selecionada
                string cpfParaExcluir = dataGridView1.SelectedRows[0].Cells["cpf"].Value.ToString();
                // Chama o método para excluir o registro do banco de dados
                ExcluirRegistro(cpfParaExcluir);
                // Atualiza o DataGridView após a exclusão
                AtualizarDataGridView();
            }
            else
            {
                MessageBox.Show("Selecione uma linha para excluir.");
            }
        }
        private void ExcluirRegistro(string cpf)
        {
            // Crie sua lógica para excluir o registro no banco de dados
            MySqlConnection con = null;
            MySqlCommand cmd = null;
            try
            {
                con = new MySqlConnection("server=cd-shell-unii.mysql.database.azure.com;uid=sr;pwd=Elder@07;database=uniinovation;SslMode=Required;");
                con.Open();

                // Use a coluna chave primária para identificar o registro a ser excluído cpf
                string sql = $"DELETE FROM cadastros WHERE cpf = '{cpf}'";

                cmd = new MySqlCommand(sql, con);
                int qtdLinhasAfetadas = cmd.ExecuteNonQuery();

                if (qtdLinhasAfetadas > 0)
                {
                    MessageBox.Show("Registro excluído com sucesso!");
                }
                else
                {
                    MessageBox.Show("Não foi possível excluir o registro.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir o registro: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
        }
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log l = new Log();
            l.ShowDialog();
            this.Close();
        }

        private void cadastrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Principal p = new Principal();
            p.ShowDialog();
            this.Close();
        }
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void AtualizarDataGridView()
        {
            MySqlConnection con = null;
            MySqlCommand cmd = null;
            MySqlDataAdapter da = null;
            try

            {
                con = new MySqlConnection("server=cd-shell-unii.mysql.database.azure.com;uid=sr;pwd=Elder@07;database=uniinovation;SslMode=Required;");
                con.Open();

                string sql = "SELECT idUsuario, cpf, email, nome, sexo, dtNasc, telefone, atendimento, observacoes, cep, logradouro, numero, bairro, cidade, estado FROM cadastros";
                cmd = new MySqlCommand(sql, con);

                da = new MySqlDataAdapter(cmd);
                dt = new DataTable(); // Use a variável de classe dt em vez de criar uma nova DataTable
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados do banco de dados: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
                da.Dispose();
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CarregarDadosNoDataGridView();

        }
        private void CarregarDadosNoDataGridView()
        {
            DataTable dt = new DataTable();

            MySqlConnection con = null;
            MySqlCommand cmd = null;
            MySqlDataAdapter da = null;
            try
            {
                con = new MySqlConnection("server=cd-shell-unii.mysql.database.azure.com;uid=sr;pwd=Elder@07;database=uniinovation");
                con.Open();

                // consulta SQL 
                string sql = "SELECT * FROM cadastros";
                cmd = new MySqlCommand(sql, con);

                // Use um DataAdapter para preencher o DataTable
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);

                // DataTable como a fonte de dados do DataGridView
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }
        private void buttonPesquisar_Click(object sender, EventArgs e)
        {
            string nomePesquisa = txtPesquisarNome.Text.Trim();

            if (string.IsNullOrEmpty(nomePesquisa))
            {
                // Se o campo de pesquisa estiver em branco, exibir todos os registros
                AtualizarDataGridView();
            }
            else
            {
                // Caso contrário, chama o método para pesquisar registros no banco de dados por nome
                PesquisarPorNome(nomePesquisa);
            }
        }
        private void PesquisarPorNome(string nome)
        {
            MySqlConnection con = null;
            MySqlCommand cmd = null;
            MySqlDataAdapter da = null;
            try
            {
                con = new MySqlConnection("server=cd-shell-unii.mysql.database.azure.com;uid=sr;pwd=Elder@07;database=uniinovation;SslMode=Required;");
                con.Open();

                // Use a cláusula LIKE para pesquisar por parte do nome
                string sql = $"SELECT * FROM cadastros WHERE nome LIKE '%{nome}%'";

                cmd = new MySqlCommand(sql, con);
                da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Atualiza o DataGridView com os resultados da pesquisa
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Nenhum registro encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao realizar a pesquisa: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
                da.Dispose();
            }
        }
        private void registradosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void tabelaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Consultas r = new Consultas();
            r.ShowDialog();
            this.Close();
        }

    }
}
