using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
namespace ProjUninove
{
    public partial class Log : Form
    {
        // Conexão com o banco de dados MySQL na Azure
        string connectionString = "Server=cd-shell-unii.mysql.database.azure.com;Database=uniinovation;Uid=sr;Pwd=Elder@07;SslMode=Required;";

        public Log()
        {
            InitializeComponent();
        }

        private void buttonEntrar_Click(object sender, EventArgs e)
        {
            string id = textBoxIdUse.Text;
            string senha = textBoxSenha.Text;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha o ID do usuário e senha.");
                return;
            }

            string sql = "SELECT COUNT(*) FROM login WHERE id=@id AND senha=@senha";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@senha", senha);

                        con.Open();

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            this.Hide();
                            Principal p = new Principal();
                            p.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("ID de usuário ou senha inválidos.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Houve um erro no sistema:\n{ex.Message}");
            }
        }

        private void labelNaoTenhoCadastro_Click(object sender, EventArgs e)
        {
            this.Hide();
            Perfil p = new Perfil();
            p.ShowDialog();
            this.Close();
        }

        private void labelSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}