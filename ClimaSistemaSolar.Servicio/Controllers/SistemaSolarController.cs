using ClimaSistemaSolar.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ClimaSistemaSolar.Servicio.Controllers
{
    public class SistemaSolarController : ApiController
    {

        
        // GET api/SistemaSolar/SimulacionClima
        [HttpGet]
        [ActionName("SimulacionClima")]
        public string SimulacionClima()
        {
            return new SistemaSolar().SimulacionClima(); ;
        }
        

    }
}
