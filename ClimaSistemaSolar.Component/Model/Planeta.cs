using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.Component.Model
{
    public class Planeta
    {
        #region PROPIEDADES
        /// <summary>
        /// Nombre del Planeta.
        /// </summary>
        public string Nombre { get; set; }
       
        /// <summary>
        /// velocidad angular grados/día.
        /// Si es negativa es antihoraria.
        /// </summary>
        public int VelocidadAngular { get; set; }
        /// <summary>
        /// Posicion del planeta expresado en Distancia y Angulo.
        /// </summary>
        public CoordenadasPolares CoordenadasPolares { get; set; }


        private CoordenadasCartesianas _CoordenadasCartesianas;
        /// <summary>
        /// Posicion de Coordenadas Cartesianas en base a las corrdenadas polares.
        /// </summary>
        public CoordenadasCartesianas CoordenadasCartesianas {
            get
            {
                //Solo se calcula sino fue calculado antes (para mejorar performance)
                if (this._CoordenadasCartesianas == null)
                {
                    this._CoordenadasCartesianas = this.CoordenadasPolares.ConvertirACartesianas();
                }
                return this._CoordenadasCartesianas;
            }
        }

        #endregion


        #region CONSTRUCTOR
        public Planeta(string strNombre,
                       int iDistancia,
                       int iVelocidadAngular,
                       int iAnguloPosicionInicial = 0)
        {
            this.Nombre = strNombre;
            this.VelocidadAngular = iVelocidadAngular;
            this.CoordenadasPolares = new CoordenadasPolares() { Distancia = iDistancia, Angulo = iAnguloPosicionInicial };
        }
        #endregion

        /// <summary>
        /// Calcula la nueva posicion del planeta del dia siguiente.
        /// </summary>
        internal void CalcularNuevaPosicion()
        {
            //Reinicia coordenadas cartesianas
            this._CoordenadasCartesianas = null;

            this.CoordenadasPolares.Angulo += this.VelocidadAngular;
            if (this.CoordenadasPolares.Angulo < 0)
            {
                this.CoordenadasPolares.Angulo = 360 + this.CoordenadasPolares.Angulo;
            }
            else if (this.CoordenadasPolares.Angulo >= 360)
            {
                this.CoordenadasPolares.Angulo = this.CoordenadasPolares.Angulo - 360;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:\t{1}\t{2}", this.Nombre, this.CoordenadasPolares.ToString(), this.CoordenadasCartesianas.ToString());
        }


    }
}
