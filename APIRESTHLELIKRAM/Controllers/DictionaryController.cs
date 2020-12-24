using APIRESTHLELIKRAM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIRESTHLELIKRAM.Controllers
{
    public class DictionaryController : ApiController
       
    {
        // GET api/Dictionary
        DataSet dataSet;
        
        //Initialiser les fichiers Json
        string pathEN = AppContext.BaseDirectory + @"DataSet\dataset_en.json";
        string pathFR = AppContext.BaseDirectory + @"DataSet\dataset_fr.json"; 

        [HttpGet]
        [Route("api/Dictionary/Get/{id}/{language}")]
        //id : indice page entré
        //language : langue sélectionée
        public IHttpActionResult Get(int id, string language)
        {
            //Contient la liste des mots retounrnées
            List<string> listOfWords = new List<string>();

            //Tester la langue entrée par l'utilisateur
            if (language.ToLower() == "en")
            {
                //Récupérer le contenu de fichier JSON
                string readText = File.ReadAllText(pathEN);
                dataSet = JsonConvert.DeserializeObject<DataSet>(readText);
            }

            else if (language.ToLower() == "fr")
            {
                string readText = File.ReadAllText(pathFR);
                dataSet = JsonConvert.DeserializeObject<DataSet>(readText);
            }
            else
            {
                //Langue hors liste
                listOfWords.Add("Language Not Found, choose Fr or En");
                return Json(listOfWords);
            }

            //Calculer le nombre de page par DataSet (Json)
            int nbPage = dataSet.results.Count() % dataSet.results_per_page != 0 ? 
                (dataSet.results.Count() / dataSet.results_per_page) + 1  :
                dataSet.results.Count() / dataSet.results_per_page;

            //indice page <= 0
            if(id <= 0)
            {
                listOfWords.Add("Page start with 1");
                return Json(listOfWords);

            }
            //indice page supérieur au nombre de page maximale
            if (id > nbPage)
            {
                listOfWords.Add("You have exceeded the maximum number of pages : " + nbPage);
                return Json(listOfWords);
            }

            int rangeMin = (id - 1) * dataSet.results_per_page;
            //indice limit de recherche
            int rangeMax = 0;
            int x = dataSet.results.Count() % dataSet.results_per_page;
            if ( x == 0)
            {
                rangeMax = (id * dataSet.results_per_page) - 1;
            }
            else
            {
                if(id < nbPage)
                    rangeMax = (id * dataSet.results_per_page) - 1;
                else
                    rangeMax = x - 1;
            }

            //chercher dans la range (offset,limit)
            foreach (Result res in dataSet.results.GetRange(rangeMin, rangeMax))
            {
                listOfWords.Add(res.headword.text);
            }
            //return Json(dataSet.results.GetRange((id - 1) * dataSet.results_per_page, rangeMax));
            return Json(listOfWords.OrderBy(a => a).Take(10));
            
        }
        

        
    }
}
