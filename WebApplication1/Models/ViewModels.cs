using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModels
{
    public class QueryForm
    {
        public string Keyword { get; set; }
        public string Gauge { get; set; }
        public string UserAction { get; set; }
        public string Manufacturer { get; set; }
        public int Start { get; set; }
        public AmazingCloudSearch.Contract.Result.SearchResult<InventoryItem> SearchResults { get; set; }
    }
}