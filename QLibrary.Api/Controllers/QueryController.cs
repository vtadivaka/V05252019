using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EntityData;
using DataLibrary;
using System.Web.Http.Cors;


namespace QLibraryApi.Controllers
{
    [EnableCors(origins: "http://localhost:59053", headers: "*", methods: "*")]
    public class QueryController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Query> Get()
        {
            DataModel ObjModel = new DataModel();
            return ObjModel.GetAllQueries();
        }

        // GET api/<controller>/5
        public Query Get(int id)
        {
            DataModel ObjModel = new DataModel();
            return ObjModel.GetQueryByQueryId(id);
        }

        // POST api/<controller>
        public int Post(Query query)
        {
            DataModel ObjModel = new DataModel();
            return ObjModel.InsertQuery(query);
        }

        // PUT api/<controller>/5
        public int Put(Query query)
        {
            DataModel ObjModel = new DataModel();
            return ObjModel.UpdateQueryByQueryId(query);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}