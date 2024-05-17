using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace ProjUninove
{
    public partial class Perfil : Form
    {
        public Perfil()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log l = new Log();
            l.ShowDialog();
            this.Close();
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            string nome = textBoxNome.Text;
            string departamento = comboBoxArea.Text;
            string senha = textBoxSenha.Text;

            // Verifica se os campos obrigatórios estão preenchidos
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(departamento) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios!");
                return;
            }

            // Consulta SQL parametrizada
            string sql = "INSERT INTO login(nome, departamento, senha) VALUES(@nome, @departamento, @senha)";

            // Substituição da string de conexão
            string connectionString = "Server=cd-shell-unii.mysql.database.azure.com;Uid=sr;Pwd=Elder@07;Database=uniinovation;SslMode=Required;";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@departamento", departamento);
                    cmd.Parameters.AddWithValue("@senha", senha);

                    try
                    {
                        con.Open();
                        int qtd_linha = cmd.ExecuteNonQuery();

                        if (qtd_linha > 0)
                        {
                            long id = cmd.LastInsertedId;
                            textBoxidUse.Text = id.ToString();
                            MessageBox.Show("Dados salvos com sucesso!");
                        }
                        else
                        {
                            throw new Exception("Nenhum registro afetado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao salvar os dados: " + ex.Message);
                    }
                }
            }
        }
    }
}