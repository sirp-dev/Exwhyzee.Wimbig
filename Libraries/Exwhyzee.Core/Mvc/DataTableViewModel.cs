using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Mvc
{

    [Serializable()]
    public class DataTableParameter
    {
        public int draw { get; set; }
        public int length { get; set; }
        public int start { get; set; }
        public List<columm> columns { get; set; }
    }
    [Serializable()]
    public class columm
    {
        public string data { get; set; }
        public string name { get; set; }
        public Boolean searchable { get; set; }
        public Boolean orderable { get; set; }
        public searchValue Search { get; set; }
    }
    [Serializable()]
    public class searchValue
    {
        public string value { get; set; }
        public Boolean regex { get; set; }
    }
}
