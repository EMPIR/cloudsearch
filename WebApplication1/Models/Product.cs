using AmazingCloudSearch;
using AmazingCloudSearch.Contract;
using AmazingCloudSearch.Query;
using AmazingCloudSearch.Query.Boolean;
using AmazingCloudSearch.Query.Facets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class Gun : SearchDocument
    {
        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("itemno")]
        public string itemno { get; set; }	//Counterpoint Item Number

        [JsonProperty("style")]
        public string style { get; set; }//Previous Celerant Item Number

        [JsonProperty("brand")]
        public string brand { get; set; }

        [JsonProperty("category")]
        public string category { get; set; }

        [JsonProperty("department")]
        public string department { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("longdescription")]
        public string longdescription { get; set; }

        [JsonProperty("detailed")]
        public string detailed { get; set; }

        [JsonProperty("caliber")]
        public string caliber { get; set; } //facet

        [JsonProperty("action")]
        public string action { get; set; } //facet

        [JsonProperty("barrellength")]
        public string barrellength { get; set; } //facet

        [JsonProperty("color")]
        public string color { get; set; } //facet

        [JsonProperty("finish")]
        public string finish { get; set; } //facet

        [JsonProperty("rating")]
        public string rating { get; set; } //facet


        [JsonProperty("lastupdated")]
        public DateTime lastupdated { get; set; }


    }
}