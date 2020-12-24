using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIRESTHLELIKRAM.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Headword
    {
        public string text { get; set; }
        public string pos { get; set; }
    }

    public class Sens
    {
        public string id { get; set; }
        public string definition { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string language { get; set; }
        public Headword headword { get; set; }
        public List<Sens> senses { get; set; }
    }

    public class DataSet
    {
        public int n_results { get; set; }
        public int results_per_page { get; set; }
        public List<Result> results { get; set; }
    }


}