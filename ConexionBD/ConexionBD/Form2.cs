using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConexionBD
{
    public partial class Form2 : Form
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        
        MySqlDataAdapter adapt;

        private bool f = false;
        private bool t = false;
        private int punto = 0;
        private bool productoExistente = false;
        private bool digitEntered = false;
        private double total = 0, pago=0,cambio=0;
        public Form2()
        {
            InitializeComponent();
            CargarPro();
            CargarNombre();
            cargarIdPedido();
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            timer1.Start();

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            t = true;
            f = false;
            if (t == true)
            {
                //button1.BackColor = Color.LightGreen;
                //button2.BackColor= Color.FromArgb(22, 158, 208);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f = true;
            t = false;
            if (f == true)
            {
                //button2.BackColor = Color.Brown;
                //button1.BackColor= Color.FromArgb(22, 158, 208);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs pE)
        {
            if (char.IsDigit(pE.KeyChar))
            {
                pE.Handled = false;
                digitEntered = true;

            }
            else if (char.IsControl(pE.KeyChar))
            {
                pE.Handled = false;
                

            }
            else if (pE.KeyChar == '.' && digitEntered==true && !((TextBox)sender).Text.Contains('.'))
            {
                // Permitir el punto solo si ya se ingresó un dígito y no hay otro punto presente en el texto
                pE.Handled = false;
                digitEntered = false; // Reiniciar el seguimiento después de ingresar el punto
            }
            else
            {
                pE.Handled = true;
                
            }

            if (pE.KeyChar == (char)Keys.Enter)
            {
                pedido();
            }
            }

        private void pedido()
        {
            Boolean existe = false;

            
            try
            {


                timer1.Stop();
                pago = Convert.ToDouble(textBox3.Text);
                total = Convert.ToDouble(labeltotal.Text);
                if (pago > total)
                {
                    cambio = pago - total;
                    labelcambio.Text = "" + cambio;

                    clsFunciones.CreaTicket Ticket1 = new clsFunciones.CreaTicket();

                    Ticket1.TextoCentro("EL REY DE LOS OCEANOS                   "); //imprime una linea de descripcion
                    Ticket1.TextoCentro("****************************************");

                    Ticket1.TextoCentro("Dirección: Colinas del rio");
                    Ticket1.TextoCentro("Rio Arno #221, C.P. 20010");
                    Ticket1.TextoCentro("Tel: 4492324748");
                    Ticket1.TextoIzquierda("");
                    Ticket1.TextoCentro("Ticket de Venta"); //imprime una linea de descripcion

                    Ticket1.TextoIzquierda("Folio: " + labelpedido.Text);
                    Ticket1.TextoIzquierda("Fecha:" + DateTime.Now.ToShortDateString() + " Hora:" + DateTime.Now.ToShortTimeString());
                    Ticket1.TextoIzquierda("Le Atendio: " + labelnombreemp.Text);
                    Ticket1.TextoIzquierda("");
                    clsFunciones.CreaTicket.LineasGuion();//-------------------------

                    clsFunciones.CreaTicket.EncabezadoVenta();
                    clsFunciones.CreaTicket.LineasGuion();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Articulo                     //Precio                                    cantidad                            Subtotal
                        Ticket1.AgregaArticulo(row.Cells["Producto"].Value.ToString(), Convert.ToDouble(row.Cells["Precio"].Value), Convert.ToInt32(row.Cells["Cantidad"].Value), Convert.ToDouble(row.Cells["SubTotal"].Value)); //imprime una linea de descripcion
                    }
                    clsFunciones.CreaTicket.LineasGuion();
                    Ticket1.AgregaTotales("Sub-Total:", Convert.ToDouble(labeltotal.Text)); // imprime linea con Subtotal
                    Ticket1.TextoIzquierda(" ");
                    Ticket1.AgregaTotales("Total:", Convert.ToDouble(labeltotal.Text)); // imprime linea con total
                    Ticket1.TextoIzquierda(" ");

                    Ticket1.AgregaTotales("Efectivo Entregado:", Convert.ToDouble(textBox3.Text));
                    Ticket1.AgregaTotales("Efectivo Devuelto:", Convert.ToDouble(labelcambio.Text));


                    // Ticket1.LineasTotales(); // imprime linea 

                    Ticket1.TextoIzquierda(" ");
                    Ticket1.TextoCentro("****************************************");
                    Ticket1.TextoCentro("*        ¡Gracias por tu visita!       *");
                    Ticket1.TextoCentro("****************************************");
                    Ticket1.TextoIzquierda(" ");

                    PrintDialog print = new PrintDialog(); // Debes tener algo similar a esto en tu código
                    PrintDocument printDocument1 = new PrintDocument(); // Define la variable printDocument1 como un nuevo PrintDocument


                    // Resto de tu código

                    print.Document = printDocument1;
                    if (print.ShowDialog() == DialogResult.OK)
                    {
                        //printDocument1.Print();
                    }

                    Ticket1.ImprimirTiket(print.PrinterSettings.PrinterName); //Imprimir

                    
                }
                else
                {
                    string titl = "Insuficiente";
                    MessageBoxButtons button = MessageBoxButtons.OK;
                    MessageBox.Show("Dinero insuficiente", titl, button, MessageBoxIcon.Error);
                }

                try
                {
                    MySqlDataReader mySqlDataReader = null;
                    con.Open();
                    string consulta2 = "SELECT IdPedido FROM pedido where IdPedido="+labelpedido.Text+"";
                    string result = "";
                    MySqlCommand mySqlCommand2 = new MySqlCommand(consulta2, con);

                    mySqlDataReader = mySqlCommand2.ExecuteReader();
                    while (mySqlDataReader.Read())
                    {

                        result = mySqlDataReader.GetString("IdPedido");
                        MessageBox.Show(result);
                        
                    }
                    if (result == "")
                    {
                        existe = false;

                    }
                    else
                    {
                        existe = true;
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }

                if (existe == false)
                {
                    agregarPagado();

                    
                }
                else if(existe==true)
                {
                    updatestatus();
                }

                labeltotal.Text = "0.00";
                nuevopedido();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void agregarPagado()
        {
            try
            {

                timer1.Stop();
                MySqlDataReader mySqlDataReader = null;

                con.Open();


                string consulta = "INSERT INTO pedido(Nombre,Fecha, Hora, Total, Status) VALUES(@Nombre,@Fecha, @Hora, @Total, @Status)";

                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);
                mySqlCommand.Parameters.AddWithValue("@Nombre", comboBox2.Text);
                mySqlCommand.Parameters.AddWithValue("@Fecha", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                mySqlCommand.Parameters.AddWithValue("@Hora", dateTimePicker2.Value.ToString("HH:mm"));
                mySqlCommand.Parameters.AddWithValue("@Total", labeltotal.Text);
                mySqlCommand.Parameters.AddWithValue("@Status", 1);


                mySqlDataReader = mySqlCommand.ExecuteReader();
                mySqlDataReader.Close();

                MySqlDataReader mySqlDataReader2 = null;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {


                    string result = "";
                    string consulta3 = "SELECT IdProducto from productos where Nombre='" + row.Cells["Producto"].Value + "'";
                    MySqlCommand mySqlCommand3 = new MySqlCommand(consulta3, con);
                    mySqlDataReader2 = mySqlCommand3.ExecuteReader();

                    while (mySqlDataReader2.Read())
                    {

                        result = mySqlDataReader2.GetString("IdProducto");

                    }
                    mySqlDataReader2.Close();

                    MySqlDataReader mySqlDataReader3 = null;

                    string consulta2 = "INSERT INTO ticket(IdPedido,IdProducto, Descripcion, Cantidad,Subtotal) VALUES(@IdPedido,@IdProducto, @Descripcion, @Cantidad,@Subtotal)";


                    MySqlCommand mySqlCommand2 = new MySqlCommand(consulta2, con);
                    mySqlCommand2.Parameters.AddWithValue("@IdPedido", labelpedido.Text);
                    mySqlCommand2.Parameters.AddWithValue("@IdProducto", Convert.ToInt32(result));
                    mySqlCommand2.Parameters.AddWithValue("@Descripcion", textBox2.Text);
                    mySqlCommand2.Parameters.AddWithValue("@Cantidad", row.Cells["Cantidad"].Value);
                    mySqlCommand2.Parameters.AddWithValue("@Subtotal", row.Cells["SubTotal"].Value);


                    mySqlDataReader3 = mySqlCommand2.ExecuteReader();


                    mySqlDataReader3.Close();
                }

                nuevopedido();
                dataGridView1.Rows.Clear();
                cargarIdPedido();
                CargarNombre();

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
        private void updatestatus()
        {
            try
            {


                MySqlDataReader mySqlDataReader = null;
                con.Open();

                
                string consulta = "UPDATE pedido set Status=1 where IdPedido=" + labelpedido.Text + "";

                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);



                mySqlDataReader = mySqlCommand.ExecuteReader();

                
                timer1.Start();
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
        private void nuevopedido()
        {
            cargarIdPedido();
            timer1.Start();
            dataGridView1.Rows.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            textBox2.Text = "";
            numericUpDown1.Value = 1;
            labeltotal.Text = "0.00";
            labelcambio.Text = "0.00";
            textBox3.Text = "";
        }
        private void CargarPro()
        {
            con.Open();
            string consulta = "select Nombre from Productos";

            MySqlDataReader mySqlDataReader = null;

            
                string result = "";
                MySqlCommand mySqlCommand = new MySqlCommand(consulta,con);
                
                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {

                    result = mySqlDataReader.GetString("Nombre");

                comboBox1.Items.Add(result);

                }
            con.Close();
            }

        private void CargarNombre()
        {
            labelnombreemp.Text = Program.empleado;
            try
            {
                con.Open();
                string consulta = "select Nombre from Pedido where Status=1";

                MySqlDataReader mySqlDataReader = null;


                string result = "";
                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);

                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {

                    result = mySqlDataReader.GetString("Nombre");

                    comboBox2.Items.Add(result);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Minimum = 1;
            try
            {
                con.Open();
                string consulta = "select Stock from Productos where Nombre='"+comboBox1.Text+"'";

                MySqlDataReader mySqlDataReader = null;


                string result = "";
                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);

                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {

                    result = mySqlDataReader.GetString("Stock");

                    numericUpDown1.Maximum = Convert.ToInt32(result);

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

        private void pictureBox2_Click(object sender, EventArgs e) 
        {
            
            try
            {
                
                con.Open();
                string consulta = "select * from Productos where Nombre='" + comboBox1.Text + "'";

                MySqlDataReader mySqlDataReader = null;


                string result = "";
                string resultP = "";
                string resultS = "";
                double subtotal = 0;
                int cantidad = Convert.ToInt32(numericUpDown1.Value);
                
                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);
                productoExistente = false;
                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {

                    result = mySqlDataReader.GetString("Nombre");
                    resultP = mySqlDataReader.GetString("Precio");
                    resultS = mySqlDataReader.GetString("Stock");

                    subtotal = Convert.ToInt32(resultP) * cantidad;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["Producto"].Value.ToString() == comboBox1.Text)
                        {
                            // El producto ya existe, actualizar la cantidad y el total
                            int cantidadExistente = Convert.ToInt32(row.Cells["Cantidad"].Value);
                            cantidadExistente += cantidad;
                            row.Cells["Cantidad"].Value= cantidadExistente;

                            double totalExistente = Convert.ToDouble(row.Cells["SubTotal"].Value);
                            totalExistente += subtotal;
                            row.Cells["SubTotal"].Value = totalExistente;

                            productoExistente = true;
                            break;
                        }
                    }

                    if (productoExistente==false)
                    {
                        dataGridView1.Rows.Add(result, resultP, cantidad, subtotal);
                        

                    }
                }

                double sumaTotal = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    sumaTotal += Convert.ToDouble(row.Cells["SubTotal"].Value);
                }

                labeltotal.Text = ""+sumaTotal;


            }
            catch
            {

            }
            finally
            {
                con.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            agregar();
            
        }

        private void agregar()
        {
            try
            {

                timer1.Stop();
                MySqlDataReader mySqlDataReader = null;

                con.Open();


                string consulta = "INSERT INTO pedido(Nombre,Fecha, Hora, Total, Status) VALUES(@Nombre,@Fecha, @Hora, @Total, @Status)";

                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);
                mySqlCommand.Parameters.AddWithValue("@Nombre", comboBox2.Text);
                mySqlCommand.Parameters.AddWithValue("@Fecha", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                mySqlCommand.Parameters.AddWithValue("@Hora", dateTimePicker2.Value.ToString("HH:mm"));
                mySqlCommand.Parameters.AddWithValue("@Total", labeltotal.Text);
                mySqlCommand.Parameters.AddWithValue("@Status", 0);


                mySqlDataReader = mySqlCommand.ExecuteReader();
                mySqlDataReader.Close();

                MySqlDataReader mySqlDataReader2 = null;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {


                    string result = "";
                    string consulta3 = "SELECT IdProducto from productos where Nombre='" + row.Cells["Producto"].Value + "'";
                    MySqlCommand mySqlCommand3 = new MySqlCommand(consulta3, con);
                    mySqlDataReader2 = mySqlCommand3.ExecuteReader();

                    while (mySqlDataReader2.Read())
                    {

                        result = mySqlDataReader2.GetString("IdProducto");

                    }
                    mySqlDataReader2.Close();

                    MySqlDataReader mySqlDataReader3 = null;

                    string consulta2 = "INSERT INTO ticket(IdPedido,IdProducto, Descripcion, Cantidad,Subtotal) VALUES(@IdPedido,@IdProducto, @Descripcion, @Cantidad,@Subtotal)";


                    MySqlCommand mySqlCommand2 = new MySqlCommand(consulta2, con);
                    mySqlCommand2.Parameters.AddWithValue("@IdPedido", labelpedido.Text);
                    mySqlCommand2.Parameters.AddWithValue("@IdProducto", Convert.ToInt32(result));
                    mySqlCommand2.Parameters.AddWithValue("@Descripcion", textBox2.Text);
                    mySqlCommand2.Parameters.AddWithValue("@Cantidad", row.Cells["Cantidad"].Value);
                    mySqlCommand2.Parameters.AddWithValue("@Subtotal", row.Cells["SubTotal"].Value);


                    mySqlDataReader3 = mySqlCommand2.ExecuteReader();


                    mySqlDataReader3.Close();
                }


                nuevopedido();
                dataGridView1.Rows.Clear();
                cargarIdPedido();
                CargarNombre();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                MySqlDataReader mySqlDataReader = null;
                con.Open();
                string resultNom = "";
                string resultCant = "";
                string resultDesc = "";
                

                string consulta = "SELECT p.Nombre,t.Descripcion, t.Cantidad FROM ticket t, productos p where p.Nombre= '" + dataGridView1.SelectedCells[0].Value + "' and t.IdProducto= p.IdProducto and t.IdPedido=" +labelpedido.Text+"";
                

                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);

                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    resultNom = mySqlDataReader.GetString("Nombre");

                    comboBox1.SelectedItem= resultNom;

                    resultDesc = mySqlDataReader.GetString("Descripcion");

                    textBox2.Text = resultDesc;
                    resultCant = mySqlDataReader.GetString("Cantidad");
                    

                    numericUpDown1.Value= Convert.ToInt32(resultCant);

                }

                mySqlDataReader.Close();
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                con.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            dataGridView1.Rows.Clear();
            CargarDatos();
           
            
        }

        private void cargarIdPedido()
        {
            try
            {
                MySqlDataReader mySqlDataReader = null;
                con.Open();
                string resultIdPedido = "";
                string consulta = "SELECT * FROM pedido  ORDER BY IdPedido DESC LIMIT 1";

                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);
                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    resultIdPedido = mySqlDataReader.GetString("IdPedido");
                    int resf;
                    resf = Convert.ToInt32(resultIdPedido) + 1;
                    labelpedido.Text = "" + resf;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            
        }
        private void CargarDatos()
        {
            try
            {
                MySqlDataReader mySqlDataReader = null;
                con.Open();
                string resultIdPedido = "";
                string resultFecha = "";
                string resultHora = "";
                string resultProd = "";
                string resultCant = "";
                string resultDesc = "";
                string resultPrecio = "";
                string resultSub = "";
                string resulttotal = "";


                string consulta = "SELECT p.IdPedido, p.Fecha, p.Hora, p.Total, pr.Nombre, pr.Precio, t.Descripcion, t.Cantidad, t.Subtotal FROM pedido p, ticket t, productos pr where p.Nombre='"+comboBox2.Text+"' and t.IdPedido=p.IdPedido and t.IdProducto= pr.IdProducto";
                

                MySqlCommand mySqlCommand = new MySqlCommand(consulta, con);

                mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    resultIdPedido = mySqlDataReader.GetString("IdPedido");

                    labelpedido.Text = resultIdPedido;

                    resultFecha = mySqlDataReader.GetString("Fecha");
                    DateTime fechaDateTime = DateTime.Parse(resultFecha);
                    dateTimePicker1.Value =  fechaDateTime;

                    resultHora = mySqlDataReader.GetString("Hora");
                    DateTime fechaDateTime2 = DateTime.Parse(resultHora);
                    dateTimePicker2.Value = fechaDateTime2;

                    resulttotal = mySqlDataReader.GetString("Total");
                    labeltotal.Text = resulttotal;

                    resultDesc = mySqlDataReader.GetString("Descripcion");
                    textBox2.Text = resultDesc;

                    resultProd = mySqlDataReader.GetString("Nombre");
                    resultPrecio = mySqlDataReader.GetString("Precio");
                    resultCant = mySqlDataReader.GetString("Cantidad");
                    resultSub = mySqlDataReader.GetString("Subtotal");

                    dataGridView1.Rows.Add(resultProd, resultPrecio, resultCant, resultSub);
                }
                mySqlDataReader.Close();
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

        private void button4_Click(object sender, EventArgs e)
        {
            nuevopedido();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void labelnombreemp_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            numericUpDown1.Value = 1;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                // Elimina la fila en el índice obtenido
                dataGridView1.Rows.RemoveAt(rowIndex);
            }
            else
            {
                MessageBox.Show("Selecciona una fila para eliminar.");
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
