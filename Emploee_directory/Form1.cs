using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Emploee_directory
{
    public partial class Form1 : Form
    {
        private static string connectString;

        public Form1()
        {
            InitializeComponent();
        }


        private void LoadData(string con)
        {
            string sqlProcedure = "GetEmployees";
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                string query = "SELECT name FROM dbo.status";
                SqlCommand cmd = new SqlCommand(query, connection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        comboBox1.Items.Add(name);
                    }
                    comboBox1.SelectedItem = comboBox1.Items[0];
                }
            }
        }

        private void GetStat(string query) 
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();

                SqlParameter statusParam = new SqlParameter
                {
                    ParameterName = "@status",
                    Value = comboBox1.SelectedItem.ToString()
                };
                command.Parameters.Add(statusParam);

                SqlParameter dateStartParam = new SqlParameter
                {
                    ParameterName = "@dateStart",
                    Value = dateTimePicker1.Value
                };
                command.Parameters.Add(dateStartParam);

                SqlParameter dateEndParam = new SqlParameter
                {
                    ParameterName = "@dateEnd",
                    Value = dateTimePicker2.Value
                };
                command.Parameters.Add(dateEndParam);

                command.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView2.DataSource = ds.Tables[0];
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"ФИО LIKE '%{textBox1.Text}%'";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Статус LIKE '%{textBox3.Text}%'";
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Отдел LIKE '%{textBox4.Text}%'";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Должность LIKE '%{textBox5.Text}%'";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                GetStat("GetEmployCountByDay");
            }
            if (radioButton2.Checked)
            {
                GetStat("GetUneployCountByDay");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            connectString = textBox6.Text;
            try
            {
                SqlConnection connection = new SqlConnection(connectString);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    LoadData(textBox6.Text);
                    string caption = "Сообщение";
                    string message = "Подключение к БД установлено";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                string caption = "Сообщение";
                string mes = "Ошибка подключения";
                MessageBox.Show(mes, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}