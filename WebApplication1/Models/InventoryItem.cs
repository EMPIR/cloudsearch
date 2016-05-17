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
        public static AmazingCloudSearch.Contract.Result.SearchResult<Gun> Query(string keyword, 
            string action, string barrellength, string brand,
            string caliber, string category, string color,
            string department, string finish, string rating, string type,
            
            bool facets, int start)
        {
            string awsAccessKey = WebConfigurationManager.AppSettings["AWSCloudSearchKey"];

            var cloudSearch = new CloudSearch<Gun>(awsAccessKey, "2013-01-01");
            var searchQuery = new SearchQuery<Gun>();
            List<Facet> liFacet = null;
            if(facets)
            {
                liFacet = new List<Facet>();
                liFacet.Add(new Facet { Name = "action" });
                liFacet.Add(new Facet { Name = "barrellength" });
                liFacet.Add(new Facet { Name = "brand" });
                liFacet.Add(new Facet { Name = "caliber" });
                liFacet.Add(new Facet { Name = "category" });
                liFacet.Add(new Facet { Name = "color" });
                liFacet.Add(new Facet { Name = "department" });
                liFacet.Add(new Facet { Name = "finish" });
                liFacet.Add(new Facet { Name = "rating" });
                liFacet.Add(new Facet { Name = "type" });
            }
            
            StringBooleanCondition actionCondition = null;
            StringBooleanCondition barrellengthCondition = null;
            StringBooleanCondition brandCondition = null;
            StringBooleanCondition caliberCondition = null;
            StringBooleanCondition categoryCondition = null;
            StringBooleanCondition colorCondition = null;
            StringBooleanCondition departmentCondition = null;
            StringBooleanCondition finishCondition = null;
            StringBooleanCondition ratingCondition = null;
            StringBooleanCondition typeCondition = null;
            

            BooleanQuery bQuery = null;
            //caliber action brand capacity
            if (!string.IsNullOrEmpty(action))
                actionCondition = new StringBooleanCondition("action", action, false);
            if (!string.IsNullOrEmpty(barrellength))
                barrellengthCondition = new StringBooleanCondition("barrellength", barrellength, false);
            if (!string.IsNullOrEmpty(brand))
                brandCondition = new StringBooleanCondition("brand", brand, false);
            if (!string.IsNullOrEmpty(caliber))
                caliberCondition = new StringBooleanCondition("caliber", caliber, false);
            if (!string.IsNullOrEmpty(category))
                categoryCondition = new StringBooleanCondition("category", category, false);
            if (!string.IsNullOrEmpty(color))
                colorCondition = new StringBooleanCondition("color", color, false);
            if (!string.IsNullOrEmpty(department))
                departmentCondition = new StringBooleanCondition("department", department, false);
            if (!string.IsNullOrEmpty(finish))
                finishCondition = new StringBooleanCondition("finish", finish, false);
            if (!string.IsNullOrEmpty(rating))
                ratingCondition = new StringBooleanCondition("rating", rating, false);
            if (!string.IsNullOrEmpty(type))
                typeCondition = new StringBooleanCondition("type", type, false);
           

            //var yCondition = new IntBooleanCondition("year");
            //yCondition.SetInterval(2000, 2004);

            if (actionCondition != null)
            {
                bQuery = new BooleanQuery();
                bQuery.Conditions.Add(actionCondition);
            }
            if (barrellengthCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(barrellengthCondition);
            }
            if (brandCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(brandCondition);
            }
            if (caliberCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(caliberCondition);
            }
            if (categoryCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(categoryCondition);
            }
            if (colorCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(colorCondition);
            }
            if (departmentCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(departmentCondition);
            }
            if (finishCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(finishCondition);
            }
            if (ratingCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(ratingCondition);
            }
            if (typeCondition != null)
            {
                if (bQuery == null)
                    bQuery = new BooleanQuery();
                bQuery.Conditions.Add(typeCondition);
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