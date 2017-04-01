using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.ComponentTests
{
    internal class TestsConstants
    {
        internal const string CORRECTO = "CORRECTO";
        internal const string FALLO = "FALLO";
        /// <summary>
        /// Habilita los test que van contra la base de datos
        /// (Se puso para poder subir a AppHarbour.ya que para subir la solucion primero debe pasar todas las pruebas
        /// unitarias y al no tener seteada bien la cadena de conexion le da errores en los tests)
        /// </summary>
        public static bool DESHABILITAR_DB_TESTS = Convert.ToBoolean(ConfigurationManager.AppSettings["DESHABILITAR_DB_TESTS"]);
        
    }
}
