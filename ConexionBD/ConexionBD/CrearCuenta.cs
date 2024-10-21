using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConexionBD
{
    public partial class CrearCuenta : Form
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        private string direccion = "";
        public CrearCuenta()
        {
            InitializeComponent();
            cargarusuarios();
            

        }

        private void CrearCuenta_Load(object sender, EventArgs e)
        {
            
        }

        private void CrearCuenta_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            login inicio = new login();
            inicio.Show();
            this.Hide();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Imágenes (*.jpg)|*.jpg|Imágenes (*.jpeg)|*.jpeg|Imágenes (*.png)|*.png";
            open.FilterIndex = 1;
            open.RestoreDirectory = false;
            if (open.ShowDialog() == DialogResult.OK)
                pictureBox2.Image = Image.FromFile(open.FileName);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            direccion = open.FileName;

            pictureBox11.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileStream fs;
            BinaryReader br;

            if (direccion == "")
            {
                pictureBox11.Visible = true;
            }
            if(textBox5.Text != textBox6.Text)
            {
                pictureBox10.Visible = true;
                label10.ForeColor = Color.Red;
            }
            if (textBox1.Text == "")
            {
                label3.ForeColor = Color.Red;
                pictureBox4.Visible = true;
            }
            if (textBox2.Text == "")
            {
                label4.ForeColor = Color.Red;
                pictureBox5.Visible = true;
            }
            
            if (textBox3.Text == "")
            {
                label6.ForeColor = Color.Red;
                pictureBox7.Visible = true;
            }
            if (textBox4.Text == "")
            {
                label8.ForeColor = Color.Red;
                pictureBox8.Visible = true;
            }
            if (textBox5.Text == "")
            {
                label9.ForeColor = Color.Red;
                pictureBox9.Visible = true;
            }
            if (textBox6.Text == "")
            {
                label10.ForeColor = Color.Red;
                pictureBox10.Visible = true;
            }
            if (textBox5.Text == textBox6.Text && textBox1.Text!="" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                try
            {
                string FileName = direccion;
                byte[] ImageData;
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                ImageData = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();

                MySqlDataReader mySqlDataReader = null;
                con.Open();


                string consulta = "INSERT INTO empleados(username,password, Nombres, Apellidos, FechaNac, Domicilio, Foto) VALUES(@username,@password, @Nombres, @Apellidos, @FechaNac, @Domicilio, @Foto)";

                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);
                mySqlCommand.Parameters.AddWithValue("@username", textBox4.Text);
                mySqlCommand.Parameters.AddWithValue("@password", textBox5.Text);
                mySqlCommand.Parameters.AddWithValue("@Nombres", textBox1.Text);
                mySqlCommand.Parameters.AddWithValue("@Apellidos", textBox2.Text);
                mySqlCommand.Parameters.AddWithValue("@FechaNac", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                mySqlCommand.Parameters.AddWithValue("@Domicilio", textBox3.Text);
                mySqlCommand.Parameters.AddWithValue("@Foto", ImageData);

                mySqlDataReader = mySqlCommand.ExecuteReader();

                MessageBox.Show("Cuenta creada con éxito");


                    cargarusuarios();
                    vaciar();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            }
            else if(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Llena los campos faltantes");
                
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pictureBox4.Visible = false;
            label3.ForeColor= Color.Black;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            pictureBox5.Visible = false;
            label4.ForeColor = Color.Black;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            pictureBox7.Visible = false;
            label6.ForeColor = Color.Black;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            pictureBox8.Visible = false;
            label8.ForeColor = Color.Black;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            pictureBox9.Visible = false;
            label9.ForeColor = Color.Black;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            pictureBox10.Visible = false;
            label10.ForeColor = Color.Black;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileStream fs;
            BinaryReader br;
            if (textBox5.Text != textBox6.Text)
            {
                pictureBox10.Visible = true;
                label10.ForeColor = Color.Red;
            }
            if (textBox1.Text == "")
            {
                label3.ForeColor = Color.Red;
                pictureBox4.Visible = true;
            }
            if (textBox2.Text == "")
            {
                label4.ForeColor = Color.Red;
                pictureBox5.Visible = true;
            }

            if (textBox3.Text == "")
            {
                label6.ForeColor = Color.Red;
                pictureBox7.Visible = true;
            }
            if (textBox4.Text == "")
            {
                label8.ForeColor = Color.Red;
                pictureBox8.Visible = true;
            }
            
            if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" )
            {
                try
                {
                    string FileName = direccion;
                    byte[] ImageData;
                    fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);
                    ImageData = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();

                    MySqlDataReader mySqlDataReader = null;
                    con.Open();


                    string consulta = "UPDATE empleados SET username = @username, password = @password, Nombres = @Nombres, Apellidos = @Apellidos, FechaNac = @FechaNac, Domicilio = @Domicilio, Foto = @Foto WHERE Nombres ='" + comboBox1.Text + "'";

                    MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);
                    mySqlCommand.Parameters.AddWithValue("@username", textBox4.Text);
                    mySqlCommand.Parameters.AddWithValue("@password", textBox5.Text);
                    mySqlCommand.Parameters.AddWithValue("@Nombres", textBox1.Text);
                    mySqlCommand.Parameters.AddWithValue("@Apellidos", textBox2.Text);
                    mySqlCommand.Parameters.AddWithValue("@FechaNac", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                    mySqlCommand.Parameters.AddWithValue("@Domicilio", textBox3.Text);


                    mySqlCommand.Parameters.AddWithValue("@Foto", ImageData);



                    mySqlDataReader = mySqlCommand.ExecuteReader();

                    MessageBox.Show("Cuenta Actualizada con éxito");





                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            else if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Llena los campos faltantes");


            }
            }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
               

                MySqlDataReader mySqlDataReader = null;
                con.Open();


                string consulta = "delete empleados from empleados WHERE Nombres ='" + comboBox1.Text + "'";
                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);




                mySqlDataReader = mySqlCommand.ExecuteReader();

                MessageBox.Show("Cuenta Eliminada con éxito");

                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
                cargarusuarios();
                vaciar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void vaciar()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            pictureBox2.Image = null;
            comboBox1.Text = "";
        }
        private void cargarusuarios()
        {
            try
            {
                con.Open();
                string consulta = "select Nombres from empleados";

                MySqlDataReader mySqlDataReader = null;


                string result = "";
                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);

                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {

                    result = mySqlDataReader.GetString("Nombres");

                    comboBox1.Items.Add(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void cargardatos()
        {
            try
            {
                con.Open();
                string consulta = "select * from empleados where Nombres= @Nombre";

                MySqlDataReader mySqlDataReader = null;


                string resultuser = "";
                string resultpassword = "";
                string resultnombre = "";
                string resultapellido = "";
                string resultuserfecha = "";
                string resultdomicilio = "";
                string resultfoto = "";
                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);


                mySqlCommand.Parameters.AddWithValue("@Nombre", comboBox1.Text);
                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {

                    resultnombre = mySqlDataReader.GetString("Nombres");

                    textBox1.Text = resultnombre;
                    resultapellido = mySqlDataReader.GetString("Apellidos");

                    textBox2.Text = resultapellido;

                    resultuserfecha = mySqlDataReader.GetString("FechaNac");
                    DateTime fechaDateTime = DateTime.Parse(resultuserfecha);
                    dateTimePicker1.Value = fechaDateTime;

                    resultdomicilio = mySqlDataReader.GetString("Domicilio");

                    textBox3.Text = resultdomicilio;

                    resultuser = mySqlDataReader.GetString("username");

                    textBox4.Text = resultuser;



                    byte[] imageData = (byte[])mySqlDataReader["Foto"]; // Reemplaza 'Foto' con el nombre real de tu columna de imagen

                    // Guardar los bytes de la imagen en un archivo temporal
                    string tempFileName = Path.GetTempFileName();
                    File.WriteAllBytes(tempFileName, imageData);

                    // Obtener la ruta del archivo temporal
                    string imageUrl = tempFileName;
                    direccion = imageUrl;
                    // Cargar la imagen en el PictureBox
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image imagenExtraida = Image.FromStream(ms);
                        pictureBox2.Image = imagenExtraida;

                    }


                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargardatos();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            vaciar();
        }
    }
}
