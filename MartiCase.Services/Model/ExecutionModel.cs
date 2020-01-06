using System;
using System.Collections.Generic;
using System.Text;

namespace MartiCase.Converters.Model
{
    public class ExecutionModel
    {
        public byte[] File { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string RequestedFormat { get; set; }
        public string SortOrder { get; set; } // asc desc
        public string SortProperty { get; set; }
        public string FilterValue { get; set; }
        public string FilterKey { get; set; }
    }
}
