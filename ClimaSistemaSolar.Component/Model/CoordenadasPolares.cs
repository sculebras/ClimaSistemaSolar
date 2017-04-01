using System;

namespace ClimaSistemaSolar.Component.Model
{
    public class CoordenadasPolares
    {
        public int Distancia { get; set; }
        /// <summary>
        /// Angulo que forma desde el eje Y.
        /// </summary>
        public int Angulo { get; set; }

        /// <summary>
        /// Calcula las Coordenadas Cartesianas.
        /// basandose como angulo el eje Y (desfazaje 90 grados y sentido horario)
        /// <see cref="http://www.disfrutalasmatematicas.com/graficos/coordenadas-polares-cartesianas.html"/>
        /// </summary>
        /// <returns></returns>
        public CoordenadasCartesianas ConvertirACartesianas()
        {
            CoordenadasCartesianas resultado = new CoordenadasCartesianas();
            //Es necesaria hacer ajuste de 90 grados ya que la formula de conversion esta expresada como grado 0 en el eje x
            //Y en la aplicacion lo estamos usando en base al eje Y.
            int iAnguloPolarReal = this.Angulo + 90;
            resultado.X = Math.Round(-this.Distancia * Math.Cos(iAnguloPolarReal * Math.PI/180), Constants.CANT_DECIMALES); //<-Se realizo ajuste de sentido de angulo de la formula original.
            resultado.Y = Math.Round(this.Distancia * Math.Sin(iAnguloPolarReal * Math.PI / 180), Constants.CANT_DECIMALES);
            return resultado;
        }

        public override string ToString()
        {
            return string.Format("({0} , {1}°)", this.Distancia, this.Angulo );
        }
    }
}