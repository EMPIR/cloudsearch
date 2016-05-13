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

namespace TestConsole
{

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
        
       
        static void Main(string[] args)
        {
           
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
