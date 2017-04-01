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
        public Clima Get(int id)
        {
            using (UOWClimaSistemaSolar unitOfWork = new UOWClimaSistemaSolar())
            {
                Clima oClima = unitOfWork.ClimaRepository.Retrieve(id);
                return oClima;
            }
        }
    }
}
