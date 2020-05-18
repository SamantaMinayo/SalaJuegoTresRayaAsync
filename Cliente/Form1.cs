/*podemos observar que las llamadas asincronas funcionan correcatamente, conectandose varios
 usuarios y enviando mensajes cada vez que ingresan*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using Protocolo;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Cliente
{
    public partial class Form1 : Form
    {
        private Socket socketCliente;
        private ArrayList listaClientes;
        string clientes;
        private string nombre;
        private EndPoint epServidor;
        private byte[] buferRx = new byte[1024];
        private delegate void DelegadoMensajeActualizacion(string mensaje);
        private DelegadoMensajeActualizacion delegadoActualizacion = null;
        private delegate void DelegadoMensajeActualizacion1(string mensaje);
        private DelegadoMensajeActualizacion1 delegadoActualizacion1 = null;
        private string miinformacion;
        private Cliente jugador;
        private string jugadormensaje;
        string jug1;
        string jug2;
        private struct Cliente
        {
            public EndPoint puntoExtremo;
            public string nombre;
            public override string ToString()
            {
                return nombre;
            }
        }
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            listaClientes.Clear();
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listaClientes = new ArrayList();
            lstClientes.Items.Clear();
            delegadoActualizacion = new DelegadoMensajeActualizacion(DesplegarMensaje);
            btnA1.Visible = false;
            btnA2.Visible = false;
            btnA3.Visible = false;
            btnA4.Visible = false;
            btnA5.Visible = false;
            btnA6.Visible = false;
            btnA7.Visible = false;
            btnA8.Visible = false;
            btnA9.Visible = false;

        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {

            try
            {
                
                Cliente Jugadorextremo = (Cliente)lstClientes.SelectedItem;
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Solicitar;
                string jugador = Jugadorextremo.nombre + "," + Jugadorextremo.puntoExtremo.ToString();
                paqueteParaEnviar.MensajeChat = jugador;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Mensaje;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);

            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                nombre = txtNombre.Text.Trim();
                Paquete paqueteInicio = new Paquete();
                paqueteInicio.NombreChat = nombre;
                paqueteInicio.MensajeChat = null;
                paqueteInicio.IdentificadorChat = IdentificadorDato.Listado;
                paqueteInicio.IdentificadorL = IdentificadorListado.Conectado;
                socketCliente = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress servidorIP = IPAddress.Parse(txtServidor.Text.Trim());
                IPEndPoint puntoRemoto = new IPEndPoint(servidorIP, 30000);
                epServidor = (EndPoint)puntoRemoto;
                byte[] buferTx = paqueteInicio.ObtenerArregloBytes();
                socketCliente.BeginSendTo(buferTx, 0, buferTx.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                buferRx = new byte[1024];
                socketCliente.BeginReceiveFrom(buferRx, 0, buferRx.Length, SocketFlags.None, ref epServidor, new AsyncCallback(this.ProcesarRecibir), null);
                btnConectar.Enabled = false;
                txtNombre.Enabled = false;
                txtServidor.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectarse: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //metodos
        private void ProcesarEnviar(IAsyncResult res)
        {
            try
            {
                socketCliente.EndSend(res);
            } catch (Exception ex)
            {
                MessageBox.Show("Enviar Datos: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //23
        private void ProcesarRecibir(IAsyncResult res)
        {
            try
            {
                socketCliente.EndReceive(res);
                Paquete paqueteRecibido = new Paquete(buferRx);
                if (paqueteRecibido.MensajeChat != null)
                {
                    switch (paqueteRecibido.IdentificadorChat)
                    {
                        case IdentificadorDato.Mensaje:
                            clientes = paqueteRecibido.NombreChat;
                            delegadoActualizacion1 = new DelegadoMensajeActualizacion1(DesplegarMensaje1);
                            Invoke(delegadoActualizacion1, new object[] { paqueteRecibido.MensajeChat });

                            break;
                        case IdentificadorDato.Listado:
                            switch (paqueteRecibido.IdentificadorL)
                            {
                                case IdentificadorListado.Tuinfo:
                                    miinformacion = paqueteRecibido.MensajeChat;
                                    Console.WriteLine(miinformacion);
                                    break;
                                case IdentificadorListado.Conectado:
                                    Invoke(delegadoActualizacion, new object[] { paqueteRecibido.MensajeChat });
                                    break;
                                case IdentificadorListado.Desconectado:
                                    Invoke(delegadoActualizacion, new object[] { paqueteRecibido.MensajeChat });
                                    break;
                                case IdentificadorListado.Actualiza:
                                    Invoke(delegadoActualizacion, new object[] { paqueteRecibido.MensajeChat });
                                    break;
                            }
                            Invoke(delegadoActualizacion, new object[] { paqueteRecibido.MensajeChat });
                            break;
                        case IdentificadorDato.Iniciar:
                            switch (paqueteRecibido.IdentificadorL)
                            {
                                case IdentificadorListado.Aceptar:
                                    btnA1.Visible = true;
                                    btnA2.Visible = true;
                                    btnA3.Visible = true;
                                    btnA4.Visible = true;
                                    btnA5.Visible = true;
                                    btnA6.Visible = true;
                                    btnA7.Visible = true;
                                    btnA8.Visible = true;
                                    btnA9.Visible = true;
                                    lstClientes.Visible = false;
                                    btnEnviar.Enabled = false;
                                    btnSalir.Enabled = false;
                                    String[] substrings1 = paqueteRecibido.MensajeChat.Split(',', ':');
                                    String dato = string.Format("Inicio Partida con: {0}", substrings1[0]);
                                    jugador = new Cliente();
                                    jugador.nombre = substrings1[0];
                                    IPAddress jug = IPAddress.Parse(substrings1[1]);
                                    IPEndPoint endp = new IPEndPoint(jug, Convert.ToInt32(substrings1[2]));
                                    jugador.puntoExtremo = endp;
                                    jugadormensaje = paqueteRecibido.MensajeChat;
                                    Console.WriteLine("no yo"+jugadormensaje);
                                    MessageBox.Show(dato, "JUGAR");
                                    
                                    break;
                                case IdentificadorListado.Negar:
                                    btnA1.Visible = false;
                                    btnA2.Visible = false;
                                    btnA3.Visible = false;
                                    btnA4.Visible = false;
                                    btnA5.Visible = false;
                                    btnA6.Visible = false;
                                    btnA7.Visible = false;
                                    btnA8.Visible = false;
                                    btnA9.Visible = false;
                                    lstClientes.Visible = true;
                                    String[] substrings2 = paqueteRecibido.MensajeChat.Split(',', ':');
                                    String dato2 = string.Format("Nego Partida: {0}", substrings2[0]);
                                    MessageBox.Show(dato2);
                                    
                                    break;
                            }

                            break;
                        case IdentificadorDato.Jugada:
                            
                            Console.WriteLine(paqueteRecibido.MensajeChat);
                            String[] substrings21 = paqueteRecibido.MensajeChat.Split(',', ':');
                            switch (substrings21[0])
                            {
                                case "A1":
                                    btnA1.Text = "X";
                                    btnA1.Enabled = false;
                                    Console.WriteLine("A1");
                                    
                                    break;
                                case "A2":
                                    btnA2.Text = "X";
                                    btnA2.Enabled = false;
                                    Console.WriteLine("A2");
                                    break;
                                case "A3":
                                    btnA3.Text = "X";
                                    btnA3.Enabled = false;
                                    Console.WriteLine("A3");
                                    break;
                                case "A4":
                                    btnA4.Text = "X";
                                    btnA4.Enabled = false;
                                    Console.WriteLine("A4");
                                    break;
                                case "A5":
                                    btnA5.Text = "X";
                                    btnA5.Enabled = false;
                                    Console.WriteLine("A5");
                                    break;
                                case "A6":
                                    btnA6.Text = "X";
                                    btnA6.Enabled = false;
                                    Console.WriteLine("A6");
                                    break;
                                case "A7":
                                    btnA7.Text = "X";
                                    Console.WriteLine("A7");
                                    btnA7.Enabled = false;
                                    break;
                                case "A8":
                                    btnA8.Text = "X";
                                    btnA8.Enabled = false;
                                    Console.WriteLine("A8");
                                    break;
                                case "A9":
                                    btnA9.Text = "X";
                                    Console.WriteLine("A9");
                                    btnA9.Enabled = false;
                                    break;
                            }
                            reanudar();
                            break;
                        case IdentificadorDato.Resultado:
                            String[] substrings22 = paqueteRecibido.MensajeChat.Split(',', ':');
                            Console.WriteLine("paquete recibido");

                            MessageBox.Show("RESULTADO: "+substrings22[0]);
                            break;

                    }

                }

                socketCliente.BeginReceiveFrom(buferRx, 0, buferRx.Length, SocketFlags.None, ref epServidor, new AsyncCallback(ProcesarRecibir), null);
                //Invoke(delegadoActualizacion, new object[] { paqueteRecibido.MensajeChat });
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex) { MessageBox.Show("Datos Recibidos: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        //24
        private void DesplegarMensaje1(string mensaje)
        {
            rxtMensajes.Text = mensaje;

            if (MessageBox.Show(clientes + " Quiere iniciar partida", "INICIAR PARTIDA", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                Console.WriteLine("aqui");
                String[] substrings = mensaje.Split(',', ':');
                jug1 = substrings[2];
                jug2 = substrings[4];
                Console.WriteLine(jug2+jug1);

                btnA1.Visible = true;
                btnA2.Visible = true;
                btnA3.Visible = true;
                btnA4.Visible = true;
                btnA5.Visible = true;
                btnA6.Visible = true;
                btnA7.Visible = true;
                btnA8.Visible = true;
                btnA9.Visible = true;
                lstClientes.Visible = false;
                nombre = txtNombre.Text;
                Paquete paqueteSolicitar = new Paquete();
                paqueteSolicitar.NombreChat = nombre;
                paqueteSolicitar.MensajeChat = mensaje;
                Console.WriteLine(mensaje);
                paqueteSolicitar.IdentificadorChat = IdentificadorDato.Mensaje;
                paqueteSolicitar.IdentificadorL = IdentificadorListado.Aceptar;
                byte[] buferTx = paqueteSolicitar.ObtenerArregloBytes();
                socketCliente.BeginSendTo(buferTx, 0, buferTx.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);

            }
            else
            {
               
                nombre = txtNombre.Text;
                Paquete paqueteSolicitar = new Paquete();
                paqueteSolicitar.NombreChat = nombre;
                paqueteSolicitar.MensajeChat = mensaje;
                paqueteSolicitar.IdentificadorChat = IdentificadorDato.Mensaje;
                paqueteSolicitar.IdentificadorL = IdentificadorListado.Negar;
                byte[] buferTx = paqueteSolicitar.ObtenerArregloBytes();
                socketCliente.BeginSendTo(buferTx, 0, buferTx.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);

            }

        }
        private void DesplegarMensaje(string mensaje)
        {

            rxtMensajes.Text = mensaje;
            listaClientes.Clear();

            String[] substrings = mensaje.Split(',', ':');

            if (listaClientes != null)
            {

                for (int i = 0; i < (substrings.Count() - 1); i = i + 3)
                {
                    Cliente nuevo = new Cliente();
                    nuevo.nombre = substrings[i];
                    IPAddress dir = IPAddress.Parse(substrings[i + 1]);
                    IPEndPoint Cli = new IPEndPoint(dir, Convert.ToInt32(substrings[i + 2]));
                    nuevo.puntoExtremo = Cli;
                    listaClientes.Add(nuevo);
                }

            }
            lstClientes.Items.Clear();
            foreach (Cliente item in listaClientes)
            {

                    lstClientes.Items.Add(item);
            }

        }

        private void resultado(string A1, string A2, string A3, string A4, string A5, string A6, string A7, string A8, string A9)
        {
            Console.WriteLine("Evaluar");
            Console.WriteLine(jug1);
            Console.WriteLine(jug2);
            Console.WriteLine("clientes");
            string variable = null;
            if (btnA1.Text!=""&& btnA2.Text != "" && btnA3.Text != "" && btnA4.Text != "" && btnA5.Text != "" && btnA6.Text != "" && btnA7.Text != "" && btnA8.Text != "" && btnA9.Text != "")
            {
                Console.WriteLine( "finalizo");
                if ((btnA1.Text=="O")&&((A1==A2&&A1==A3)|| (A1 == A4 && A1 == A7)|| (A1 == A5 && A1 == A9)))
                {
                    Console.WriteLine(jug1 + "gano");
                    variable ="gano";
                }
                else if ((btnA2.Text == "O") && ((A2 == A5 && A2 == A8)))
                {
                    Console.WriteLine(jug1 + "gano");
                    variable = "gano";

                }
                else if ((btnA3.Text == "O") && ((A3 == A6 && A3 == A9) || (A3 == A5 && A3 == A7)))
                {
                    Console.WriteLine(jug1 + "gano");
                    variable = "gano";
                }
                else if ((btnA4.Text == "O") && ((A4 == A5 && A4 == A6)))
                {
                    Console.WriteLine(jug1 + "gano");
                    variable = "gano";
                }
                else if ((btnA7.Text == "O") && ((A7 == A8 && A7 == A9)))
                {
                    Console.WriteLine(jug1+"gano");
                    variable = "gano";
                }
                else if ((btnA1.Text == "X") && ((A1 == A2 && A1 == A3) || (A1 == A4 && A1 == A7) || (A1 == A5 && A1 == A9)))
                {
                    Console.WriteLine(jug1 + "perdio");
                    variable = "perdio";
                }
                else if ((btnA2.Text == "X") && ((A2 == A5 && A2 == A8)))
                {
                    Console.WriteLine(jug1 + "perdio");
                    variable = "perdio";

                }
                else if ((btnA3.Text == "X") && ((A3 == A6 && A3 == A9) || (A3 == A5 && A3 == A7)))
                {
                    Console.WriteLine(jug1 + "perdio");
                    variable = "perdio";
                }
                else if ((btnA4.Text == "X") && ((A4 == A5 && A4 == A6)))
                {
                    Console.WriteLine(jug1 + "perdio");
                    variable = "perdio";
                }
                else if ((btnA7.Text == "X") && ((A7 == A8 && A7 == A9)))
                {
                    Console.WriteLine(jug1 + "perdio");
                    variable = "perdio";
                }else
                {
                    variable = "empate";
                }
                switch (variable)
                {
                    case "gano":

                        nombre = txtNombre.Text;
                        Paquete paqueteRespuesta = new Paquete();
                        paqueteRespuesta.NombreChat = nombre;
                        paqueteRespuesta.MensajeChat = "PERDIO,"+jugadormensaje;
                        paqueteRespuesta.IdentificadorChat = IdentificadorDato.Resultado;
                        byte[] buferTx = paqueteRespuesta.ObtenerArregloBytes();
                        socketCliente.BeginSendTo(buferTx, 0, buferTx.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                        MessageBox.Show("Usted GANO");
                        break;
                    case "perdio":
                        nombre = txtNombre.Text;
                        Paquete paqueteRespuesta2 = new Paquete();
                        paqueteRespuesta2.NombreChat = nombre;
                        paqueteRespuesta2.MensajeChat = "GANO," +jugadormensaje;
                        paqueteRespuesta2.IdentificadorChat = IdentificadorDato.Resultado;
                        byte[] buferTx2 = paqueteRespuesta2.ObtenerArregloBytes();
                        socketCliente.BeginSendTo(buferTx2, 0, buferTx2.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                        MessageBox.Show("Usted PERDIO");
                        break;
                    case "empate":
                        nombre = txtNombre.Text;
                        Paquete paqueteRespuesta1 = new Paquete();
                        paqueteRespuesta1.NombreChat = nombre;
                        paqueteRespuesta1.MensajeChat = "EMPATE,"+jugadormensaje;
                        paqueteRespuesta1.IdentificadorChat = IdentificadorDato.Resultado;
                        byte[] buferTx1 = paqueteRespuesta1.ObtenerArregloBytes();
                        socketCliente.BeginSendTo(buferTx1, 0, buferTx1.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                        MessageBox.Show("EMPATE");
                        break;
                }
            }        
            else{
                Console.WriteLine("la partida continua");
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            listaClientes.Clear();
            try
            {
                if (this.socketCliente != null)
                {
                    Paquete paqueteSalida = new Paquete(); paqueteSalida.IdentificadorChat = IdentificadorDato.Listado; paqueteSalida.IdentificadorL = IdentificadorListado.Desconectado; paqueteSalida.NombreChat = nombre; paqueteSalida.MensajeChat = null;
                    byte[] buferTx = paqueteSalida.ObtenerArregloBytes();
                    socketCliente.SendTo(buferTx, 0, buferTx.Length, SocketFlags.None, epServidor);
                    socketCliente.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error al desconectar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        
        private void btnA1_Click(object sender, EventArgs e)
        {
            
            try
            {
                
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.MensajeChat = "A1," + jugadormensaje;
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Null;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                btnA1.Text = "O";
                btnA1.Enabled = false;
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                resultado(btnA1.Text, btnA2.Text, btnA3.Text, btnA4.Text, btnA5.Text, btnA6.Text, btnA7.Text, btnA8.Text, btnA9.Text);
                pausar();
            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnA2_Click(object sender, EventArgs e)
        {
            try
            {
             
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.MensajeChat = "A2," + jugadormensaje;
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Null;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                    btnA2.Text = "O";
                btnA2.Enabled = false;
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                resultado(btnA1.Text, btnA2.Text, btnA3.Text, btnA4.Text, btnA5.Text, btnA6.Text, btnA7.Text, btnA8.Text, btnA9.Text);
                pausar();
            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnA3_Click(object sender, EventArgs e)
        {
            try
            {
                
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.MensajeChat = "A3," + jugadormensaje;
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Null;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                    btnA3.Text = "O";
                btnA3.Enabled = false;
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                resultado(btnA1.Text, btnA2.Text, btnA3.Text, btnA4.Text, btnA5.Text, btnA6.Text, btnA7.Text, btnA8.Text, btnA9.Text);
                pausar();
            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnA4_Click(object sender, EventArgs e)
        {
            try
            {
                
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.MensajeChat = "A4," + jugadormensaje;
               
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Null;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                    btnA4.Text = "O";
                btnA4.Enabled = false;
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                resultado(btnA1.Text, btnA2.Text, btnA3.Text, btnA4.Text, btnA5.Text, btnA6.Text, btnA7.Text, btnA8.Text, btnA9.Text);
                pausar();
            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnA5_Click(object sender, EventArgs e)
        {
            try
            {
           
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.MensajeChat = "A5," + jugadormensaje;
                
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Null;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                btnA5.Text = "O";
                btnA5.Enabled = false;
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                resultado(btnA1.Text, btnA2.Text, btnA3.Text, btnA4.Text, btnA5.Text, btnA6.Text, btnA7.Text, btnA8.Text, btnA9.Text);
                pausar();
            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnA6_Click(object sender, EventArgs e)
        {
            try
            {
                
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.MensajeChat = "A6," + jugadormensaje;
                
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Null;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                    btnA6.Text = "O";
                btnA6.Enabled = false;
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                resultado(btnA1.Text, btnA2.Text, btnA3.Text, btnA4.Text, btnA5.Text, btnA6.Text, btnA7.Text, btnA8.Text, btnA9.Text);
                pausar();
            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnA7_Click(object sender, EventArgs e)
        {
            try
            {
              
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.MensajeChat = "A7," + jugadormensaje;
                
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Null;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                    btnA7.Text = "O";
                btnA7.Enabled = false;
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                resultado(btnA1.Text, btnA2.Text, btnA3.Text, btnA4.Text, btnA5.Text, btnA6.Text, btnA7.Text, btnA8.Text, btnA9.Text);
                pausar();
            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnA8_Click(object sender, EventArgs e)
        {
            try
            {
                
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.MensajeChat = "A8," + jugadormensaje;
                
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Null;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                    btnA8.Text = "O";
                btnA8.Enabled = false;
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                resultado(btnA1.Text, btnA2.Text, btnA3.Text, btnA4.Text, btnA5.Text, btnA6.Text, btnA7.Text, btnA8.Text, btnA9.Text);
                pausar();
            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnA9_Click(object sender, EventArgs e)
        {
          
            try
            {
               
                Paquete paqueteParaEnviar = new Paquete();
                paqueteParaEnviar.MensajeChat = "A9," + jugadormensaje;
                paqueteParaEnviar.NombreChat = nombre;
                paqueteParaEnviar.IdentificadorL = IdentificadorListado.Null;
                paqueteParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                byte[] arregloBytes = paqueteParaEnviar.ObtenerArregloBytes();
                btnA9.Text = "O";
                btnA9.Enabled = false;
                socketCliente.BeginSendTo(arregloBytes, 0, arregloBytes.Length, SocketFlags.None, epServidor, new AsyncCallback(ProcesarEnviar), null);
                resultado(btnA1.Text, btnA2.Text, btnA3.Text, btnA4.Text, btnA5.Text, btnA6.Text, btnA7.Text, btnA8.Text, btnA9.Text);
                pausar();
            }
            catch (Exception ex) { MessageBox.Show("Error al enviar: " + ex.Message, "Cliente UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void pausar()
        {
            btnA1.Enabled = false;
            btnA2.Enabled = false;
            btnA3.Enabled = false;
            btnA4.Enabled = false;
            btnA5.Enabled = false;
            btnA6.Enabled = false;
            btnA7.Enabled = false;
            btnA8.Enabled = false;
            btnA9.Enabled = false;
        }
        private void reanudar() {
            if (btnA1.Text == "")
            {
                btnA1.Enabled = true;
            }if(btnA2.Text == "")
            {
                btnA2.Enabled = true;
            }if (btnA3.Text == "")
            {
                btnA3.Enabled = true;
            }if (btnA4.Text == "")
            {
                btnA4.Enabled = true;
            }if (btnA5.Text == "")
            {
                btnA5.Enabled = true;
            }if (btnA6.Text == "")
            {
                btnA6.Enabled = true;
            }if (btnA7.Text == "")
            {
                btnA7.Enabled = true;
            }if (btnA8.Text == "")
            {
                btnA8.Enabled = true;
            }if (btnA9.Text == "")
            {
                btnA9.Enabled = true;
            }
        }
    }
}