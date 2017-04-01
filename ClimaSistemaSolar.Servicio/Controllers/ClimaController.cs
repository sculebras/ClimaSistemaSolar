using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClimaSistemaSolar.Component.Model;
using ClimaSistemaSolar.Component.Repositories;
using ClimaSistemaSolar.Component;

namespace ClimaSistemaSolar.Servicio.Controllers
{
    public class ClimaController : ApiController
    {
        // GET api/clima/5
        [HttpGet]
        [Route("api/clima/{id}")]
        public Clima Get(int id)
        {
            using (UOWClimaSistemaSolar unitOfWork = new UOWClimaSistemaSolar())
            {
                Clima oClima = unitOfWork.ClimaRepository.Retrieve(id);
                return oClima;
            }
        }


        // GET api/clima/DeleteAll
        [HttpGet]
        //[ActionName("DeleteAll")]
        [Route("api/clima/DeleteAll")]
        public string DeleteAll()
        {
            using (UOWClimaSistemaSolar unitOfWork = new UOWClimaSistemaSolar())
            {
                unitOfWork.ClimaRepository.DeleteAll();
                return "Se han eliminado todos los datos del Clima.";
            }
        }
    }
}
