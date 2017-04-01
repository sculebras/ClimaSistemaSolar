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
    public class UOWClimaSistemaSolarTests : TestBase
    {
        #region CONSTRUCTORES
        public UOWClimaSistemaSolarTests() { }
        /// <summary>
        /// Define si usar Assert. 
        /// Para Ejecucion de prueba: false.
        /// Para ejecutar desde consola: true.
        /// </summary>
        /// <param name="blNoEvaluarAssert"></param>
        public UOWClimaSistemaSolarTests(bool blNoEvaluarAssert) : base(blNoEvaluarAssert) { }
        #endregion

        /// <summary>
        /// Escritura y lectura de un registro individual de la tabla Clima.
        /// se verifica que se obtiene el registro escrito.
        /// Se verifica que los valores escritos para el registro sean los ingresados.
        /// </summary>
        [TestMethod()]
        public void EscrituraLecturaTablaClimaTest()
        {
            this.EscrituraLecturaTablaClimaTest(24, TipoClima.enumTipoClima.LluviaPicoMaximo);
        }


        /// <summary>
        /// Escritura y lectura de un registro individual de la tabla Clima.
        /// se verifica que se obtiene el registro escrito.
        /// Se verifica que los valores escritos para el registro sean los ingresados.
        /// </summary>
        /// <param name="iDia"></param>
        /// <param name="enumTipoClima"></param>
        [TestMethod()]
        private void EscrituraLecturaTablaClimaTest(int iDia, TipoClima.enumTipoClima enumTipoClima)
        {
            string strMethod = Logger.TraceStartMethod();
            bool blResultadoMetodo = true;
            Clima oClima;
            using (UOWClimaSistemaSolar unitOfWork = new UOWClimaSistemaSolar())
            {
                unitOfWork.ClimaRepository.Create(new Clima() { dia = iDia, enumTipoClima = enumTipoClima });
                unitOfWork.Commit();
               
                oClima = unitOfWork.ClimaRepository.Retrieve(iDia);
            }
            if (oClima == null || oClima == default(Clima))
            {
                Logger.Trace(TraceEventType.Warning, string.Format("{0}: {1}: No se obtuvo ningun registro", strMethod, TestsConstants.FALLO));
                blResultadoMetodo = false;
            }
            else if (!(oClima.dia == iDia && oClima.IdTipoClima == (int)enumTipoClima))
            {
                Logger.Trace(TraceEventType.Warning, string.Format("{4}: {5}:\n\t\tIngresado\tLeido\ndia:\t\t{0}\t\t{1}\nIdTipoClima:\t{2}\t\t{3}",
                    iDia, oClima.dia,
                    (int)enumTipoClima, oClima.IdTipoClima,
                    strMethod, TestsConstants.FALLO));
                blResultadoMetodo = false;
            }
            base.TraceResultMethod_EvaluaAssert(strMethod, blResultadoMetodo);
        }


        /// <summary>
        /// Escritura de todos los registros de la tabla TipoClima.
        /// Lectura de los registros
        /// Se compara la cantidad de registros escritos contra los leidos.
        /// EJECUTAR SOLO SI LA TABLA TipoClima y Clima estan vacia.
        /// </summary>
        [TestMethod()]
        public void EscrituraLecturaTablaTipoClimaTest()
        {
            string strMethod = Logger.TraceStartMethod();
            bool blResultadoMetodo = true;

            const int CANT_ITEMS = 4;
            TipoClima[] arrTipoClima = new TipoClima[CANT_ITEMS] {
                new TipoClima() { Id = 1, Descripcion = "Sequía" },
                new TipoClima() { Id = 2, Descripcion = "Lluvia" },
                new TipoClima() { Id = 3, Descripcion = "Lluvia Pico Máximo" },
                new TipoClima() { Id = 4, Descripcion = "Óptimo" }
            };
            int itemCount;
            using (UOWClimaSistemaSolar unitOfWork = new UOWClimaSistemaSolar())
            {
                foreach (TipoClima oTipoClima in arrTipoClima)
                {
                    unitOfWork.TipoClimaRepository.Create(oTipoClima);
                }
                unitOfWork.Commit();

                IEnumerable<TipoClima> arrTipoClima2 = unitOfWork.TipoClimaRepository.RetrieveEnumerable();
                itemCount = arrTipoClima2.Count();
            }

            if (itemCount != CANT_ITEMS) { 
                Logger.Trace(TraceEventType.Warning, string.Format("{2}: {3}: Se intento grabar {0} registros en Tabla TipoClima. Se leyeron {1} registros",
                    CANT_ITEMS, itemCount, strMethod, TestsConstants.FALLO));
                blResultadoMetodo = false;
            }
            base.TraceResultMethod_EvaluaAssert(strMethod, blResultadoMetodo);
        }


    }
}