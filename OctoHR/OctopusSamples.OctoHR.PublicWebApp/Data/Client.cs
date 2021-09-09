using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctopusSamples.OctoHR.PublicWebApp.Data
{
    public class Client
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ClientDatabase { get; set; }
    }
}
