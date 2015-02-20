using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
namespace CocktionMVC.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "sdfsdf";
        }

        [Authorize]
        [HttpPost]
        public string Post([FromBody]Customer customer)
        {
            //Метод, который принимает ответ типа кастомер и 
            //отправляет строчку
            //данные сюда отправляются из вьюшки "Тест"
            Customer cust = customer;
            return "string";
        }

        
        

    }

    public class Customer 
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }
}
