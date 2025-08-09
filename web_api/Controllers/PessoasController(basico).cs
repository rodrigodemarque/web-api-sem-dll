//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;

//namespace Web_api.Controllers
//{
//    public class PessoasController : ApiController
//    {
//        // GET: api/Pessoas
//        public IEnumerable<Models.Pessoa> Get()
//        {
//            Models.Pessoa p1 = new Models.Pessoa("João",25);

//            Models.Pessoa p2 = new Models.Pessoa("Zeca",42);

//            Models.Pessoa p3 = new Models.Pessoa("Digo",43);

//            return new Models.Pessoa[] { p1, p2, p3 };
//        }

//        // GET: api/Pessoas/5
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST: api/Pessoas
//        public void Post([FromBody]string value)
//        {
//        }

//        // PUT: api/Pessoas/5
//        public void Put(int id, [FromBody]string value)
//        {
//        }

//        // DELETE: api/Pessoas/5
//        public void Delete(int id)
//        {
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace web_api.Controllers
{
    public class PessoasController : ApiController
    {
        private static List<Models.Pessoa> pessoas = new List<Models.Pessoa>();

        public PessoasController()
        {
        }

        // GET: api/Pessoas
        public IEnumerable<Models.Pessoa> Get()
        {
            return pessoas;
        }

        // GET: api/Pessoas/5
        public Models.Pessoa Get(int id)
        {
            foreach (var item in pessoas)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            return null;
        }

        // POST: api/Pessoas
        public void Post([FromBody] Models.Pessoa pessoa)
        {
            pessoas.Add(pessoa);
        }

        // PUT: api/Pessoas/5
        public void Put(int id, [FromBody] Models.Pessoa pessoa)
        {
            foreach (var item in pessoas)
            {
                if (item.Id == id)
                {
                    item.Nome = pessoa.Nome;
                    item.Idade = pessoa.Idade;
                    break;
                }
            }
        }

        // DELETE: api/Pessoas/5
        public void Delete(int id)
        {
            foreach (var item in pessoas)
            {
                if (item.Id == id)
                {
                    pessoas.Remove(item);
                    break;
                }
            }
        }
    }
}
