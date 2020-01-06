using System;
using System.Collections.Generic;
using System.Text;

namespace MartiCase.Converters.Model
{
    public class ResultModel
    {
        public byte[] File { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
    }

}
