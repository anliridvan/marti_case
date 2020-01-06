using Microsoft.AspNetCore.Http;
using System;

namespace MartiCase.API.DataContracts.Requests
{
    public class FileRequest
    {
        public IFormFile FileToUpload { get; set; }
        public string RequestedFormat { get; set; } // asc desc
        public string SortOrder { get; set; } // asc desc
        public string SortProperty { get; set; }
        public string FilterValue { get; set; }
        public string FilterKey { get; set; }
    }
}
