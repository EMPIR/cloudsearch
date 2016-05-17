using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModels
{
    public class QueryForm
    {
        
        public string Keyword {get;set;} 
        public string mAction {get;set;}
        public string Barrellength {get;set;}
        public string Brand {get;set;}
        public string Caliber {get;set;}
        public string Category {get;set;}
        public string Color {get;set;}
        public string Department {get;set;} 
        public string Finish {get;set;}
        public string Rating {get;set;}
        public string Type { get; set; }


        public int Start { get; set; }
        public AmazingCloudSearch.Contract.Result.SearchResult<Gun> SearchResults { get; set; }
    }
}