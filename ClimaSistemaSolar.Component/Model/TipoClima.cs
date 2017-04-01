using SF.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.Component.Model
{
    public class TipoClima : EntityBase
    {
        /// <summary>
        /// Tipos de Clima.
        /// </summary>
        public enum enumTipoClima
        {
            Sequia = 1,
            Lluvia = 2,
            LluviaPicoMaximo = 3,
            Optimo = 4
        }

        [DataMember]
        [Column("IdTipoClima")]
        public override int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        

        #region Navigation Properties
        public virtual ICollection<Clima> Climas { get; set; }
        #endregion
    }
}
