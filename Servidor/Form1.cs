using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Protocolo;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace Servidor
{
    public partial class Form1 : Form
    {
        private ArrayList listaClientes;
        private ArrayList listaClientesJugando;
        private Socket socketServidor;
        private byte[] buferRx = new byte[1024];
        private delegate void DelegadoActualizarEstado(string estado);
        private DelegadoActualizarEstado delegadoActualizarEstado = null;
        string saber;
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try {
                listaClientes = new ArrayList();
                listaClientesJugando = new ArrayList();
                delegadoActualizarEstado = new DelegadoActualizarEstado(ActualizarEstado);
                socketServidor = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint servidorExtremo = new IPEndPoint(IPAddress.Any, 30000);
                socketServidor.Bind(servidorExtremo);
                IPEndPoint clienteExtremo = new IPEndPoint(IPAddress.Any, 0);
                EndPoint extremoEP = (EndPoint)clienteExtremo;
                socketServidor.BeginReceiveFrom(buferRx, 0, buferRx.Length, SocketFlags.None, ref extremoEP, new AsyncCallback(ProcesarRecibir), extremoEP);
                //saber = extremoEP.ToString();
                lblEstado.Text = "Escuchando";
            } catch (Exception ex)
            {
                lblEstado.Text = "Error"; MessageBox.Show("Cargando Error: " + ex.Message, "Servidor UDP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //metodos
        private void ProcesarRecibir(IAsyncResult resultadoAsync)
        {
            try
            {
                byte[] data;
                string listado=null;
                Paquete datoRecibido = new Paquete(buferRx);
                Paquete datoParaEnviar = new Paquete();
                IPEndPoint puntoExtremoCliente = new IPEndPoint(IPAddress.Any, 0);
                EndPoint extremoEP = (EndPoint)puntoExtremoCliente;
                socketServidor.EndReceiveFrom(resultadoAsync, ref extremoEP);
                saber = extremoEP.ToString();
                Console.WriteLine(saber);
                datoParaEnviar.IdentificadorChat = datoRecibido.IdentificadorChat;
                datoParaEnviar.NombreChat = datoRecibido.NombreChat;
                switch (datoRecibido.IdentificadorChat)
                {
                    case IdentificadorDato.Mensaje:
                        switch (datoRecibido.IdentificadorL)
                        {
                            case IdentificadorListado.Solicitar:
                                datoParaEnviar.MensajeChat = string.Format("{0}:{1}:{2}", datoRecibido.MensajeChat,extremoEP.ToString(),datoRecibido.NombreChat);
                                Cliente client = new Cliente();
                                String[] substrings = datoRecibido.MensajeChat.Split(',', ':');
                                client.nombre = substrings[0];
                                IPAddress dir = IPAddress.Parse(substrings[1]);
                                IPEndPoint Cli = new IPEndPoint(dir, Convert.ToInt32(substrings[2]));
                                client.puntoExtremo = Cli;
                                data = datoParaEnviar.ObtenerArregloBytes();
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    string nombre = clienteEnLista.nombre;
                                    string direccion = clienteEnLista.puntoExtremo.ToString();
                                    if ((client.nombre == nombre) && (client.puntoExtremo.ToString() == direccion))
                                    {
                                        socketServidor.BeginSendTo(data, 0, data.Length, SocketFlags.None, clienteEnLista.puntoExtremo, new AsyncCallback(ProcesarEnviar), clienteEnLista.puntoExtremo);

                                    }
                                }
                                break;
                            case IdentificadorListado.Aceptar:
                                String[] substrings1 = datoRecibido.MensajeChat.Split(',', ':');
                                Cliente client1 = new Cliente();
                                Cliente client2 = new Cliente();
                                client1.nombre = substrings1[0];
                                IPAddress dir1 = IPAddress.Parse(substrings1[1]);
                                IPEndPoint Cli1 = new IPEndPoint(dir1, Convert.ToInt32(substrings1[2]));
                                client1.puntoExtremo = Cli1;
                                IPAddress dir2 = IPAddress.Parse(substrings1[3]);
                                IPEndPoint Cli2 = new IPEndPoint(dir1, Convert.ToInt32(substrings1[4]));
                                client2.nombre = substrings1[5];
                                client2.puntoExtremo = Cli2;
                                string mensaje1 = client1.nombre+","+client1.puntoExtremo.ToString();
                                string mensaje2 = client2.nombre+ "," + client2.puntoExtremo.ToString();
                                datoParaEnviar.IdentificadorChat = IdentificadorDato.Iniciar;
                                datoParaEnviar.IdentificadorL = IdentificadorListado.Aceptar;
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    string direccion = clienteEnLista.puntoExtremo.ToString();
                                    datoParaEnviar.MensajeChat= mensaje2;
                                    data = datoParaEnviar.ObtenerArregloBytes();
                                    if ((client1.puntoExtremo.ToString() == direccion))
                                    {
                                        socketServidor.BeginSendTo(data, 0, data.Length, SocketFlags.None, clienteEnLista.puntoExtremo, new AsyncCallback(ProcesarEnviar), clienteEnLista.puntoExtremo);

                                    }
                                }
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    string direccion = clienteEnLista.puntoExtremo.ToString();
                                    datoParaEnviar.MensajeChat = mensaje1;
                                    data = datoParaEnviar.ObtenerArregloBytes();
                                    if ((client2.puntoExtremo.ToString() == direccion))
                                    {
                                        socketServidor.BeginSendTo(data, 0, data.Length, SocketFlags.None, clienteEnLista.puntoExtremo, new AsyncCallback(ProcesarEnviar), clienteEnLista.puntoExtremo);

                                    }
                                }
                                jug1 = mensaje1;
                                jug2 = mensaje2;
                                listaClientesJugando.Add(client1);
                                listaClientesJugando.Add(client2);
                                Console.WriteLine(jug1);
                                Console.WriteLine(jug2);
                                foreach (Cliente c in listaClientes)
                                {
                                    if (c.puntoExtremo.ToString()==client1.puntoExtremo.ToString())
                                    {
                                        listaClientes.Remove(c);
                                        break;
                                    }
                                }
                                foreach (Cliente c in listaClientes)
                                {
                                    if (c.puntoExtremo.ToString() == client2.puntoExtremo.ToString())
                                    {
                                        listaClientes.Remove(c);
                                        break;
                                    }
                                }
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    listado = listado + clienteEnLista.nombre + "," + clienteEnLista.puntoExtremo.ToString() + ",";
                                }
                                datoParaEnviar.IdentificadorChat = IdentificadorDato.Listado;
                                datoParaEnviar.IdentificadorL = IdentificadorListado.Actualiza;
                                datoParaEnviar.MensajeChat = listado;
                                data = datoParaEnviar.ObtenerArregloBytes();
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    socketServidor.BeginSendTo(data, 0, data.Length, SocketFlags.None, clienteEnLista.puntoExtremo, new AsyncCallback(ProcesarEnviar), clienteEnLista.puntoExtremo);

                                }
                                break;
                            case IdentificadorListado.Negar:
                                String[] substrings3 = datoRecibido.MensajeChat.Split(',', ':');
                                Cliente client3 = new Cliente();
                               IPAddress dir3 = IPAddress.Parse(substrings3[1]);
                                IPEndPoint Cli3 = new IPEndPoint(dir3, Convert.ToInt32(substrings3[2]));
                                client3.nombre = substrings3[5];
                                client3.puntoExtremo = Cli3;
                                string mensaje3 = datoRecibido.MensajeChat;
                                datoParaEnviar.IdentificadorChat = IdentificadorDato.Iniciar;
                                datoParaEnviar.IdentificadorL = IdentificadorListado.Negar;
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    string direccion = clienteEnLista.puntoExtremo.ToString();
                                    datoParaEnviar.MensajeChat = mensaje3;
                                    data = datoParaEnviar.ObtenerArregloBytes();
                                    if ((client3.puntoExtremo.ToString() == direccion))
                                    {
                                        socketServidor.BeginSendTo(data, 0, data.Length, SocketFlags.None, clienteEnLista.puntoExtremo, new AsyncCallback(ProcesarEnviar), clienteEnLista.puntoExtremo);

                                    }
                                }
                                break;

                        }
                        
                        break;
                    case IdentificadorDato.Listado:
                        switch (datoRecibido.IdentificadorL)
                        {
                            case IdentificadorListado.Tuinfo:
                               
                                break;
                            case IdentificadorListado.Conectado:
                                Cliente nuevoCliente = new Cliente(); nuevoCliente.puntoExtremo = extremoEP; nuevoCliente.nombre = datoRecibido.NombreChat;
                                listaClientes.Add(nuevoCliente);
                                datoParaEnviar.MensajeChat = string.Format("-- {0} está conectado --", datoRecibido.NombreChat);
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    listado = listado+ clienteEnLista.nombre + "," + clienteEnLista.puntoExtremo.ToString() + ",";
                                }
                                    datoParaEnviar.MensajeChat = listado;
                                    data = datoParaEnviar.ObtenerArregloBytes();
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    socketServidor.BeginSendTo(data, 0, data.Length, SocketFlags.None, clienteEnLista.puntoExtremo, new AsyncCallback(ProcesarEnviar), clienteEnLista.puntoExtremo);
                                }
                                break;
                            case IdentificadorListado.Desconectado:
                        
                                foreach (Cliente c in listaClientes)
                                {
                                    if (c.puntoExtremo.Equals(extremoEP))
                                    {
                                        listaClientes.Remove(c); break;
                                    }
                                }
                                datoParaEnviar.MensajeChat = string.Format("-- {0} se ha desconectado -- ", datoRecibido.NombreChat);
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    listado = listado + clienteEnLista.nombre + "," + clienteEnLista.puntoExtremo.ToString() + ",";
                                }
                                datoParaEnviar.MensajeChat = listado;
                                data = datoParaEnviar.ObtenerArregloBytes();
                                foreach (Cliente clienteEnLista in listaClientes)
                                {
                                    socketServidor.BeginSendTo(data, 0, data.Length, SocketFlags.None, clienteEnLista.puntoExtremo, new AsyncCallback(ProcesarEnviar), clienteEnLista.puntoExtremo);

                                }
                                break;
                        }
                        
                        break;
                    case IdentificadorDato.Jugada:
                        Console.WriteLine(datoRecibido.MensajeChat);
                        String[] substrings10 = datoRecibido.MensajeChat.Split(',', ':');
                        Cliente jugador = new Cliente();
                        jugador.nombre = substrings10[1];
                        IPAddress dire = IPAddress.Parse(substrings10[2]);
                        IPEndPoint extr = new IPEndPoint(dire, Convert.ToInt32(substrings10[3]));
                        jugador.puntoExtremo = extr;
                        string enviara = jugador.puntoExtremo.ToString();
                        datoParaEnviar.IdentificadorChat = IdentificadorDato.Jugada;
                        datoParaEnviar.MensajeChat = datoRecibido.MensajeChat;
                        data = datoParaEnviar.ObtenerArregloBytes();
 
                        foreach (Cliente clienteEnLista in listaClientesJugando)
                        {
                            if (clienteEnLista.puntoExtremo.ToString() == enviara)
                            {
                                socketServidor.BeginSendTo(data, 0, data.Length, SocketFlags.None, clienteEnLista.puntoExtremo, new AsyncCallback(ProcesarEnviar), clienteEnLista.puntoExtremo);
                            }
                        }

                        break;
                    case IdentificadorDato.Resultado:
                        Console.WriteLine("Resultaddo");
                        Console.WriteLine(datoRecibido.MensajeChat);
                        String[] substrings11 = datoRecibido.MensajeChat.Split(',', ':');
                        Cliente jugador2 = new Cliente();
                        jugador2.nombre = substrings11[1];
                        IPAddress dire2 = IPAddress.Parse(substrings11[2]);
                        IPEndPoint extr2 = new IPEndPoint(dire2, Convert.ToInt32(substrings11[3]));
                        jugador2.puntoExtremo = extr2;
                        string enviara2 = jugador2.puntoExtremo.ToString();
                        datoParaEnviar.IdentificadorChat = IdentificadorDato.Resultado;
                        datoParaEnviar.MensajeChat = datoRecibido.MensajeChat;
                        data = datoParaEnviar.ObtenerArregloBytes();

                        foreach (Cliente clienteEnLista in listaClientesJugando)
                        {
                            if (clienteEnLista.puntoExtremo.ToString() == enviara2)
                            {
                                socketServidor.BeginSendTo(data, 0, data.Length, SocketFlags.None, clienteEnLista.puntoExtremo, new AsyncCallback(ProcesarEnviar), clienteEnLista.puntoExtremo);
                            }
                        }
                        break;


                }
            
                socketServidor.BeginReceiveFrom(buferRx, 0, buferRx.Length, SocketFlags.None, ref extremoEP, new AsyncCallback(ProcesarRecibir), extremoEP);
                Invoke(delegadoActualizarEstado, new object[] { datoParaEnviar.MensajeChat });
            }
            catch (Exception ex) { MessageBox.Show("Error en la recepción: " + ex.Message, "Servidor UDP", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //11
        public void ProcesarEnviar(IAsyncResult resultadoAsync)
        {
            try
            {
                socketServidor.EndSend(resultadoAsync);
            } catch (Exception ex)
            {
                MessageBox.Show("Error al enviar datos: " + ex.Message, "Servidor UDP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //13
        private void ActualizarEstado(string estado)
        {
            rxtInformacion.Text += estado + Environment.NewLine;
        }

        private void btnTerminar_Click(object sender, EventArgs e)
        {
            socketServidor.Close();
            Close();
        }

        

    }
}
