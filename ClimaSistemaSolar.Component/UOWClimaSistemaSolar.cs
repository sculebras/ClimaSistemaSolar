using ClimaSistemaSolar.Component.DAL.Context;
using ClimaSistemaSolar.Component.Repositories;
using SF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClimaSistemaSolar.Component.Model;

namespace ClimaSistemaSolar.Component
{
    public class UOWClimaSistemaSolar : UnitOfWork
    {
        private TipoClimaRepository _TipoClimaRepository;
        private ClimaRepository _ClimaRepository;


        #region PROPERTIES
        
        /// <summary>
        /// Repository of TipoClima Entity
        /// </summary>
        public TipoClimaRepository TipoClimaRepository
        {
            get
            {
                if (this._TipoClimaRepository == null)
                {
                    this._TipoClimaRepository = new TipoClimaRepository(base.Context);
                }
                return this._TipoClimaRepository;
            }
        }
        /// <summary>
        /// Repository of Clima Entity
        /// </summary>
        public ClimaRepository ClimaRepository
        {
            get
            {
                if (this._ClimaRepository == null)
                {
                    this._ClimaRepository = new ClimaRepository(base.Context);
                }
                return this._ClimaRepository;
            }
        }

        #endregion //PROPERTIES

        /// <summary>
        /// Constructor
        /// </summary>
        public UOWClimaSistemaSolar()
        {
            base.Context = new ModelContext();
        }

    }//End class
}//End Namespace				
