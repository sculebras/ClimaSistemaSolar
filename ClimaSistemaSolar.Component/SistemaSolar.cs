using ClimaSistemaSolar.Component.Helpers;
using ClimaSistemaSolar.Component.Model;
using SF.Core.Extensions;
using SF.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.Component
{
    public class SistemaSolar
    {

        /// <summary>
        /// Planetas del Sistema Solar.
        /// </summary>
        public List<Planeta> Planetas = new List<Planeta>()
            {
                new Planeta("Ferengi", 500, 1, Constants.ANGULO_INICIO_Ferengi),
                new Planeta("Betasoide", 2000, 3, Constants.ANGULO_INICIO_Betasoide),
                new Planeta("Vulcano", 1000, -5, Constants.ANGULO_INICIO_Vulcano)
            };

        #region CONSTRUCTORES
        public SistemaSolar(){}
        
        public SistemaSolar(List<Planeta> planetas) {
            if (planetas.Count!=3)
            {
                throw new Exception("El sistema solar debe tener 3 planetas.");
            }
            this.Planetas = planetas;
        }

        #endregion



        /// <summary>
        /// Ejecuta la simulación del clima durante los años ingresados por parametro (Deafault = 10 años).
        /// Almacena en base de datos la informacion diaria sólo de los siguientes tipos de clima:
        /// -sequía.
        /// -lluvia.
        /// -pico máximo de lluvia (dia puntual).
        /// -condiciones óptimas.
        /// Emite un reporte de la cantidad de de periodos de cada uno de los períodos 
        /// y el día que será el pico máximo de lluvia
        /// </summary>
        /// <returns></returns>
        public string SimulacionClima()
        {
            string strMetodo = "SimulacionClima"; //No se utiliza reflection para obtener nombre de metodo para mejorar performance
            Logger.TraceStart(strMetodo);
            Dictionary<TipoClima.enumTipoClima, int> dicContPeriodos = InicializarContadorPeriodos();
            //Alamacena el dia del perimetro maximo formado por los planetas que tambien es el de pico maximo de lluvia.
            int iDiaPerimetroMaximo = 0;
            using (UOWClimaSistemaSolar unitOfWork = new UOWClimaSistemaSolar())
            {
                //Flag que controla inicio de epocas de lluvia para que el contador cuente las temporadas (conjunto de varios dias juntos) y no que cuente los dias de lluvia.
                bool blInicioEpocaLluvia = false;
                //Variables que almacenan el maximo perimetro del triangulo formado por los planetas y el dia que sucede
                //Para determinar el dia de Pico máximo de Lluvia.
                double dblPerimetroMaximo = 0;
                
                //Se separo en 2 for por si a futuro se quiere realizar algun procedimiento que se ejecute para cada año.
                for (int anio = 1; anio <= Constants.ANIOS; anio++)
                {
                    for (int dia = 1; dia <= 360; dia++)
                    {
                        ProcesarDia(dicContPeriodos, ref iDiaPerimetroMaximo, unitOfWork, ref blInicioEpocaLluvia, ref dblPerimetroMaximo, anio, dia);
                    }
                }
                GrabaPicoMaximoLluvia(unitOfWork, iDiaPerimetroMaximo);
            }

            Logger.TraceStop(strMetodo);
            return ReportePeriodos(dicContPeriodos , iDiaPerimetroMaximo);
        }

        private void ProcesarDia(Dictionary<TipoClima.enumTipoClima, int> dicContPeriodos, ref int iDiaPerimetroMaximo, UOWClimaSistemaSolar unitOfWork, ref bool blInicioEpocaLluvia, ref double dblPerimetroMaximo, int anio, int dia)
        {
            int diaAbsoluto = dia + ((anio - 1) * 360);

            if (EsSequia())
            {
                IncrementaContadorPeriodosGrabaClima(diaAbsoluto, TipoClima.enumTipoClima.Sequia, dicContPeriodos, unitOfWork, ref blInicioEpocaLluvia);
            }
            else if (EsOptima())
            {
                IncrementaContadorPeriodosGrabaClima(diaAbsoluto, TipoClima.enumTipoClima.Optimo, dicContPeriodos, unitOfWork, ref blInicioEpocaLluvia);
            }
            else if (EsLluvia())
            {
                if (!blInicioEpocaLluvia)
                {
                    dicContPeriodos[TipoClima.enumTipoClima.Lluvia]++;
                    blInicioEpocaLluvia = true;
                }
                GrabarClima(unitOfWork, diaAbsoluto, TipoClima.enumTipoClima.Lluvia);
                DiaPicoMaximoLluvia(diaAbsoluto, ref iDiaPerimetroMaximo, ref dblPerimetroMaximo);
            }
            else
            {
                blInicioEpocaLluvia = false;
            }
            
            CalcularNuevaPosicionPlanetas(); 
        }

        /// <summary>
        /// Hace un Update del registro que pertence al dia con el Pico máximo de lluvia.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="iDiaPicoMaximoLluvia"></param>
        private static void GrabaPicoMaximoLluvia(UOWClimaSistemaSolar unitOfWork, int iDiaPicoMaximoLluvia)
        {
            if (iDiaPicoMaximoLluvia > 0)
            {
                unitOfWork.ClimaRepository.Update(new Clima() { Id = iDiaPicoMaximoLluvia, enumTipoClima = TipoClima.enumTipoClima.LluviaPicoMaximo });
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Va determinando cual es el perimetro maximo del triangulo formado por los planetas y el dia en que sucede.
        /// </summary>
        /// <param name="diaAbsoluto"></param>
        /// <param name="iDiaPerimetroMaximo"></param>
        /// <param name="dblPerimetroMaximo"></param>
        private void DiaPicoMaximoLluvia(int diaAbsoluto, ref int iDiaPerimetroMaximo, ref double dblPerimetroMaximo)
        {
            double dblPerimetro = GeometriaHelper.PerimetroTriangulo(this.Planetas[0].CoordenadasCartesianas,
                                                                     this.Planetas[1].CoordenadasCartesianas,
                                                                     this.Planetas[1].CoordenadasCartesianas);
            if (dblPerimetro > dblPerimetroMaximo)
            {
                dblPerimetroMaximo = dblPerimetro;
                iDiaPerimetroMaximo = diaAbsoluto;
            }
        }

        /// <summary>
        /// Incrementa el Contador de Periodos.
        /// Graba un registro en la Base de datos en la Tabla Clima.
        /// Realiza trace del estado de los planetas en ese momento (Si es que esta habilitada el trace en archivo de configuración).
        /// </summary>
        /// <param name="diaAbsoluto"></param>
        /// <param name="enumTipoClima"></param>
        /// <param name="dicContPeriodos"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="blInicioEpocaLluvia"></param>
        private void IncrementaContadorPeriodosGrabaClima(int diaAbsoluto,
                                                          TipoClima.enumTipoClima enumTipoClima,
                                                          Dictionary<TipoClima.enumTipoClima, int> dicContPeriodos, 
                                                          UOWClimaSistemaSolar unitOfWork, 
                                                          ref bool blInicioEpocaLluvia)
        {
            dicContPeriodos[enumTipoClima]++;
            blInicioEpocaLluvia = false;
            GrabarClima(unitOfWork, diaAbsoluto, enumTipoClima);
        }

       

        /// <summary>
        /// Graba un registro en la Base de datos en la Tabla Clima.
        /// Realiza trace del estado de los planetas en ese momento (Si es que esta habilitada el trace en archivo de configuración).
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="diaAbsoluto"></param>
        /// <param name="enumTipoClima"></param>
        private void GrabarClima(UOWClimaSistemaSolar unitOfWork, int diaAbsoluto, TipoClima.enumTipoClima enumTipoClima)
        {
            unitOfWork.ClimaRepository.Create(new Clima() { dia = diaAbsoluto, enumTipoClima = enumTipoClima });
            unitOfWork.Commit();
            TraceEstadoPlanetas(diaAbsoluto, enumTipoClima);
        }



        /// <summary>
        /// Devuelve un string con el reporte del conteo de las epocas de los distintos periodos
        /// y el Día pico máximo de lluvia.
        /// </summary>
        /// <param name="sbResultado">string</param>
        /// <param name="dicContPeriodos">diccionario contador de periodos.</param>
        /// <param name="iDiaPicoLluviaMaximo">Dia de LLuvia Máxima.</param>
        private static string ReportePeriodos(Dictionary<TipoClima.enumTipoClima, int> dicContPeriodos, int iDiaPicoLluviaMaximo)
        {
            StringBuilder sbResultado = new StringBuilder();
            foreach (var periodo in dicContPeriodos)
            {
                sbResultado.AppendLine();
                sbResultado.AppendFormat("{0}:\t{1}", periodo.Key.GetName(), periodo.Value);
            }
            sbResultado.AppendLine();
            sbResultado.AppendFormat("Día pico máximo de lluvia: {0}", iDiaPicoLluviaMaximo);
            sbResultado.AppendLine();
            string strResultado = sbResultado.ToString();
            Logger.Trace(TraceEventType.Information, strResultado);
            return strResultado;
        }

        /// <summary>
        /// Trace de la posicion de los planeta para un dia terminado.
        /// (Si es que esta habilitada el trace en archivo de configuración).
        /// </summary>
        /// <param name="diaAbsoluto"></param>
        /// <param name="enumTipoClima"></param>
        //Se puso en public para dejar expresado el test del metodo con fines de evaluacion. (en realidad deberia ser private) 
        public void TraceEstadoPlanetas(int diaAbsoluto, TipoClima.enumTipoClima enumTipoClima)
        {
            if (Constants.TRACE_ESTADO_PLANETAS) 
            {
                StringBuilder sb = new StringBuilder();
                foreach (Planeta oPlaneta in Planetas)
                {
                    sb.AppendLine();
                    sb.Append(oPlaneta.ToString());
                }

                string strMsg = string.Format("Dia: {0} - {1} {2}",
                    diaAbsoluto, enumTipoClima.GetName(), sb.ToString());
                Logger.Trace(TraceEventType.Verbose, strMsg);
            }            
        }

        /// <summary>
        /// Se fija si los planetas estan alineados entre si pero no con el sol.
        /// (Como este metodo se ejecuta luego de la evaluacion si estan alineados con el sol
        /// no es necesaria comprobacion que el sol no este alineado con los planetas.)
        /// Comprueba si en la recta que forman 2 de los planetas se encuentra el 3er planeta.
        /// </summary>
        /// <returns></returns>
        private bool EsOptima()
        {
            return Helpers.GeometriaHelper.PuntosPertenceARecta(Planetas[1].CoordenadasCartesianas,
                Planetas[2].CoordenadasCartesianas,
                Planetas[0].CoordenadasCartesianas);
         }

       

        /// <summary>
        /// Se fija si todos los planetas estan alineados con el sol.
        /// Para ello Se fija si los angulos de los planetas son iguales o es su opuesto.
        /// (Este metodo es mas rapido que calcular si estan en lineas los planetas con el sol)
        /// </summary>
        /// <returns></returns>
        private bool EsSequia()
        {
            int anguloPlaneta1 = Planetas[0].CoordenadasPolares.Angulo;
            for (int i = 1; i < Planetas.Count(); i++)
            {
                int anguloPlaneta = Planetas[i].CoordenadasPolares.Angulo;
                if (GeometriaHelper.EsAnguloIgualUOpuesto(anguloPlaneta1, anguloPlaneta))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;

            //Esta opcion es Valida pero es menos performante (calcula si los planetas estan alineados al sol):
            //return Helpers.GeometriaHelper.PuntosPertenceARecta(
            //    Planetas[1].CoordenadasCartesianas, Planetas[2].CoordenadasCartesianas,
            //    Planetas[0].CoordenadasCartesianas, new CoordenadasCartesianas(0, 0));

        }

        /// <summary>
        /// Se fija si el triangulo que forman los planetas tiene el sol dentro.
        /// </summary>
        /// <returns></returns>
        private bool EsLluvia()
        {
           return GeometriaHelper.EsPuntoAdentroTriangulo(new CoordenadasCartesianas(0,0), //Sol
                                                          Planetas[0].CoordenadasCartesianas,
                                                          Planetas[1].CoordenadasCartesianas,
                                                          Planetas[2].CoordenadasCartesianas);
        }


        /// <summary>
        /// Calcula la nueva posicion de los planetas para el siguiente dia.
        /// </summary>
        private void CalcularNuevaPosicionPlanetas()
        {
            Planetas.ForEach(p => p.CalcularNuevaPosicion());
        }

        /// <summary>
        /// Inicializa a cero el contador de periodos climaticos.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<TipoClima.enumTipoClima, int> InicializarContadorPeriodos()
        {
            Dictionary < TipoClima.enumTipoClima, int> dicContPeriodos = new Dictionary<TipoClima.enumTipoClima, int>();
            dicContPeriodos.Add(TipoClima.enumTipoClima.Lluvia, 0);
            dicContPeriodos.Add(TipoClima.enumTipoClima.Optimo, 0);
            dicContPeriodos.Add(TipoClima.enumTipoClima.Sequia, 0);            

            return dicContPeriodos;
        }
    }
}
