using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClimaSistemaSolar.Component.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ClimaSistemaSolar.ComponentTests;
using SF.Logger;

namespace ClimaSistemaSolar.Component.Model.Tests
{
    [TestClass()]
    public class CoordenadasPolaresTests: TestBase
    {
        #region CONSTRUCTORES
        public CoordenadasPolaresTests() { }
        /// <summary>
        /// Define si usar Assert. 
        /// Para Ejecucion de prueba: false.
        /// Para ejecutar desde consola: true.
        /// </summary>
        /// <param name="blNoEvaluarAssert"></param>
        public CoordenadasPolaresTests(bool blNoEvaluarAssert) : base(blNoEvaluarAssert) { }
        #endregion

        /// <summary>
        /// Test de conversion de coordenadas polares a cartesianes con saltos de 45 grados distancia fija.
        /// Comparados contra resultados fijos calculados por una fuente externa.
        /// Ver excel: Test Manuales\Test Conversion Coordenadas Polares A Cartesianas.xlsx
        /// </summary>
        [TestMethod()]
        public void ConvertirACartesianasTest()
        {
            string strMethod = Logger.TraceStartMethod();
            int iDistancia = 2;

            Dictionary<CoordenadasPolares, CoordenadasCartesianas> ResultadosCorrectos = new Dictionary<CoordenadasPolares, CoordenadasCartesianas>() {
                { new CoordenadasPolares() {Distancia= iDistancia, Angulo=0 }, new CoordenadasCartesianas() { X=0, Y=2} },
                { new CoordenadasPolares() {Distancia= iDistancia, Angulo=45 }, new CoordenadasCartesianas() { X=1.41, Y=1.41} },
                { new CoordenadasPolares() {Distancia= iDistancia, Angulo=90 }, new CoordenadasCartesianas() { X=2, Y=0} },
                { new CoordenadasPolares() {Distancia= iDistancia, Angulo=135 }, new CoordenadasCartesianas() { X=1.41, Y=-1.41} },
                { new CoordenadasPolares() {Distancia= iDistancia, Angulo=180 }, new CoordenadasCartesianas() { X=0, Y=-2} },
                { new CoordenadasPolares() {Distancia= iDistancia, Angulo=225 }, new CoordenadasCartesianas() { X=-1.41, Y=-1.41} },
                { new CoordenadasPolares() {Distancia= iDistancia, Angulo=270 }, new CoordenadasCartesianas() { X=-2, Y=0} },
                { new CoordenadasPolares() {Distancia= iDistancia, Angulo=315 }, new CoordenadasCartesianas() { X=-1.41, Y=
                1.41} }
            };
            bool blResultadoMetodo = true;
            foreach (var cPolar in ResultadosCorrectos)
            {
                CoordenadasCartesianas cCartesianaCalculada = cPolar.Key.ConvertirACartesianas();
                string strMsgFormat = string.Format("{0} -> {1} = {2}",
                    cPolar.Key.ToString(), cCartesianaCalculada.ToString(), cPolar.Value.ToString());
                string strResultadoComparacion;

                if (cPolar.Value.X == Math.Round(cCartesianaCalculada.X, 2) && cPolar.Value.Y == Math.Round(cCartesianaCalculada.Y, 2))
                {
                    strResultadoComparacion = TestsConstants.CORRECTO;
                }
                else
                {
                    strResultadoComparacion = TestsConstants.FALLO;
                    blResultadoMetodo = false;
                }
                Logger.Trace(TraceEventType.Information, string.Format("{0} : {1}.", strMsgFormat, strResultadoComparacion));
            }

            base.TraceResultMethod_EvaluaAssert(strMethod, blResultadoMetodo);
        }
    }
}