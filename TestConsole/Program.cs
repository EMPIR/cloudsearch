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
using AmazingCloudSearch.Query.Boolean;
using System.Net;
using System.Net.Cache;
using System.IO;
using System.Text.RegularExpressions;

namespace TestConsole
{
    public class GunFromAPI
    {
        public string Title { get; set; }
        public string ItemNo { get; set; }	//Counterpoint Item Number
        public string Style { get; set; }//Previous Celerant Item Number
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string Detailed { get; set; }
        public string Caliber { get; set; } //facet
        public string Action { get; set; } //facet
        public string BarrelLength { get; set; } //facet
        public string Color { get; set; } //facet
        public string Finish { get; set; } //facet
        public string Rating { get; set; } //facet
        public DateTime LastUpdated { get; set; }

        private static Regex _invalidChars = new Regex(
@"(?<![\uD800-\uDBFF])[\uDC00-\uDFFF]|[\uD800-\uDBFF](?![\uDC00-\uDFFF])|[\x00-\x08\x0B\x0C\x0E-\x1F\x7F-\x9F\uFEFF\uFFFE\uFFFF]",
RegexOptions.Compiled);

        /// <summary>
        /// removes any unusual unicode characters that can't be encoded into XML/JSON
        /// </summary>
        public static string RemoveInvalidChars(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return _invalidChars.Replace(text, "");
        }

        public static string cleanseData(string s)
        {
            string ret = string.Empty;
            // filters control characters but allows only properly-formed surrogate sequences

            ret = RemoveInvalidChars(s);

            return ret;
        }

    }

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





        public static Gun SetGun(GunFromAPI g)
        {
            Gun ret = new Gun();
            ret.title = g.Title != null ? g.Title : "Unknown";
            ret.itemno = g.ItemNo != null ? g.ItemNo : "Unknown";
            ret.style = g.Style != null ? g.Style : "Unknown";
            ret.brand = g.Brand != null ? g.Brand : "Unknown";
            ret.category = g.Category != null ? g.Category : "Unknown";
            ret.department = g.Department != null ? g.Department : "Unknown";
            ret.type = g.Type != null ? g.Type : "Unknown";
            ret.description = g.Description != null ? g.Description : "Unknown";
            ret.longdescription = g.LongDescription != null ? g.LongDescription : "Unknown";
            ret.detailed = g.Detailed != null ? g.Detailed : "Unknown";

            ret.detailed = GunFromAPI.cleanseData(ret.detailed);

            ret.caliber = g.Caliber != null ? g.Caliber : "Unknown";
            ret.action = g.Action != null ? g.Action : "Unknown";
            ret.barrellength = g.BarrelLength != null ? g.BarrelLength : "Unknown";
            ret.color = g.Color != null ? g.Color : "Unknown";
            ret.finish = g.Finish != null ? g.Finish : "Unknown";
            ret.rating = g.Rating;
            ret.id = g.ItemNo;
            ret.lastupdated = g.LastUpdated.ToUniversalTime();
            return ret;
        }
        public static List<Gun> FromGunAPI(List<GunFromAPI> gList)
        {
            List<Gun> ret = new List<Gun>();
            foreach (GunFromAPI gun in gList)
            {
                ret.Add(Gun.SetGun(gun));

            }
            return ret;
        }

    }
    //The JsonProperty is important to declare for doing CRUD and search
    //The actual property name is important for search, and needs to match exactly to the JsonProperty name,
    //e.g. you cannot have property name Title and JsonProperty title.  Both need to be title, one of the index
    //values of the cloudsearch domain
    public class InventoryItem : SearchDocument
    {
        [JsonProperty("addl_descr_1")]
        public string addl_descr_1 { get; set; }

        [JsonProperty("descr")]
        public string descr	 { get; set; }

        [JsonProperty("descr_upr")]
        public string descr_upr	{ get; set; }

        [JsonProperty("item_no")]        
        public int item_no	{ get; set; }

        [JsonProperty("long_descr")]   
        public string long_descr	{ get; set; }

        [JsonProperty("user_action")]
        public string user_action	{ get; set; }

        [JsonProperty("user_caliber_gauge")]        
        public string user_caliber_gauge	{ get; set; }

        [JsonProperty("user_is_firearm")]                
        public string user_is_firearm	{ get; set; }

        [JsonProperty("user_manufacturer")]                
        public string user_manufacturer{ get; set; }

        [JsonProperty("user_model")]                        
        public string user_model { get; set; }
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
            
            oConfig.ServiceURL = "http://search-" + WebConfigurationManager.AppSettings["AWSCloudSearchKey"];
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

        public static string DoWebRequest(string address, string method)
        {
            return DoWebRequest(address, method, string.Empty);
        }

        public static string DoWebRequest(string address, string method, string body)
        {
            var request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = method; 
            request.ContentType = "application/json; charset=utf-8";
            //request.PreAuthenticate = true;

            if (method == "POST")
            {
                if (!string.IsNullOrEmpty(body))
                {
                    var requestBody = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = requestBody.Length;
                    request.ContentType = "application/json";
                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(requestBody, 0, requestBody.Length);
                    }
                }
                else
                {
                    request.ContentLength = 0;
                }
            }

            request.Timeout = 15000;
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

            string output = string.Empty;
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                    {
                        output = stream.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    using (var stream = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        output = stream.ReadToEnd();
                    }
                }
                else if (ex.Status == WebExceptionStatus.Timeout)
                {
                    output = "Request timeout is expired.";
                }
            }
            //Console.WriteLine(output);

            return output;
        }

        public static Gun GetGunByItemID(string id, List<Gun>guns)
        {
            foreach(Gun g in guns)
            {
                if (g.itemno == id)
                    return g;
            }
            return null;
        }
        

        public static void CallAPI()
        {
            DateTime lastUpdate = DateTime.Now.AddDays(-1);
            string result = DoWebRequest("http://kygunco-api.us-east-1.elasticbeanstalk.com/api/cloudsearch", "GET");

            List<GunFromAPI>guns = JsonConvert.DeserializeObject<List<GunFromAPI>>(result);
           
            List<Gun> gunList = Gun.FromGunAPI(guns);
            string awsAccessKey = WebConfigurationManager.AppSettings["AWSCloudSearchKey"];
            var cloudSearch = new CloudSearch<Gun>(awsAccessKey, "2013-01-01");
            /*int i = 1;
            Console.WriteLine("Start Time " + DateTime.Now.ToLongTimeString());
            List<Gun> updateGunList = new List<Gun>();
            foreach(Gun gun in gunList)
            {
                var searchQuery = new SearchQuery<Gun> { Keyword = gun.itemno };

                var found = cloudSearch.Search(searchQuery);
                if (found.hits.hit.Count == 0)
                    cloudSearch.Add(gun);
                else
                {
                    //cloudSearch.Update(gun);
                    updateGunList.Add(gun);
                }
                if(updateGunList.Count > 4999)
                {
                    Console.WriteLine("Performing Batch Update");
                    cloudSearch.Add(updateGunList);
                    Console.WriteLine("Done");
                    updateGunList.Clear();

                }

                Console.Write("\rGun " + i++ + " of " + gunList.Count);
            }
            Console.WriteLine("Performing Final Batch Update");
            cloudSearch.Add(updateGunList);
            Console.WriteLine("Done");
            updateGunList.Clear();
            Console.WriteLine("End Time " + DateTime.Now.ToLongTimeString());
            */
            Console.WriteLine("Start Time " + DateTime.Now.ToLongTimeString());
            var searchQuery = new SearchQuery<Gun>();
            StringBooleanCondition itemnoCondition = null;
            itemnoCondition = new StringBooleanCondition("itemno", Guid.NewGuid().ToString(), false);
            BooleanQuery bQuery = new BooleanQuery();
            bQuery.Conditions.Add(itemnoCondition);
            searchQuery.BooleanQuery = bQuery;

            var found = cloudSearch.Search(searchQuery);
            int totalRecords = found.hits.found;
            int count = 0;

            while(count < totalRecords)
            {
                
                foreach (dynamic hit in found.hits.hit)
                {
                    Gun g = hit;
                    Console.Write("\r"+g.itemno);
                }
                searchQuery.Start += 10;
                found = cloudSearch.Search(searchQuery);
                count += 10;
            }

            Console.WriteLine("End Time " + DateTime.Now.ToLongTimeString());



            /*32274
32376
35584
58047
70487
94964
94994
96229
96583*/
            /*string[] itemno = { "32274", "32376", "35584", "58047", "70487", "94964", "94994", "96229", "96583" };
            foreach(string io in itemno)
            {

                Gun gun = GetGunByItemID(io, gunList);
                var searchQuery = new SearchQuery<Gun> { Keyword = io };

                var found = cloudSearch.Search(searchQuery);
                if (found.hits.hit.Count == 0)
                    cloudSearch.Add(gun);
                else
                    cloudSearch.Update(gun);
                Console.WriteLine("Gun " + i++ + " of " + itemno.Length);
            }*/

        }
        
       
        static void Main(string[] args)
        {
            CallAPI();
            return;
            //Example of using a similar format as source code given.  This uses the AWS cloudsearch library directly
            Results("9mm", string.Empty, string.Empty, string.Empty, string.Empty);
            //return;

            string awsAccessKey = WebConfigurationManager.AppSettings["AWSCloudSearchKey"];

            var cloudSearch = new CloudSearch<InventoryItem>(awsAccessKey, "2013-01-01");

            InventoryItem item = new InventoryItem();
            item.addl_descr_1 = "DTG-XDS-9MM";
            item.descr = "DTG XDS 9MM";
            item.descr_upr = "DTG XDS 9MM";
            item.id = Guid.NewGuid().ToString();
            item.item_no = 9999;
            item.long_descr = "DTG XDS 9MM";
            item.user_action = "MANUAL";
            item.user_caliber_gauge = "14MM";
            item.user_is_firearm = "N";
            item.user_manufacturer = "PROVIDENCE";
            item.user_model = "Peabody";
            

            //CRUD 
            cloudSearch.Add(item);
            item.user_model = "Winchester";
            cloudSearch.Update(item);            
            cloudSearch.Delete(item);
            

            //SEARCH
            
            var searchQuery = new SearchQuery<InventoryItem> { Keyword = "9mm" };
            var found = cloudSearch.Search(searchQuery);

            var liFacet = new List<Facet> ();
            liFacet.Add(new Facet { Name = "user_caliber_gauge" });
            liFacet.Add(new Facet { Name = "user_action" });
            liFacet.Add(new Facet { Name = "user_is_firearm" });


            searchQuery = new SearchQuery<InventoryItem> { Keyword = "9mm", Facets = liFacet };
            found = cloudSearch.Search(searchQuery);


            string keyword = "XDS";
            string gauge = "9MM";
            string action = "SEMI AUTO";
            string manufacturer = "";//"SPRINGFIELD";

            StringBooleanCondition gaugeCondition = null;
            StringBooleanCondition actionCondition = null;
            StringBooleanCondition manufacturerCondition = null;

            BooleanQuery bQuery = null;
            //caliber action brand capacity
            if(!string.IsNullOrEmpty(gauge))
                gaugeCondition = new StringBooleanCondition("user_caliber_gauge", "9MM", true);
            if (!string.IsNullOrEmpty(action))
                actionCondition = new StringBooleanCondition("user_action", "SEMI AUTO", true);
            if (!string.IsNullOrEmpty(manufacturer))            
                manufacturerCondition = new StringBooleanCondition("user_manufacturer", "SPRINGFIELD", false);
            
            //var yCondition = new IntBooleanCondition("year");
            //yCondition.SetInterval(2000, 2004);

            if (gaugeCondition != null)
            {
                bQuery = new BooleanQuery();
                bQuery.Conditions.Add(gaugeCondition);
            }
            if (actionCondition != null)
            {
                if(bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(actionCondition);
            }
            if (manufacturerCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(manufacturerCondition);
            }

            

            searchQuery = new SearchQuery<InventoryItem>();// { Size = 20, BooleanQuery = bQuery };
            searchQuery.BooleanQuery = bQuery;
            searchQuery.Keyword = keyword;
            searchQuery.Facets = liFacet;
            found = cloudSearch.Search(searchQuery);

            
        }
    }
}
