using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocolo
{
        // -------------------------- // Estructura del Paquete // --------------------------
        // Descripcion -> |idDato|longitud|long del mensaje| nombre | mensaje | // Tama;o en bytes -> | 4 | 4 | 4 |
        public enum IdentificadorDato { Mensaje, Listado,Iniciar,Jugada,Resultado, Null }
        public enum IdentificadorListado { Conectado,Desconectado,Aceptar,Negar,Solicitar,Tuinfo,Actualiza, Null }
    public class Paquete
        {
            private IdentificadorDato idDato; private IdentificadorListado identi; private string nombre; private string mensaje;
            public IdentificadorDato IdentificadorChat { get { return idDato; } set { idDato = value; } }
        //para identificar si es conectado o desconectado
            public IdentificadorListado IdentificadorL { get { return identi; } set { identi = value; } }
            public string NombreChat { get { return nombre; } set { nombre = value; } }
            public string MensajeChat { get { return mensaje; } set { mensaje = value; } }
            public Paquete()
        {
            this.idDato = IdentificadorDato.Null;
            this.identi = IdentificadorListado.Null;
            this.mensaje = null;
            this.nombre = null;
        }
            public Paquete(byte[] arregloBytes)
        {
            this.idDato = (IdentificadorDato)BitConverter.ToInt32(arregloBytes, 0);
            this.identi = (IdentificadorListado)BitConverter.ToInt32(arregloBytes, 4);
            int longitudNombre = BitConverter.ToInt32(arregloBytes, 8);
            int longitudMensaje = BitConverter.ToInt32(arregloBytes, 12);
            if (longitudNombre > 0) this.nombre = Encoding.UTF8.GetString(arregloBytes, 16, longitudNombre);
            else this.nombre = null;
            if (longitudMensaje > 0) this.mensaje = Encoding.UTF8.GetString(arregloBytes, 16 + longitudNombre, longitudMensaje);
            else this.mensaje = null;
        }
            public byte[] ObtenerArregloBytes()
            {
                List<Byte> arregloBytes = new List<Byte>();
            arregloBytes.AddRange(BitConverter.GetBytes((int)this.idDato));
            arregloBytes.AddRange(BitConverter.GetBytes((int)this.identi));
            if (this.nombre != null) arregloBytes.AddRange(BitConverter.GetBytes(this.nombre.Length));
            else arregloBytes.AddRange(BitConverter.GetBytes(0));
            if (this.mensaje != null) arregloBytes.AddRange(BitConverter.GetBytes(this.mensaje.Length));
                else
                    arregloBytes.AddRange(BitConverter.GetBytes(0));
            if (this.nombre != null) arregloBytes.AddRange(Encoding.UTF8.GetBytes(this.nombre));
            if (this.mensaje != null) arregloBytes.AddRange(Encoding.UTF8.GetBytes(this.mensaje));
            return arregloBytes.ToArray();
            }
        }
    
}
