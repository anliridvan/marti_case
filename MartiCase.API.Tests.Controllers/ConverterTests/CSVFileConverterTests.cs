
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using MartiCase.Converters;

namespace MartiCase.API.Tests.ConverterTests
{
    [TestClass]
    public class CSVFileConverterTests 
    {
        [TestMethod]
        public async Task ExecuteCSVFile_Nominal_OK()
        {
            var converter = new CSVFileConverter();

            var bytes = System.IO.File.ReadAllBytes("samples\\sample_data.csv");

            var result = await converter.ByteArrayToAddressInfo(bytes);
            
            Assert.IsTrue(result.Count > 0);
        }
    }
}
