using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClimaSistemaSolar.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ClimaSistemaSolar.ComponentTests;
using ClimaSistemaSolar.Component.Model;
using SF.Logger;

namespace ClimaSistemaSolar.Component.Tests
{
    [TestClass()]
    public class SistemaSolarTests: TestBase
    {
         #region CONSTRUCTORES
        public SistemaSolarTests() { }
        /// <summary>
        /// Define si usar Assert. 
        /// Para Ejecucion de prueba: false.
        /// Para ejecutar desde consola: true.
        /// </summary>
        /// <param name="blNoEvaluarAssert"></param>
        public SistemaSolarTests(bool blNoEvaluarAssert) : base(blNoEvaluarAssert) { }
        #endregion

        /// <summary>
        /// Ejecuta la Simulacion de Clima.
        /// Si no tira excepcion se da como prueba satisfactoria.
        /// Para verificar resultado es mejor ejecutar el test desde el proyecto de consola 
        /// que tiene salida por consola y log de archivo.
        /// </summary>
        [TestMethod()]
        public void SimulacionClimaTest()
        {
            if (!TestsConstants.DESHABILITAR_DB_TESTS)
            {
                string strMethod = Logger.TraceStartMethod();
                new SistemaSolar().SimulacionClima();
            }
        }

        /// <summary>
        /// Test de simulacion de clima con otras condiciones planetarias.
        /// </summary>
        [TestMethod()]
        public void SimulacionClimaTestAlternativo()
        {
            if (!TestsConstants.DESHABILITAR_DB_TESTS)
            {
                string strMethod = Logger.TraceStartMethod();
                List<Planeta> planetas = new List<Planeta>()
                {
                    new Planeta("Ferengi", 100, 1, 90),
                    new Planeta("Betasoide", 200, 2,  90),
                    new Planeta("Vulcano", 300, 3, 90)
                };
                new SistemaSolar(planetas).SimulacionClima();
            }
        }

    }
}