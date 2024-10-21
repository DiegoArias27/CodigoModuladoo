using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConexionBD
{
    public partial class login : Form
    {

        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        public login()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string consulta = "select username,password, Nombres from empleados";

                MySqlDataReader mySqlDataReader = null;


                string result = "";
                string result2 = "";
                string result3 = "";
                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);

                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {

                    result = mySqlDataReader.GetString("username");
                    result2 = mySqlDataReader.GetString("password");
                    result3 = mySqlDataReader.GetString("Nombres");

                }
                Program.empleado = result3;
                if(result != "" && result2 != "")
                {
                    if (result == textBox1.Text && result2 == textBox2.Text)
                    {

                        this.Hide();
                        Form2 puntodeventa = new Form2();
                        puntodeventa.Show();
                        

                    }
                    else
                    {
                        MessageBox.Show("Datos incorrectos");
                    }
                }
                else
                {
                    MessageBox.Show("Crea una cuenta");
                }
                
            }
            catch
            {

            }
            finally
            {
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            
            CrearCuenta crear = new CrearCuenta();
            crear.Show();
            
            



        }
    }
}
