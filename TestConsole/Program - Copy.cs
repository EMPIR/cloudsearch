using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using AmazingCloudSearch;
using AmazingCloudSearch.Contract;
using Newtonsoft.Json;
using AmazingCloudSearch.Query;
using Amazon.CloudSearchDomain.Model;
using Amazon.CloudSearchDomain;
using AmazingCloudSearch.Query.Facets;
using Newtonsoft.Json.Linq;

namespace TestConsole
{
    public class Movie : SearchDocument
    {
        //The JsonProperty is important to declare for doing CRUD and search
        //The actual property name is important for search, and needs to match exactly to the JsonProperty name,
        //e.g. you cannot have property name Title and JsonProperty title.  Both need to be title, one of the index
        //values of the cloudsearch domain
        [JsonProperty("title")]
        public string title { get; set; }

        
        [JsonProperty("directors")]
        public List<string> directors { get; set; }

        [JsonProperty("actors")]
        public List<string> actors { get; set; }

        [JsonProperty("year")]
        public int year { get; set; }
    }
    class Program
    {
        public static void Results(string query, string caliber, string action, string brand, string capacity)
        {
            SearchRequest oReq = new SearchRequest();
            SearchResponse oRes = new SearchResponse();
            AmazonCloudSearchDomainConfig oConfig = new AmazonCloudSearchDomainConfig();
            AmazonCloudSearchDomainClient oClient;

            //build search string
            string queryFilter = "";

            if (caliber != "" && action != "" && brand != "" && capacity != "")
            {

            }
            if (string.IsNullOrEmpty(query))
            {
                query = "KyGunCo";
            }

            //DG--KyGunCo.Data.KYGContext _db = new Data.KYGContext();



            oConfig.RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1;
            //
            
            oConfig.ServiceURL = WebConfigurationManager.AppSettings["AWSCloudSearchKeyCustom"];
            oClient = new AmazonCloudSearchDomainClient(oConfig);
            oReq.Query = query;

            
            //DG The following block was originally commented out
            //oReq.Facet = "{\"user_action\":{\"buckets\":[\"BULK AMMO\", \"CF RIFLE AMMO\", \"GUN CASES\"]}}";
            
            
            //This is an example of requesting specific buckets for each facet
            oReq.Facet = "{"+
                           "\"user_action\":{\"buckets\":[\"BULK AMMO\", \"CF RIFLE AMMO\", \"GUN CASES\"]},"+
                           "\"user_is_firearm\":{\"buckets\":[\"Y\", \"N\"]},"+
                           "\"user_caliber_gauge\":{}"
                           +"}";


            //This is an example of requesting all buckets for each facet
            oReq.Facet = "{" +
                           "\"user_action\":{}," +
                           "\"user_is_firearm\":{}," +                           
                           "\"user_caliber_gauge\":{}"
                           + "}";
            //oReq.Facet = "{{\"buckets\"}}"; //list all calibers for search term
            //oReq.Facet = "{\"caliber\":{buckets:[\"9mm\"]}} + {\"typ\":{sort:\"bucket\"}}";
            //oReq.FilterQuery = "( and caliber:'9mm' )";

            //oReq.FilterQuery = "( and caliber:'9mm' typ:'SEMI-AUTO')";
            //oReq.FilterQuery = "";
            //oReq.QueryOptions = "{\"caliber\":[\"9mm\"]}";
              
            //DG end comment block
            
            oReq.QueryParser = QueryParser.Simple;

            //DG The following block was originally commented out 
            //oReq.QueryOptions = "{defaultOperator: 'or', fields:['user_manufacturer^5','long_descr']}";
            //DG end comment block 
            
            oReq.Size = 50;
            oReq.Sort = "user_is_firearm desc";
            oRes = oClient.Search(oReq);

            //DG--List<KyGunCo.Data.Models.Product> results = new List<Data.Models.Product>();

            string message = "";
            /*
            user_model
            item_no
            descr_upr
            descr
            long_descr
            user_action
            user_is_firearm
            user_manufacturer
            addl_descr_1
             * 
             */
            //foreach (var fac in oRes.Facets)
            //{
            //    message = message + "facet: " + fac.Key + " = " + fac.Value + "<br />";
            //    foreach (var cal in fac.Value.Buckets)
            //    {
            //        message = message + "<br />  " + cal.Value + "(" + cal.Count + ") <br />";
            //    }
            //}



            //DG--
            /*foreach (Hit oHit in oRes.Hits.Hit)
            {
                //message = message + oHit.Fields["style"][0].ToString() + "<br />";

                try
                {
                    string style = oHit.Fields["addl_descr_1"][0].ToString().ToUpper();
                    var prod = _db.Products.FirstOrDefault(m => m.Style.ToUpper() == style && m.Enabled == true);
                    if (prod.Images.Any())
                    {
                        results.Add(prod);
                    }

                }
                catch
                {

                }

            }--DG*/

            oClient.Dispose();

            //DG--
            /*if (results.Count > 0)
            {
                return View("Results", results.OrderByDescending(m => m.IsFirearm).ToList());
            }
            else
            {
                return View("Results", results.ToList());
            }*/



        }
        public static string json = @"{
	'status': {
		'rid': 'wv/qscoqtxgKJAq3',
		'time-ms': 3
	},
	'hits': {
		'found': 9,
		'start': 0,
		'hit': null
	},
	'facets': {
		'genres': {
			'buckets': [{
				'value': 'Adventure',
				'count': 9
			},
			{
				'value': 'Action',
				'count': 8
			},
			{
				'value': 'Sci-Fi',
				'count': 8
			},
			{
				'value': 'Fantasy',
				'count': 6
			},
			{
				'value': 'Comedy',
				'count': 1
			},
			{
				'value': 'Drama',
				'count': 1
			}]
		}
	}
}";

        public class myBucket
        {
            public string value { get; set; }
            public int count { get; set; }
        }
        public class myFacet
        {
            public string name { get; set; }
            public List<myBucket> buckets { get; set; }
            public myFacet()
            {
                buckets = new List<myBucket>();
            }
        }
        static void Main(string[] args)
        {
            /*List<myFacet> facets = new List<myFacet>();
            dynamic jsonArray = JsonConvert.DeserializeObject(json);
            dynamic facetsArray = jsonArray.facets;
            foreach(dynamic facet in facetsArray)
            {
                myFacet facetObj = new myFacet();
                facetObj.name = facet.Name;

                
                foreach(dynamic bucket in facet.Value.buckets)
                {
                    myBucket bucketObj = new myBucket();
                    bucketObj.count = bucket.count;
                    bucketObj.value = bucket.value;
                    facetObj.buckets.Add(bucketObj);
                    int debug = 0;
                    string value = bucket.value;
                    int count = bucket.count;
                }
                facets.Add(facetObj);
            }

            return;*/

            //Results("9mm", string.Empty, string.Empty, string.Empty, string.Empty);
            //return;

            string awsAccessKey = WebConfigurationManager.AppSettings["AWSCloudSearchKey"];

            var cloudSearch = new CloudSearch<Movie>(awsAccessKey, "2013-01-01");
            var movie = new Movie
            {
                id = "fjuhewdijsdjoi",//Guid.NewGuid().ToString(),
                
                title = "Redneck Zombies",
                directors = new List<string>
                {
                    "Pericles Lewnes"
                },
                actors = new List<string>
                {
                    "Steve Sooy",
                    "Anthony M. Carr"
                },
                
                year = 1989,
            };
            
            
            
            
            

            //CRUD 
            //cloudSearch.Add(movie);
            //movie.actors.Add("Ken Davis");
            //cloudSearch.Update(movie);            
            //cloudSearch.Delete(movie);


            //SEARCH

            var searchQuery = new SearchQuery<Movie> { Keyword = "star wars" };
            var found = cloudSearch.Search(searchQuery);



            var genreFacet = new Facet { Name = "genres" };
            var liFacet = new List<Facet> { genreFacet };

            searchQuery = new SearchQuery<Movie> { Keyword = "star wars", Facets = liFacet };
            found = cloudSearch.Search(searchQuery);
        }
    }
}
