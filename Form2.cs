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
    public partial class Form2 : Form
    {
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label1.Text = "Онлайн: " + Form1.userName;
            UserRead();
            MsgRead();
            /* Adds the event and the event handler for the method that will 
                process the timer event to the timer. */
            myTimer.Tick += new EventHandler(TimerEventProcessor);
            // Sets the timer interval to 5 seconds.
            myTimer.Interval = 500;
            myTimer.Start();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            string queryLogin = "UPDATE users SET isLogin = false WHERE login = @param4";
            MySqlCommand cmdLogin = new MySqlCommand(queryLogin, Form1.connection);
            cmdLogin.Parameters.AddWithValue("param4", Form1.login);
            cmdLogin.Prepare();
            cmdLogin.ExecuteNonQuery();
            Application.Exit();
        }

        void MsgRead()
        {
            string query = "SELECT id,user,text,time FROM messages ORDER BY time ASC";
            MySqlCommand cmd = new MySqlCommand(query, Form1.connection);
            //cmd.Parameters.AddWithValue("param1", login);
            //cmd.Parameters.AddWithValue("param2", pass);

            MySqlDataReader dataReader = cmd.ExecuteReader();
            textBox1.Clear();
            while (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    textBox1.Text += dataReader.GetString(3) + " " + dataReader.GetString(1) + " " + dataReader.GetString(2) + " " + Environment.NewLine;
                }
                dataReader.NextResult();
            }
            dataReader.Close();
        }
        void UserRead()
        {
            textBox3.Clear();
            string query1 = "SELECT name FROM users WHERE isLogin=true";
            MySqlCommand cmd1 = new MySqlCommand(query1, Form1.connection);
            MySqlDataReader dataReader1 = cmd1.ExecuteReader();

            while (dataReader1.HasRows)
            {
                while (dataReader1.Read())
                {
                    textBox3.Text += dataReader1.GetString(0) + Environment.NewLine;
                }
                dataReader1.NextResult();
            }
            dataReader1.Close();
        }
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            Console.WriteLine("Tick");
            MsgRead();
            UserRead();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() != "")
            {
                string query = "INSERT INTO messages(id,user,text,time) VALUES(null, @param1, @param2, null) ";
                MySqlCommand cmd = new MySqlCommand(query, Form1.connection);
                cmd.Parameters.AddWithValue("param1", Form1.login);
                cmd.Parameters.AddWithValue("param2", textBox2.Text.Trim());
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                textBox2.Clear();
            }
        }
    }
}
