using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.Component
{
    /// <summary>
    /// Constantes
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Decimales a redondear.
        /// </summary>
        public const int CANT_DECIMALES = 2;
        /// <summary>
        /// Años que se van a procesar
        /// </summary>
        public static int ANIOS = Convert.ToInt32(ConfigurationManager.AppSettings["ANIOS"]);
        /// <summary>
        /// Define si activar el trace de estados de planetas 
        /// (loguea la posicion de los planetas en las distintas Epocas detectadas)
        /// Para ambiente productivo deberia estar en false.
        /// </summary>
        public static bool TRACE_ESTADO_PLANETAS = Convert.ToBoolean(ConfigurationManager.AppSettings["TRACE_ESTADO_PLANETAS"]);

        public static int ANGULO_INICIO_Ferengi =Convert.ToInt32(ConfigurationManager.AppSettings["ANGULO_INICIO_Ferengi"]);
        public static int ANGULO_INICIO_Betasoide = Convert.ToInt32(ConfigurationManager.AppSettings["ANGULO_INICIO_Betasoide"]);
        public static int ANGULO_INICIO_Vulcano = Convert.ToInt32(ConfigurationManager.AppSettings["ANGULO_INICIO_Vulcano"]);

        
    }
}
