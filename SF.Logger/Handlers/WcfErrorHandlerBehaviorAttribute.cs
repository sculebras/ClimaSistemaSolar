using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace SF.Logger.Handlers
{
    //2014-11-19 02:05:44 sculebras: 
    /// <summary>
    /// Error handler for Services.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class WcfErrorHandlerBehaviorAttribute : Attribute, IServiceBehavior, IErrorHandler
    {

        protected Type ServiceType { get; set; }

        #region IServiceBehavior Implementation
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            //Dont do anything
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            this.ServiceType = serviceDescription.ServiceType;
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers) { 
                dispatcher.ErrorHandlers.Add(this); 
            }
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            //Dont do anything
        } 
        #endregion //IServiceBehavior Implementation



        #region IErrorHandler Implementation
        public bool HandleError(Exception error)
        {
            Logger.TraceError(error); //Calls log4net under the cover
            return false;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            fault = null; //Suppress any faults in contract
        } 
        #endregion //IErrorHandler Implementation
    }//End class
}//End Namespace				
				     
