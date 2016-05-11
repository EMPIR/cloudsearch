using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace AmazingCloudSearch.Contract
{
    public class SearchDocument
    {
        public SearchDocument() //constructor
        {
        }

        [ScriptIgnoreAttribute, JsonIgnoreAttribute]        
        public string id { get; set; }
    }
}