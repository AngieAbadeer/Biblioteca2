using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BibiotecaNova
{
    public partial class FormBiblioteca : Form
    {
        public FormBiblioteca()
        {
            InitializeComponent();
        }

        private MySqlConnectionStringBuilder conexaoBanco()
        {
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
            conexaoBD.Server = "localhost";
            conexaoBD.Database = "biblioteca";
            conexaoBD.UserID = "root";
            conexaoBD.Password = "";
            conexaoBD.SslMode = 0;
            return conexaoBD;
        }

        private void FormBiblioteca_Load(object sender, EventArgs e)
        {
            atualizaGrid();
        }

        private void atualizaGrid()
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                comandoMySql.CommandText = "SELECT * FROM livro WHERE statusLivro = 1";
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                dgBiblioteca.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dgBiblioteca.Rows[0].Clone();//FAZ UM CAST E CLONA A LINHA DA TABELA
                    row.Cells[0].Value = reader.GetInt32(0);//ID
                    row.Cells[1].Value = reader.GetString(1);//NOME
                    row.Cells[2].Value = reader.GetString(2);//AUTOR
                    row.Cells[3].Value = reader.GetString(3);//EDITORA
                    row.Cells[4].Value = reader.GetString(4);//CATEGORIA
                    row.Cells[5].Value = reader.GetString(5);//DESCRIÇÃO
                    dgBiblioteca.Rows.Add(row);//ADICIONO A LINHA NA TABELA
                }

                realizaConexacoBD.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();  
        }

        private void limparCampos()
        {
            tbNome.Clear();
            tbAutor.Clear();
            tbEditora.Clear();
            tbCategoria.Clear();
            tbDescricao.Clear();
            tbID.Clear();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();

                comandoMySql.CommandText = "INSERT INTO livro (nomeLivro,autorLivro,editoraLivro,categoriaLivro,descricaoLivro) " +
                    "VALUES ('" + tbNome.Text + "', '" + tbAutor.Text + "', '" + tbEditora.Text + "','" + tbCategoria.Text + "', '" + tbDescricao.Text + "')";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close();
                MessageBox.Show("Inserido com sucesso");
                atualizaGrid();
                limparCampos();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open(); //Abre a conexão com o banco

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); //Crio um comando SQL
                comandoMySql.CommandText = "UPDATE livro SET 'nomeLivro' = '" + tbNome.Text + "', " +
                    "autorLivro = '" + tbAutor.Text + "', " +                                     
                    "editoraLivro = '" + tbEditora.Text + "', " +
                    "categoriaLivro = '" + tbCategoria.Text + "', " +
                    "descricaoLivro = '" + tbDescricao.Text + "', " +
                    " WHERE idLivro = " + tbID.Text + "";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close(); // Fecho a conexão com o banco
                MessageBox.Show("Atualizado com sucesso"); //Exibo mensagem de aviso
                atualizaGrid();
                limparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnInativar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open(); //Abre a conexão com o banco

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); //Crio um comando SQL              
                comandoMySql.CommandText = "UPDATE livro SET statusLivro = 0 WHERE idLivro = " + tbID.Text + "";

                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close(); // Fecho a conexão com o banco
                MessageBox.Show("Inativado com sucesso"); //Exibo mensagem de aviso
                atualizaGrid();
                limparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO");
                Console.WriteLine(ex.Message);
            }
        }

        private void dgBiblioteca_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgBiblioteca.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgBiblioteca.CurrentRow.Selected = true;
                //preenche os textbox com as células da linha selecionada
                tbNome.Text = dgBiblioteca.Rows[e.RowIndex].Cells["colNome"].FormattedValue.ToString();
                tbCategoria.Text = dgBiblioteca.Rows[e.RowIndex].Cells["colCategoria"].FormattedValue.ToString();
                tbDescricao.Text = dgBiblioteca.Rows[e.RowIndex].Cells["colDescricao"].FormattedValue.ToString();
                tbAutor.Text = dgBiblioteca.Rows[e.RowIndex].Cells["colAutor"].FormattedValue.ToString();
                tbID.Text = dgBiblioteca.Rows[e.RowIndex].Cells["colID"].FormattedValue.ToString();
                tbEditora.Text = dgBiblioteca.Rows[e.RowIndex].Cells["colEditora"].FormattedValue.ToString();
            }
        }
    }
}
