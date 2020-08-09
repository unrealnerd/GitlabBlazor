using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Org.Gitlab.Models
{
    public class PageInfo
    {
        public int PerPage { get; set; }
        public int Page { get; set; }
        public int NextPage { get; set; }
        public int PrevPage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public string Route { get; set; }
        
    }

    
}