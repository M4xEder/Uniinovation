using System;
using System.Data;
using System.Windows.Forms;
using Newtonsoft.Json.Linq; // localizar Cep API
using System.IO;
using System.Net;
using MySql.Data.MySqlClient;

namespace ProjUninove
{
    public partial class Principal : Form
    {
        // int h = 0, m = 0;
        private DataTable dt; // data table
        public Principal()
        {

            InitializeComponent();
            AtualizarDataGridView();
        }
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void textBoxNome_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {

        }
        private void Principal_Load(object sender, EventArgs e)
        {

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void registradosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AtualizarDataGridView();
            this.Hide();
            Consultas c = new Consultas();
            c.ShowDialog();
            this.Close();
        }
        private void cadastrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Principal p = new Principal();
            p.ShowDialog();
            this.Close();
        }
        private void loginToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log l = new Log();
            l.ShowDialog();
            this.Close();
        }
        private void cadastroDeAcessoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Perfil p = new Perfil();
            p.ShowDialog();
            this.Close();
        }
        private void tabelaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Consultas r = new Consultas();
            r.ShowDialog();
            this.Close();
        }
        private void buttonLimpar_Click(object sender, EventArgs e)
        {
            textBoxBairro.Text = "";
            textBoxCep.Text = "";
            textBoxBairro.Text = "";
            textBoxEstado.Text = "";
            textBoxCidade.Text = "";
            textBoxLogradouro.Text = "";
            textBoxNumero.Text = "";
            textBoxNumero.Text = "";
            textBoxEstado.Text = "";

        }

        private void buttonBuscarCep_Click(object sender, EventArgs e)
        {
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + textBoxCep.Text + "/json/?ra=123&senha=uni9");
            myReq.ContentType = "application/json";
            myReq.Method = "GET";

            try
            {
                var myResp = (HttpWebResponse)myReq.GetResponse();

                using (var streamReader = new StreamReader(myResp.GetResponseStream()))
                {
                    var resultQR = streamReader.ReadToEnd();
                    string jsonStringsign = resultQR;
                    JObject meuJson = JObject.Parse(jsonStringsign);

                    if (meuJson["erro"] != null && (bool)meuJson["erro"])
                    {
                        MessageBox.Show("CEP inválido. Por favor, insira um CEP válido.", "CEP Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LimparCamposEndereco(); // Limpa os campos de endereço
                    }
                    else
                    {
                        textBoxLogradouro.Text = meuJson["logradouro"].ToString();
                        textBoxBairro.Text = meuJson["bairro"].ToString();
                        textBoxCidade.Text = meuJson["localidade"].ToString();
                        textBoxEstado.Text = meuJson["uf"].ToString();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado ao buscar o CEP.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        private void LimparCamposEndereco()
        {
            textBoxLogradouro.Text = "";
            textBoxBairro.Text = "";
            textBoxCidade.Text = "";
            textBoxEstado.Text = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {

            textBoxNome.Text = "";
            textBoxCpf.Text = "";
            textBoxEmail.Text = "";
            richTextBox1.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            dateTimePicker1.ResetText();
            maskedTextBox1.Text = "";
            listBox1.SelectedIndex = -1;

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
                string sql = "SELECT cpf, email, nome, sexo, dtNasc, telefone, pais, atendimento, observacoes, convenio, cep, logradouro, numero, bairro, cidade, estado FROM cadastros"; //foto
                cmd = new MySqlCommand(sql, con);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable(); // Use a variável de classe dt em vez de criar uma nova DataTable
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
        private void buttonBuscarPorCPF_Click(object sender, EventArgs e) // consultar na tela principal
        {
            string cpf = textBoxCpf.Text;

            // Verifica se o CPF está preenchido
            if (string.IsNullOrWhiteSpace(cpf))
            {
                MessageBox.Show("Por favor, digite um CPF para buscar informações.");
                return;
            }
            string sql = $"SELECT email, nome, sexo, dtNasc, telefone, atendimento, observacoes, cep, logradouro, numero, bairro, cidade, estado FROM cadastros WHERE cpf = '{cpf}'";

            MySqlConnection con = null;
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;

            try
            {
                con = new MySqlConnection("server=cd-shell-unii.mysql.database.azure.com;uid=sr;pwd=Elder@07;database=uniinovation");
                con.Open();
                cmd = new MySqlCommand(sql, con);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Preenche os campos com os dados do banco

                    textBoxNome.Text = reader["nome"].ToString();
                    dateTimePicker1.Text = reader["dtNasc"].ToString();
                    maskedTextBox1.Text = reader["telefone"].ToString();
                    textBoxEmail.Text = reader["email"].ToString();
                    richTextBox1.Text = reader["observacoes"].ToString();
                    textBoxCep.Text = reader["cep"].ToString();
                    textBoxLogradouro.Text = reader["logradouro"].ToString();
                    textBoxNumero.Text = reader["numero"].ToString();
                    textBoxBairro.Text = reader["bairro"].ToString();
                    textBoxCidade.Text = reader["cidade"].ToString();
                    textBoxEstado.Text = reader["estado"].ToString();
                    string sexo = reader["sexo"].ToString();
                    if (sexo == "masc")
                        radioButton1.Checked = true;
                    else if (sexo == "fem")
                        radioButton2.Checked = true;

                    MessageBox.Show("Informações encontradas e preenchidas!");
                }
                else
                {
                    MessageBox.Show("CPF não encontrado no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar informações: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        private void btnSalvar(object sender, EventArgs e)
        {

            string cpf = textBoxCpf.Text;
            string email = textBoxEmail.Text;
            string nome = textBoxNome.Text;
            string sexo = radioButton1.Checked ? "masc" : "fem";
            string dtNasc = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string telefone = maskedTextBox1.Text;
            string atendimento = listBox1.Text;
            string observacoes = richTextBox1.Text;
            string bairro = textBoxBairro.Text;
            string cep = textBoxCep.Text;
            string cidade = textBoxCidade.Text;
            string logradouro = textBoxLogradouro.Text;
            string numero = textBoxNumero.Text;
            string estado = textBoxEstado.Text;

            string sql = $"insert into cadastros(" +
                     "cpf, email, nome, sexo, dtNasc, telefone, atendimento, observacoes, cep, logradouro, numero, bairro, cidade, estado)" +
                     "values(" +
                     $"'{cpf}', '{email}', '{nome}', '{sexo}', '{dtNasc}', '{telefone}', '{atendimento}','{observacoes}','{cep}', '{logradouro}', '{numero}', '{bairro}', '{cidade}', '{estado}')";

            MySqlConnection con = null;
            MySqlCommand cmd = null;

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf))
            {
                MessageBox.Show("Cpf e nome Obrigatorio", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con = new MySqlConnection("server=cd-shell-unii.mysql.database.azure.com;uid=sr;pwd=Elder@07;database=uniinovation;SslMode=Required;");
                cmd = new MySqlCommand(sql, con);
                con.Open();
                int qtdLinhasAfetadas = cmd.ExecuteNonQuery();
                if (qtdLinhasAfetadas > 0)

                {
                    AtualizarDataGridView();
                    //AtualizarGrafico();
                    MessageBox.Show("Dados inseridos com sucesso!");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível inserir!");
                MessageBox.Show($"Erro ao inserir dados: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
        }

    }
}
