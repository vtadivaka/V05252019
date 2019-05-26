using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EntityData;
using DataLibrary;

namespace QLibraryApi.Controllers
{
    public class SectionController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Section> Get()
        {
            DataModel ObjModel = new DataModel();
            return ObjModel.GetAllSections();
        }

        // GET api/<controller>/5
        public Section Get(int id)
        {
            DataModel ObjModel = new DataModel();
            return ObjModel.GetSectionBySectionId(id);
        }

        // POST api/<controller>
        public int Post(Section section)
        {
            DataModel ObjModel = new DataModel();
            return ObjModel.InsertSection(section);
        }

        // PUT api/<controller>/5
        public int Put(Section section)
        {
            DataModel ObjModel = new DataModel();
            return ObjModel.UpdateSectionBySectionId(section);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}