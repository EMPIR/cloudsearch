using AmazingCloudSearch;
using AmazingCloudSearch.Contract;
using AmazingCloudSearch.Query;
using AmazingCloudSearch.Query.Boolean;
using AmazingCloudSearch.Query.Facets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WebApplication1.Models
{
    public class InventoryItem : SearchDocument
    {
        [JsonProperty("addl_descr_1")]
        public string addl_descr_1 { get; set; }

        [JsonProperty("descr")]
        public string descr { get; set; }

        [JsonProperty("descr_upr")]
        public string descr_upr { get; set; }

        [JsonProperty("item_no")]
        public int item_no { get; set; }

        [JsonProperty("long_descr")]
        public string long_descr { get; set; }

        [JsonProperty("user_action")]
        public string user_action { get; set; }

        [JsonProperty("user_caliber_gauge")]
        public string user_caliber_gauge { get; set; }

        [JsonProperty("user_is_firearm")]
        public string user_is_firearm { get; set; }

        [JsonProperty("user_manufacturer")]
        public string user_manufacturer { get; set; }

        [JsonProperty("user_model")]
        public string user_model { get; set; }
    }

    public class CloudSearchService
    {
        public static AmazingCloudSearch.Contract.Result.SearchResult<InventoryItem> Query(string keyword, string gauge, string useraction, string manufacturer, bool facets, int start)
        {
            string awsAccessKey = WebConfigurationManager.AppSettings["AWSCloudSearchKey"];

            var cloudSearch = new CloudSearch<InventoryItem>(awsAccessKey, "2013-01-01");
            var searchQuery = new SearchQuery<InventoryItem>();
            List<Facet> liFacet = null;
            if(facets)
            {
                liFacet = new List<Facet>();
                liFacet.Add(new Facet { Name = "user_caliber_gauge" });
                liFacet.Add(new Facet { Name = "user_action" });
                liFacet.Add(new Facet { Name = "user_is_firearm" });
            }
            StringBooleanCondition gaugeCondition = null;
            StringBooleanCondition actionCondition = null;
            StringBooleanCondition manufacturerCondition = null;

            BooleanQuery bQuery = null;
            //caliber action brand capacity
            if (!string.IsNullOrEmpty(gauge))
                gaugeCondition = new StringBooleanCondition("user_caliber_gauge", gauge, false);
            if (!string.IsNullOrEmpty(useraction))
                actionCondition = new StringBooleanCondition("user_action", useraction, false);
            if (!string.IsNullOrEmpty(manufacturer))
                manufacturerCondition = new StringBooleanCondition("user_manufacturer", manufacturer, false);

            //var yCondition = new IntBooleanCondition("year");
            //yCondition.SetInterval(2000, 2004);

            if (gaugeCondition != null)
            {
                bQuery = new BooleanQuery();
                bQuery.Conditions.Add(gaugeCondition);
            }
            if (actionCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(actionCondition);
            }
            if (manufacturerCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(manufacturerCondition);
            }

            searchQuery.BooleanQuery = bQuery;
            searchQuery.Keyword = keyword;
            searchQuery.Facets = liFacet;
            searchQuery.Start = start;
            var found = cloudSearch.Search(searchQuery);

            return found;

        }
    }
}