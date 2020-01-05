using System;
using AccesoBaseDeDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PruebasUnitarias
{
    [TestClass]
    public class PruebaInicioSesion
    {
        [TestMethod]
        public void TestMethod1()
        {
            Jugador jugador = new Jugador()
            {
                NombreJugador = "Gabriela",
                CorreoJugador = "gabriela.uv@outlook.com",
                ContraseñaJugador = "12345",
                PuntajeJugador = null,
                PuntajeJugadorAlAzar = null
            };

            MetodosImplementados metodosImplementados = new MetodosImplementados();
            MessageService.MessageService messageService = new MessageService.MessageService(metodosImplementados);
            messageService.IniciarSesion("Gabriela", "12345");
            Assert.AreEqual<String>("Gabriela", metodosImplementados.Response.NombreJugador);
        }
    }
}
