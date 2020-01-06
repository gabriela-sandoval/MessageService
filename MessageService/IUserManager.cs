using AccesoBaseDeDatos;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MessageService
{
    /// <summary>
    /// La interfaz IUserManager se encarga de especificar los metodos que un cliente puede utilizar del servidor.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IMessageServiceCallback))]

    public interface IUserManager
    {
        [OperationContract(IsOneWay = true)]
        void CrearCuentaJugador(Jugador jugador);

        [OperationContract(IsOneWay = true)]
        void IniciarSesion(string nombreUsuario, string contraseñaUsuario);

        [OperationContract(IsOneWay = true)]
        void CambiarDatosJugador(Jugador jugador);

        [OperationContract(IsOneWay = true)]
        void PuntajeMaximo(string nombreUsuario, int puntajeUsuario);

        [OperationContract(IsOneWay = true)]
        void EnviarMensajeChat(string nombreUsuario, string mensajeUsuario, string nombreRemitente);

        [OperationContract(IsOneWay = true)]
        void SolicitarPuntaje();

        [OperationContract(IsOneWay = true)]
        void CerrarSesion(string nombreUsuario);

        [OperationContract(IsOneWay = true)]
        void FinalizarJuego(string nombreUsuario, string remitente);

        [OperationContract(IsOneWay = true)]
        void EnviarInvitacion(string mensajeUsuario, string modoJuego, string nombreUsuario, string remitente);

        [OperationContract(IsOneWay = true)]
        void ConfirmarInvitacion(bool opcion, string modoJuego, string nombreUsuario, string remitente);
    }

    /// <summary>
    /// La interfaz IMessageServiceCallback se encarga de especificar los metodos que un cliente puede utilizar del servidor.
    /// </summary>
    [ServiceContract]
    public interface IMessageServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void Respuesta(string mensaje);

        [OperationContract(IsOneWay = true)]
        void DevuelveCuenta(Jugador jugador);

        [OperationContract(IsOneWay = true)]
        void MensajeChat(string mensaje);

        [OperationContract(IsOneWay = true)]
        void MostrarPuntajes(List<PuntajeUsuario> puntajesUsuarios);

        [OperationContract(IsOneWay = true)]
        void RecibirConfirmacion(bool opcion, string nombreUsuario, string modoJuego);

        [OperationContract(IsOneWay = true)]
        void RecibirInvitacion(string nombreUsuario, string mensajeUsuario, string modoJuego);

        [OperationContract(IsOneWay = true)]
        void FinPartida(string mensaje);
    }
  
    [DataContract]
    public class PuntajeUsuario
    {
        [DataMember]
        string nombreUsuario;
        [DataMember]
        private int? puntajeUsuario;
        public PuntajeUsuario(string NombreUsuario, int? PuntajeUsuario)
        {
            this.nombreUsuario = NombreUsuario;
            this.puntajeUsuario = PuntajeUsuario;
        }
    }

}
