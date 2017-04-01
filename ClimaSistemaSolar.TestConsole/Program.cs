using ClimaSistemaSolar.Component;
using ClimaSistemaSolar.Component.Helpers;
using ClimaSistemaSolar.Component.Helpers.Tests;
using ClimaSistemaSolar.Component.Model;
using ClimaSistemaSolar.Component.Model.Tests;
using ClimaSistemaSolar.Component.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Logger;

namespace ClimaSistemaSolar.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Logger.Trace(TraceEventType.Start, "Application start");

                SimulacionClimaTest();
                //TestsUnitariosConsola();
                Logger.Trace(TraceEventType.Stop, "Application End");
            }
            catch (Exception ex)
            {
                Logger.TraceError(ex);
            }
            finally
            {
                Logger.TraceClose();
                Console.ReadLine();
            }
        }


        /// <summary>
        /// Ejecuta la Simulacion de Clima.
        /// Si no tira excepcion se da como prueba satisfactoria.
        /// Para verificar resultado es mejor ejecutar el teste desde el proyecto de consola 
        /// que tiene salida por consola y log de archivo.
        /// </summary>
        public static void SimulacionClimaTest()
        {
            string strMethod = Logger.TraceStartMethod();
            new SistemaSolar().SimulacionClima();
        }
        
        /// <summary>
        /// Agrupa las llamadas a los tests unitarios para que tengan salida por pantalla de consola 
        /// (a medida que vayan saliendo los mensajes del Trace).
        /// Los tests se pueden ejecutar por el entorno de Visual Studio (Ejecutar Pruebas).
        /// </summary>
        private static void TestsUnitariosConsola()
        {
            //DESCOMENTAR LOS QUE SE QUIERAN PROBAR (Indivudualmente o en Conjunto)
            
            CoordenadasPolaresTests oCoordenadasPolaresTests = new CoordenadasPolaresTests(true);
            //oCoordenadasPolaresTests.ConvertirACartesianasTest();

            GeometriaHelperTests oGeometriaHelperTests = new GeometriaHelperTests(true);
            //oGeometriaHelperTests.EsAnguloIgualUOpuestoTest();
            //oGeometriaHelperTests.PuntosPertenceARectaTest();
            //oGeometriaHelperTests.PerimetroTrianguloTest();
            //oGeometriaHelperTests.EsPuntoAdentroTrianguloTest();

            SistemaSolarTests oSistemaSolarTests = new SistemaSolarTests(true);
            //oSistemaSolarTests.SimulacionClimaTest();
            oSistemaSolarTests.SimulacionClimaTestAlternativo();

            UOWClimaSistemaSolarTests oUOWClimaSistemaSolarTests = new UOWClimaSistemaSolarTests(true);
            //oUOWClimaSistemaSolarTests.EscrituraLecturaTablaClimaTest();
            ////oUOWClimaSistemaSolarTests.EscrituraLecturaTablaTipoClimaTest();

            //TestSalidaTraceEstadoPlanetas(0, TipoClima.enumTipoClima.Lluvia);
        }

        
        /// <summary>
        /// Test del formato de Salida del trace del estado de los planetas
        /// La prueba satisfactoria es mirar una salida agradable tanto en consola como al Log en archivo.
        /// </summary>
        private static void TestSalidaTraceEstadoPlanetas(int diaAbsoluto, TipoClima.enumTipoClima enumTipoClima)
        {
            new SistemaSolar().TraceEstadoPlanetas(diaAbsoluto, enumTipoClima);
        }
                

    }
}
