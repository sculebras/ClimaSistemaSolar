using ClimaSistemaSolar.Component.Model;
using SF.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.Component.Repositories
{
    public class ClimaRepository : Repository<Clima>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ClimaRepository(DbContext context)
                : base(context)
        { }

        //TODO: Se deberia localizar segun idioma:
        private const string MSG_CLIMA_SIN_ESPECIFICAR = "Sin Especificar";
        private const string MSG_DIA_FUERA_DE_RANGO = "Día fuera de rango admitido. Min: 1. Max: {0} ({1} años)";

        /// <summary>
        /// Obtiene Clima de un dia especifico.
        /// </summary>
        /// <param name="dia">Dia del cual se quiere obtener el clima.</param>
        /// <returns></returns>
        public override Clima Retrieve(int dia)
        {
            //Validación de dia:
            int iDiaMax = Constants.ANIOS * 365;
            if (dia <1 ||dia > iDiaMax)
            {
                return new Clima() { dia = dia, clima = string.Format(MSG_DIA_FUERA_DE_RANGO, iDiaMax, Constants.ANIOS) };
            }

            Clima oClima = base.RetrieveQueryable("TipoClima").FirstOrDefault(e => e.Id == dia);
            if (oClima == default(Clima))
            {
                return new Clima() { dia = dia, clima = MSG_CLIMA_SIN_ESPECIFICAR };
            }
            else
            {
                return oClima;
            }
        }

    }
}
