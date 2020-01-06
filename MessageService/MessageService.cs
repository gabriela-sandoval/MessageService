using AccesoBaseDeDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace MessageService
{
    /// <summary>
    /// La clase MessageService se encarga de realizar las operaciones de los metodos implementados por la interfaz.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single,InstanceContextMode = InstanceContextMode.Single)]

    public class MessageService : IUserManager
    {
        readonly Dictionary<IMessageServiceCallback, string> jugadoresConectados = new Dictionary<IMessageServiceCallback, string>();
        private IMessageServiceCallback messageServiceCallbackChannel;

        /// <summary>
        /// Constructor que llama al cliente.
        /// </summary>
        public MessageService()
        {

        }

        /// <summary>
        ///  Requiere de una operacion de la interfaz IMessageService
        /// </summary>
        /// <param name="messageServiceCallbackCreator"></param>
        public MessageService(IMessageServiceCallback messageServiceCallbackCreator)
        {
            this.messageServiceCallbackChannel = messageServiceCallbackCreator ?? throw new ArgumentException("messageServiceCallbackCreator");
        }

        /// <summary>
        /// Permite crear una nueva cuenta a un jugador.
        /// </summary>
        /// <param name="jugador">Recibe un parametro jugador de tipo jugador</param>
        public void CrearCuentaJugador(Jugador jugador)
        {
            try
            {
                Console.WriteLine("BaseDeDatosLoterestEntities");
                BaseDeDatosLoterestEntities baseDeDatosLoterestEntities = new BaseDeDatosLoterestEntities();
                Console.WriteLine("BaseDeDatosLoterestEntities2");
                Jugador nuevoJugador = (from per in baseDeDatosLoterestEntities.Jugador where per.NombreJugador == jugador.NombreJugador select per).First();
                Console.WriteLine("Consulta");

                if(nuevoJugador != null)
                {
                    OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>().Respuesta("El nombre del jugador ya se encuentra registrado");
                }
            }catch (InvalidOperationException)
            {
                BaseDeDatosLoterestEntities baseDeDatosLoterestEntities = new BaseDeDatosLoterestEntities();
                baseDeDatosLoterestEntities.Jugador.Add(jugador);
                baseDeDatosLoterestEntities.SaveChanges();
                Console.WriteLine("Nuevo jugador registrado: " + jugador.NombreJugador);
                baseDeDatosLoterestEntities.Dispose();
            }
        }

        /// <summary>
        /// Permite iniciar sesion a un jugador.
        /// </summary>
        /// <param name="nombreUsuario">Recibe un parametro nombre de usuario de tipo string</param>
        /// <param name="contraseñaUsuario">Recibe un parametro contraseña de tipo string</param>
        public void IniciarSesion(string nombreUsuario, string contraseñaUsuario)
        {
            try
            {
                BaseDeDatosLoterestEntities baseDeDatosLoterestEntities = new BaseDeDatosLoterestEntities();
                var cuenta = (from per in baseDeDatosLoterestEntities.Jugador where per.NombreJugador == nombreUsuario && per.ContraseñaJugador == contraseñaUsuario select per).First();
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>().DevuelveCuenta(cuenta);
                var conexion = OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>();
                jugadoresConectados[conexion] = nombreUsuario;
                Console.WriteLine("Jugador ha iniciado sesión: " + cuenta.NombreJugador + " // Puntaje: " + cuenta.PuntajeJugador);
                baseDeDatosLoterestEntities.Dispose();
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>().Respuesta("El usuario o contraseña son incorrectos, intente nuevamente");
            }
        }

        /// <summary>
        /// Permite cambiar la contraseña del jugador.
        /// </summary>
        /// <param name="jugador">Recibe un parametro jugador de tipo jugador</param>
        public void CambiarDatosJugador(Jugador jugador)
        {
            try
            {
                BaseDeDatosLoterestEntities baseDeDatosLoterestEntities = new BaseDeDatosLoterestEntities();
                var cuenta = (from per in baseDeDatosLoterestEntities.Jugador where per.NombreJugador == jugador.NombreJugador select per).First();
                cuenta.CorreoJugador = jugador.CorreoJugador;
                cuenta.ContraseñaJugador = jugador.ContraseñaJugador;
                baseDeDatosLoterestEntities.SaveChanges();
                Console.WriteLine("Se han modificado los datos del jugador: " + jugador.NombreJugador);
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>().Respuesta("Se modificaron los datos con éxito");
                baseDeDatosLoterestEntities.Dispose();
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>().Respuesta("Alguno de los datos introducidos es incorrecto, intente nuevamente");
            }
        }

        /// <summary>
        /// Permite registrar un nuevo puntaje.
        /// </summary>
        /// <param name="nombreUsuario">Recibe un parametro nombre de usuario de tipo string</param>
        /// <param name="puntajeUsuario">Recibe un parametro puntaje de usuario de tipo int</param>
        public void PuntajeMaximo(string nombreUsuario, int puntajeUsuario)
        {
            try
            {
                BaseDeDatosLoterestEntities baseDeDatosLoterestEntities = new BaseDeDatosLoterestEntities();
                var cuenta = (from per in baseDeDatosLoterestEntities.Jugador where per.NombreJugador == nombreUsuario select per).First();
                if (cuenta.PuntajeJugador == null)
                {
                    cuenta.PuntajeJugador = puntajeUsuario;
                    baseDeDatosLoterestEntities.SaveChanges();
                    Console.WriteLine(cuenta.NombreJugador + " tiene un nuevo puntaje: " + cuenta.PuntajeJugador);
                }
                else
                {
                    if(cuenta.PuntajeJugador < puntajeUsuario)
                    {
                        cuenta.PuntajeJugador = puntajeUsuario;
                        baseDeDatosLoterestEntities.SaveChanges();
                        Console.WriteLine(cuenta.NombreJugador + " tiene un nuevo puntaje: " + cuenta.PuntajeJugador);
                    }
                    else
                    {
                        Console.WriteLine(cuenta.NombreJugador + " su puntaje anterior era más alto: " + cuenta.PuntajeJugador);
                    }
                }
                baseDeDatosLoterestEntities.Dispose();
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>().Respuesta("Ocurrió un error al intentar registrar un nuevo puntaje");
            }
        }

        /// <summary>
        /// Permite enviar mensajes en el chat.
        /// </summary>
        /// <param name="nombreUsuario">Recibe un parametro nombre de usuario de tipo string</param>
        /// <param name="mensajeUsuario">Recibe un parametro mensaje de usuario de tipo string</param>
        /// <param name="nombreRemitente">Recibe un parametro nombre de remitente de tipo string</param>
        public void EnviarMensajeChat(string nombreUsuario, string mensajeUsuario, string nombreRemitente)
        {
            string mensajeChat = nombreUsuario + " dice: " + mensajeUsuario;
            Thread.Sleep(10);
            var conexion = OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>();

            foreach (var destinatario in jugadoresConectados)
            {
                if(destinatario.Value.Equals(nombreRemitente))
                {
                    if(destinatario.Key == conexion)
                        continue;
                    destinatario.Key.MensajeChat(mensajeChat);
                }
            }
        }

        /// <summary>
        /// Solicita el puntaje de los jugadores.
        /// </summary>
        public void SolicitarPuntaje()
        {
            try
            {
                List<PuntajeUsuario> puntajesUsuarios = new List<PuntajeUsuario>();
                using (var baseDeDatos = new BaseDeDatosLoterestEntities())
                {
                    var cuentas = from s in baseDeDatos.Jugador
                                  orderby s.PuntajeJugador descending
                                  select s;
                    foreach(var valor in cuentas)
                    {
                        if(valor.PuntajeJugador != null)
                        {
                            puntajesUsuarios.Add(new PuntajeUsuario(valor.NombreJugador, valor.PuntajeJugador));
                            Console.WriteLine("Puntaje: " + valor.NombreJugador + " = " + valor.PuntajeJugador);
                        }
                    }
                }
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>().MostrarPuntajes(puntajesUsuarios);
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>().Respuesta("No se pudo acceder a la base de datos, intente más tarde");
            }
        }

        /// <summary>
        /// Permite cerrar la sesion del jugador.
        /// </summary>
        /// <param name="nombreUsuario">Recibe un parametro nombre de usuario de tipo string</param>
        public void CerrarSesion(string nombreUsuario)
        {
            var conexion = OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>();
            jugadoresConectados[conexion] = nombreUsuario;
        }

        /// <summary>
        /// Finaliza el juego cuando uno de los dos jugadores gana
        /// </summary>
        /// <param name="nombreUsuario">Recibe un parametro nombre de usuario de tipo string</param>
        /// <param name="remitente">Recibe un parametro remitente de tipo string</param>
        public void FinalizarJuego(string nombreUsuario, string remitente)
        {
            var conexion = OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>();
            foreach(var perdedor in jugadoresConectados)
            {
                if(perdedor.Value.Equals(remitente))
                {
                    if (perdedor.Key == conexion)
                        continue;
                    perdedor.Key.FinPartida("El jugador: " + nombreUsuario + " ganó");
                }
            }
        }

        /// <summary>
        /// Permite enviar la invitación para unirse a un juego.
        /// </summary>
        /// <param name="mensajeUsuario">Recibe un parametro mensaje de usuario de tipo string</param>
        /// <param name="modoJuego">Recibe un parametro modo de juego de tipo string</param>
        /// <param name="nombreUsuario">Recibe un parametro nombre de usuario de tipo string</param>
        /// <param name="remitente">Recibe un parametro remitente de tipo string</param>
        public void EnviarInvitacion(string mensajeUsuario, string modoJuego, string nombreUsuario, string remitente)
        {
            try
            {
                var conexion = OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>();
                foreach(var jugadorInvitado in jugadoresConectados)
                {
                    if((jugadorInvitado.Value.Equals(remitente)) && (!jugadorInvitado.Value.Equals(nombreUsuario)))
                    {
                        if (jugadorInvitado.Key == conexion)
                            continue;
                        jugadorInvitado.Key.RecibirInvitacion(nombreUsuario, mensajeUsuario, modoJuego);
                    }
                }
            }catch (NullReferenceException)
            {
                OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>().Respuesta("El jugador que invitaste no está conectado");
            }
        }

        /// <summary>
        /// Permite confirmar la invitación del juego.
        /// </summary>
        /// <param name="opcion">Recibe un parametro opcion de tipo bool</param>
        /// <param name="modoJuego">Recibe un parametro modo de juego de tipo string</param>
        /// <param name="nombreUsuario">Recibe un parametro nombre de usuario de tipo string</param>
        /// <param name="remitente">Recibe un parametro remitente de tipo string</param>
        public void ConfirmarInvitacion(bool opcion, string modoJuego, string nombreUsuario, string remitente)
        {
            var conexion = OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>();
            foreach(var jugadorRemitente in jugadoresConectados)
            {
                if(jugadorRemitente.Value.Equals(remitente))
                {
                    if (jugadorRemitente.Key == conexion)
                        continue;
                    jugadorRemitente.Key.RecibirConfirmacion(opcion, nombreUsuario, modoJuego);
                }
            }
        }

    }
}
