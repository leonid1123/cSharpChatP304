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

namespace chat
{
    public partial class Form1 : Form
    {
        public static MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public static String login = "";
        private String pass = "";
        public static String userName = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server = "192.168.0.23";
            database = "chat";
            uid = "mainUser";
            password = "1234567890";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + "; SSL Mode=None";
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Enabled == true)
            {
                login = textBox1.Text.Trim();
                pass = textBox2.Text.Trim();

                if (login != null && pass != null)
                {
                    string query = "SELECT login, password, isLogin,name FROM users WHERE login =@param1 AND password =@param2 ";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("param1", login);
                    cmd.Parameters.AddWithValue("param2", pass);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    bool canUse = dataReader.HasRows;
                    if (canUse)
                    {
                        dataReader.Read();
                        userName = dataReader.GetString(3);
                        bool isLoggedIn = dataReader.GetBoolean(2);
                        dataReader.Close();
                        if (!isLoggedIn)
                        {
                            string queryLogin = "UPDATE users SET isLogin = true WHERE login = @param4";
                            MySqlCommand cmdLogin = new MySqlCommand(queryLogin, Form1.connection);
                            cmdLogin.Parameters.AddWithValue("param4", login);
                            cmdLogin.Prepare();
                            cmdLogin.ExecuteNonQuery();
                            var myForm2 = new Form2();
                            myForm2.Show();
                            this.Hide();
                        }
                        else
                        {
                            label3.Text = "Такой пользователь уже вошел в систему";
                        }
                    }
                    else
                    {
                        label3.Text = "Неверный логин или пароль";
                    }
                }
            }
            else
            {
                string guestQuery = "INSERT INTO users(id,login,name,password,isLogin) VALUES(null, @param1, null, null,true) ";
                MySqlCommand cmd = new MySqlCommand(guestQuery, Form1.connection);
                cmd.Parameters.AddWithValue("param1", Form1.login);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {//radio1 - зареган, radio2 - гость
            RadioButton rb = (RadioButton)sender;
            if (rb.Name == "radioButton1")
            {
               
                textBox2.Enabled = true;
            }
            else
            {
              
                textBox2.Enabled = false;
            }
        }

    }
}
