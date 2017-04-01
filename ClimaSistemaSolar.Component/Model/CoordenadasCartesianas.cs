using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.Component.Model
{
    /// <summary>
    /// Representacion de una posición en un plano mediante coordenadas cartesianas.
    /// </summary>
    public class CoordenadasCartesianas
    {
        public double X { get; set; }
        public double Y { get; set; }

        #region CONSTRUCTORES
        public CoordenadasCartesianas() { }
        public CoordenadasCartesianas(double x, double y)
        {
            this.X = x;
            this.Y = y;
        } 
        #endregion

        public override string ToString()
        {
            return string.Format("({0} , {1})", this.X.ToString("F"), this.Y.ToString("F"));
        }

        public static string ToString(IEnumerable<CoordenadasCartesianas> listCoord) {
            StringBuilder sb = new StringBuilder();
            foreach (var item in listCoord)
            {
                if (sb.Length != 0)
                {
                    sb.Append(" ");
                }
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }
    }
}
