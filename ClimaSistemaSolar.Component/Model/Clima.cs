using SF.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.Component.Model
{
    /// <summary>
    /// Representa el Clima Diario
    /// </summary>
    [DataContract]
    public class Clima : EntityBase
    {
        private string _clima;
        
        /// <summary>
        /// Id para EF (SF.Core framework) para mapear con la base de datos.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[IgnoreDataMember]
        [Column("Dia")]
        public override int Id { get; set; }

        [DataMember]
        [NotMapped]
        public int dia 
        {
            get { return this.Id; }
            set { this.Id = value; }
        }
        public int IdTipoClima { get; set; }

        [NotMapped]
        public TipoClima.enumTipoClima enumTipoClima
        {
            set { this.IdTipoClima = (int)value; }
        }

        /// <summary>
        /// Descripción del tipo de Clima.
        /// </summary>
        [NotMapped]
        [DataMember]
        public string clima {
            get {
                if (this.TipoClima != null)
                {
                    this._clima = this.TipoClima.Descripcion;
                }
                return this._clima;
            }
            set {
                this._clima = value;
            }
        }
        

        #region Navigation Properties
        [ForeignKey("IdTipoClima")]
        public virtual TipoClima TipoClima { get; set; } 
        #endregion

    }
}
