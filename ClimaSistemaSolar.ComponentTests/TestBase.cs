using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Logger;

namespace ClimaSistemaSolar.ComponentTests
{
    /// <summary>
    /// Clase base para las clases de Test.
    /// </summary>
    [TestClass()]
    public class TestBase
    {
        /// <summary>
        /// Define si usar Assert. 
        /// Para Ejecucion de prueba: false.
        /// Para ejecutar desde consola: true.
        /// </summary>
        protected bool NoEvaluarAssert { get; set; }


        #region CONSTRUCTORES
        public TestBase() { }
        public TestBase(bool blNoEvaluarAssert)
        {
            this.NoEvaluarAssert = blNoEvaluarAssert;
        }
        #endregion


       
        /// <summary>
        /// Evalua Asssert si blEvaluarAssert=true (Para Ejecucion en Modo prueba)
        /// </summary>
        /// <param name="blEvaluarAssert">determina si usa assert o no.</param>
        /// <param name="blResultadoMetodo">resultado del metodo.</param>
        protected void EvaluaAssert(bool blResultadoMetodo)
        {
            if (!blResultadoMetodo && !this.NoEvaluarAssert)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Hace trace del resulatado del metodo y evalua si hay que hacer Assert.
        /// </summary>
        /// <param name="strMethod"></param>
        /// <param name="blResultadoMetodo"></param>
        protected void TraceResultMethod_EvaluaAssert(string strMethod, bool blResultadoMetodo)
        {
            Logger.Trace(blResultadoMetodo ? TraceEventType.Information : TraceEventType.Warning, string.Format("{0}: {1}.", strMethod, blResultadoMetodo ? TestsConstants.CORRECTO : TestsConstants.FALLO));
            this.EvaluaAssert(blResultadoMetodo);
        }


        /// <summary>
        /// Lanza una AssertFailedException con el mensaje ingresado y tambien hace trace del evento.
        /// </summary>
        /// <param name="strMethod"></param>
        /// <param name="strMsgFormat"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected static AssertFailedException FailedAssertTrace(string strMethod, string strMsgFormat, params string[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}: ", strMethod, TestsConstants.FALLO);
            sb.AppendFormat(strMsgFormat, parameters);
            string strMsg = sb.ToString();
            Logger.Trace(TraceEventType.Warning, strMsg);
            return new AssertFailedException(strMsg);
        }

    }
}
