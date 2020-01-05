using AccesoBaseDeDatos;
using MessageService;
using System.Collections.Generic;

namespace PruebasUnitarias
{
    class MetodosImplementados : IMessageServiceCallback
    {
        private Jugador jugador;

        internal MetodosImplementados()
        {
            jugador = null;
        }

        internal Jugador Response
        {
            get { return jugador; }
        }

        public void DevuelveCuenta(Jugador jugador)
        {
            this.jugador = jugador;
        }

        public void FinPartida(string mensaje)
        {
            throw new System.NotImplementedException();
        }

        public void MensajeChat(string mensaje)
        {
            throw new System.NotImplementedException();
        }

        public void MostrarPuntajes(List<PuntajeUsuario> puntajesUsuarios)
        {
            throw new System.NotImplementedException();
        }

        public void RecibirConfirmacion(bool opcion, string nombreUsuario, string modoJuego)
        {
            throw new System.NotImplementedException();
        }

        public void RecibirInvitacion(string nombreUsuario, string mensajeUsuario, string modoJuego)
        {
            throw new System.NotImplementedException();
        }

        public void Respuesta(string mensaje)
        {
            throw new System.NotImplementedException();
        }
    }
}
