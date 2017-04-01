using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClimaSistemaSolar.Component.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ClimaSistemaSolar.ComponentTests;
using ClimaSistemaSolar.Component.Model;
using SF.Logger;

namespace ClimaSistemaSolar.Component.Helpers.Tests
{
    [TestClass()]
    public class GeometriaHelperTests : TestBase
    {

        #region CONSTRUCTORES
        public GeometriaHelperTests() { }
        /// <summary>
        /// Define si usar Assert. 
        /// Para Ejecucion de prueba: false.
        /// Para ejecutar desde consola: true.
        /// </summary>
        /// <param name="blNoEvaluarAssert"></param>
        public GeometriaHelperTests(bool blNoEvaluarAssert) : base(blNoEvaluarAssert) { }
        #endregion


        /// <summary>
        /// Calse Auxiliar para ayudar en el Test para representar una recta definida por 2 puntos.
        /// </summary>
        private class Recta
        {
            public CoordenadasCartesianas Punto1 { get; set; }
            public CoordenadasCartesianas Punto2 { get; set; }
        }
        /// <summary>
        /// Calse Auxiliar para ayudar en el Test para representar una triangulo definido por 3 puntos.
        /// </summary>
        private class Triangulo
        {
            public CoordenadasCartesianas Punto1 { get; set; }
            public CoordenadasCartesianas Punto2 { get; set; }
            public CoordenadasCartesianas Punto3 { get; set; }

            public Triangulo(CoordenadasCartesianas punto1, CoordenadasCartesianas punto2, CoordenadasCartesianas punto3)
            {
                this.Punto1 = punto1;
                this.Punto2 = punto2;
                this.Punto3 = punto3;
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", this.Punto1.ToString(), this.Punto2.ToString(), this.Punto3.ToString());
            }
        }

        /// <summary>
        /// Calse Auxiliar para ayudar en el Test para representar la verificacion de si un punto esta dentro de un  triangulo definido por 3 puntos.
        /// </summary>
        private class PuntoEnTriangulo
        {
            public Triangulo Triangulo { get; set; }
            public CoordenadasCartesianas Punto { get; set; }

        }

        /// <summary>
        /// Testea el metodo helper para verificar si 2 angulos son iguales u opuestos.
        /// </summary>
        /// <param name="blEvaluarAssert">define si usar Assert. 
        /// Para Ejecucion de prueba: true.
        /// Para ejecutar desde consola: false.
        /// </param>
        [TestMethod()]
        public void EsAnguloIgualUOpuestoTest()
        {
            string strMethod = Logger.TraceStartMethod();
            //Angulos Iguales
            //Diccionario de deltas de angulos que se van a aplicar a los angulos con su resultado esperado
            Dictionary<int, bool> dicDeltaAnguloResultado = new Dictionary<int, bool>() {
                {0, true },
                {180, true },
                {45, false },
                {90, false }
            };
            bool blResultadoMetodo = true;
            foreach (var itemDeltaResultado in dicDeltaAnguloResultado)
            {
                bool blResultadoDelta = true;
                for (int angulo = 0; angulo < 360; angulo++)
                {
                    if (GeometriaHelper.EsAnguloIgualUOpuesto(angulo, angulo + itemDeltaResultado.Key) != itemDeltaResultado.Value)
                    {
                        Logger.Trace(TraceEventType.Warning, string.Format("{0}: Angulo1: {1} Angulo2: {2}", TestsConstants.FALLO, angulo, angulo + itemDeltaResultado.Key));
                        blResultadoDelta = false;
                        blResultadoMetodo = false;
                    }
                }
                //Loguea resultado del Delta a probar
                Logger.Trace(blResultadoDelta ? TraceEventType.Information : TraceEventType.Warning, string.Format("Delta Angulo: {0} - {1}", itemDeltaResultado.Key, blResultadoDelta ? TestsConstants.CORRECTO : TestsConstants.FALLO));
            }

            base.TraceResultMethod_EvaluaAssert(strMethod, blResultadoMetodo);
        }

        /// <summary>
        /// Se testea el metodo helper para calcular si un punto esta dentro de una recta definido por 2 puntos.
        /// Se tomaron puntos calculados manual de 2 rectas distintas. Ver Excel: Test Manuales\Test Puntos Cartesianos de Rectas.xlsx
        /// </summary>
        [TestMethod()]
        public void PuntosPertenceARectaTest()
        {
            string strMethod = Logger.TraceStartMethod();
            bool blResultadoMetodo = true;
            List<CoordenadasCartesianas> arr = new List<CoordenadasCartesianas>(){
                new CoordenadasCartesianas(), new CoordenadasCartesianas()
            };

            Dictionary<Recta, List<CoordenadasCartesianas>> dicResultadosCorrectos = new Dictionary<Recta, List<CoordenadasCartesianas>>()
            {
                //Punto: Recta :A(1,3) y B(2,-5)
                {new Recta() { Punto1= new CoordenadasCartesianas(1,3), Punto2 = new CoordenadasCartesianas(2,-5)},new List<CoordenadasCartesianas>(){ new CoordenadasCartesianas(5,-29), new CoordenadasCartesianas(6,-37) } },
                {new Recta() { Punto1= new CoordenadasCartesianas(1,2), Punto2 = new CoordenadasCartesianas(3,4)},new List<CoordenadasCartesianas>(){ new CoordenadasCartesianas(8,9), new CoordenadasCartesianas(9,10) } }
            };

            //Test de Puntos Especificos
            foreach (var puntoRecta in dicResultadosCorrectos)
            {
                if (!GeometriaHelper.PuntosPertenceARecta(puntoRecta.Key.Punto1, puntoRecta.Key.Punto2, puntoRecta.Value.ToArray()))
                {
                    Logger.Trace(TraceEventType.Warning, "{0}: {1} No pertenecen a Recta {2} , {3}",
                                 TestsConstants.FALLO, CoordenadasCartesianas.ToString(puntoRecta.Value), puntoRecta.Key.Punto1.ToString(), puntoRecta.Key.Punto2.ToString());
                    blResultadoMetodo = false;
                }
            }
            //Test de puntos calculados para una recta: Y= 11 - 8x
            CoordenadasCartesianas punto1 = new CoordenadasCartesianas(1,3);
            CoordenadasCartesianas punto2 = new CoordenadasCartesianas(2,-5);

            for (int x = 0; x <= 14; x++)
            {
                int y = 11 - 8 * x;//Formula de la recta a testear
                CoordenadasCartesianas punto = new CoordenadasCartesianas(x, y);
                if (!GeometriaHelper.PuntosPertenceARecta(punto1, punto2, punto))
                {
                    Logger.Trace(TraceEventType.Warning, "{0}: {1} No pertenece a Recta {2} , {3}",
                                 TestsConstants.FALLO, punto.ToString(), punto1.ToString(), punto2.ToString());
                    blResultadoMetodo = false;
                }
            }

            base.TraceResultMethod_EvaluaAssert(strMethod, blResultadoMetodo);
        }

        /// <summary>
        /// Se prueba el calculo de perimetro de triangulos probados contra otros calculados mediante el siguiente sitio:
        /// http://www.mathopenref.com/triangleperimeter.html
        /// </summary>
        /// <see cref="http://www.mathopenref.com/triangleperimeter.html"/>
        [TestMethod()]
        public void PerimetroTrianguloTest()
        {
            string strMethod = Logger.TraceStartMethod();
            bool blResultadoMetodo = true;

            //Diccionario de triangulos con sus perimetros calculados por fuentes externas
            Dictionary<Triangulo, double> dicResultadosCorrectos = new Dictionary<Triangulo, double>()
            {
                { new Triangulo(new CoordenadasCartesianas(0,0),
                                new CoordenadasCartesianas(0,11.5), 
                                new CoordenadasCartesianas(28,0)),
                  69.80
                },
                { new Triangulo(new CoordenadasCartesianas(-10.7,0), 
                                new CoordenadasCartesianas(0,0), 
                                new CoordenadasCartesianas(0,13.1)),
                40.70
                }
            };

            foreach (var triangulo in dicResultadosCorrectos)
            {
                double dblPerimetroCalculado = Math.Round(GeometriaHelper.PerimetroTriangulo(triangulo.Key.Punto1, triangulo.Key.Punto2, triangulo.Key.Punto3), 1);
                if (dblPerimetroCalculado != triangulo.Value)
                {
                    Logger.Trace(TraceEventType.Warning, "{0}: perímetro calculado: {1} - perímetro correcto: {2}",
                                 TestsConstants.FALLO, dblPerimetroCalculado.ToString("F"), triangulo.Value.ToString("F"));
                    blResultadoMetodo = false;
                }
            }
            base.TraceResultMethod_EvaluaAssert(strMethod, blResultadoMetodo);
        }
        
        /// <summary>
        /// Se prueba si un punto se ubica dentro de un triangulo.
        /// Se tomaron triangulos y puntos comprobados de una fuente externa (Manual).
        /// </summary>
        [TestMethod()]
        public void EsPuntoAdentroTrianguloTest()
        {
            string strMethod = Logger.TraceStartMethod();
            bool blResultadoMetodo = true;
            //Diccionario de triangulos y punto a evaluar si esta dentro del mismo
            //con sus respectivos resultados como valor. true: adentro, false: fuera
            Dictionary<PuntoEnTriangulo, bool> dicResultadosCorrectos = new Dictionary<PuntoEnTriangulo, bool>()
            {
                {new PuntoEnTriangulo() {
                    Punto = new CoordenadasCartesianas(0,0),
                    Triangulo = new Triangulo(new CoordenadasCartesianas(-2,1),
                                              new CoordenadasCartesianas(1,0.5),
                                              new CoordenadasCartesianas(-1,-2))
                },
                true },
                {new PuntoEnTriangulo() {
                    Punto = new CoordenadasCartesianas(2,-1),
                    Triangulo = new Triangulo(new CoordenadasCartesianas(-2,1),
                                              new CoordenadasCartesianas(1,0.5),
                                              new CoordenadasCartesianas(-1,-2))
                },
                false },
            };

            foreach (var triangulo in dicResultadosCorrectos)
            {
                if (GeometriaHelper.EsPuntoAdentroTriangulo(triangulo.Key.Punto, triangulo.Key.Triangulo.Punto1, triangulo.Key.Triangulo.Punto2, triangulo.Key.Triangulo.Punto3) != triangulo.Value)
                {
                    Logger.Trace(TraceEventType.Warning, "{0}: punto: {1} debería estar {2} del triangulo formado por {3}.",
                                 TestsConstants.FALLO, triangulo.Key.Punto.ToString(), triangulo.Value?"dentro":"fuera", triangulo.Key.Triangulo.ToString());
                    blResultadoMetodo = false;
                }
            }
            base.TraceResultMethod_EvaluaAssert(strMethod, blResultadoMetodo);
        }
    }
}