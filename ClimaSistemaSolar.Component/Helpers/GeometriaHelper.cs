using ClimaSistemaSolar.Component.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.Component.Helpers
{
    /// <summary>
    /// Metodos para hacer cálculos de Geometría.
    /// </summary>
    public static class GeometriaHelper
    {
        /// <summary>
        /// Verifica si 2 angulos son iguales u opuestos.
        /// </summary>
        /// <param name="angulo1"></param>
        /// <param name="angulo2"></param>
        /// <returns></returns>
        public static bool EsAnguloIgualUOpuesto(int angulo1, int angulo2)
        {
            return angulo1 == angulo2 || angulo1 == angulo2 + 180 || angulo1 + 180 == angulo2;
        }

        /// <summary>
        /// Dado 2 puntos que trazan una recta verifica si los puntos ingresados estas contenidos en la misma.
        /// </summary>
        /// <see cref="http://www.vitutor.com/geo/rec/d_7.html"/>
        /// <param name="PuntoRecta1">Punto 1 de la recta</param>
        /// <param name="PuntoRecta2">Punto 2 de la recta</param>
        /// <param name="arrPuntosAVerificar">Puntos a Verificar.</param>
        /// <returns></returns>
        public static bool PuntosPertenceARecta(CoordenadasCartesianas PuntoRecta1,
                                               CoordenadasCartesianas PuntoRecta2,
                                               params CoordenadasCartesianas[] arrPuntosAVerificar)
        {
            bool blResultado = false;
            foreach (CoordenadasCartesianas puntoAVerificar in arrPuntosAVerificar)
            {
                //Chequea si es recta horizontal (sino, da division por cero en la formula)
                if (PuntoRecta1.X == PuntoRecta2.X && puntoAVerificar.X == PuntoRecta1.X)
                {
                    blResultado = true;
                }
                //Formula: (((x-x1)/(X2-x1))*(y2-y1))+y1-y=0
                //http://www.vitutor.com/geo/rec/d_7.html
                else if ((((puntoAVerificar.X - PuntoRecta1.X) / (PuntoRecta2.X - PuntoRecta1.X)) * (PuntoRecta2.Y - PuntoRecta1.Y)) + PuntoRecta1.Y - puntoAVerificar.Y == 0)
                {
                    blResultado = true;
                }
                else {
                    return false;
                }
            }
            return blResultado;            
        }

        /// <summary>
        /// Determina si un punto esta contenido dentro de un triangulo.
        /// </summary>
        /// <see cref="http://www.dma.fi.upm.es/personal/mabellanas/tfcs/kirkpatrick/Aplicacion/algoritmos.htm#puntoInterior"/>
        /// <seealso cref="http://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle"/>
        /// <param name="puntoAAveriguar">Punto que se quiere averiguar si esta dentro del triángulo.</param>
        /// <param name="punto1">Punto del triángulo.</param>
        /// <param name="punto2">Punto del triángulo</param>
        /// <param name="punto3">Punto del triángulo</param>
        /// <returns></returns>
        public static bool EsPuntoAdentroTriangulo(CoordenadasCartesianas puntoAAveriguar, 
                                                   CoordenadasCartesianas punto1, 
                                                   CoordenadasCartesianas punto2, 
                                                   CoordenadasCartesianas punto3)
        {
            bool b1, b2, b3;

            b1 = OrientacionTiangulo(puntoAAveriguar, punto1, punto2) < 0.0f;
            b2 = OrientacionTiangulo(puntoAAveriguar, punto2, punto3) < 0.0f;
            b3 = OrientacionTiangulo(puntoAAveriguar, punto3, punto1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        /// <summary>
        /// Calcula la orientacion de un triángulo.
        /// </summary>
        /// <param name="punto1">Punto del triángulo.</param>
        /// <param name="punto2">Punto del triángulo</param>
        /// <param name="punto3">Punto del triángulo</param>
        /// <returns></returns>
        private static double OrientacionTiangulo(CoordenadasCartesianas punto1, 
                                                  CoordenadasCartesianas punto2, 
                                                  CoordenadasCartesianas punto3)
        {
            return (punto1.X - punto3.X) * (punto2.Y - punto3.Y) - (punto2.X - punto3.X) * (punto1.Y - punto3.Y);
        }

        /// <summary>
        /// Devuelve el perímetro de un triángulo.
        /// </summary>
        /// <param name="punto1">Punto del triángulo.</param>
        /// <param name="punto2">Punto del triángulo</param>
        /// <param name="punto3">Punto del triángulo</param>
        /// <returns></returns>
        public static double PerimetroTriangulo(CoordenadasCartesianas punto1,
                                                CoordenadasCartesianas punto2,
                                                CoordenadasCartesianas punto3)
        {
            return PerimetroPoligono(punto1, punto2, punto3);
        }

        /// <summary>
        /// Devuelve el perímetro de cualquier poligono ingresando sus puntos.
        /// </summary>
        /// <param name="paramsPuntos"></param>
        /// <returns></returns>
        public static double PerimetroPoligono(params CoordenadasCartesianas[] paramsPuntos)
        {
            if (paramsPuntos.Length < 3)
            {
                throw new ApplicationException("Deben ingresarse como mínimo 3 puntos.");
            }

            double dblResultado = 0;
            for (int i = 0; i < paramsPuntos.Length-1; i++)
            {
                dblResultado += DistanciaEntre2Puntos(paramsPuntos[i], paramsPuntos[i + 1]);
            }
            dblResultado += DistanciaEntre2Puntos(paramsPuntos[paramsPuntos.Length - 1], paramsPuntos[0]);

            return dblResultado;
        }

        /// <summary>
        /// Determina la distancia entre 2 puntos.
        /// </summary>
        /// <see cref="http://www.profesorenlinea.cl/geometria/Distancia_entre_dos_puntos.html"/>
        /// <param name="punto1"></param>
        /// <param name="punto2"></param>
        /// <returns></returns>
        public static double DistanciaEntre2Puntos(CoordenadasCartesianas punto1,
                                                   CoordenadasCartesianas punto2)
        {
            return Math.Sqrt(Math.Pow(punto2.X - punto1.X, 2) + Math.Pow(punto2.Y - punto1.Y, 2));
        }
    }
}
