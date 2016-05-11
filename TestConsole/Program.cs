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
        //public static string AWSCloudSearchKey = "imbd-movie-s73byoqpg3zr23xxbpczykppmq.us-west-2.cloudsearch.amazonaws.com";
        
        static void Callback(IAsyncResult result)
        {
            string hostName = (string)result.AsyncState;
        }
        static void Main(string[] args)
        {

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
            cloudSearch.Add(movie);
            movie.actors.Add("Ken Davis");
            cloudSearch.Update(movie);            
            cloudSearch.Delete(movie);


            //SEARCH
            var searchQuery = new SearchQuery<Movie> { Keyword = "star wars" };
            var found = cloudSearch.Search(searchQuery);            
        }
    }
}
