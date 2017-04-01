using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SF.Core
{
    [DataContract]
    public class EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //.Identity
        [Column("ID")]
        //[DataMember]
        public virtual int Id { get; set; }
    }//End class
}//End Namespace				
