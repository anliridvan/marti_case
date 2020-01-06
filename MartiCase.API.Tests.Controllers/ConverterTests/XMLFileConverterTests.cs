
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using MartiCase.Converters;

namespace MartiCase.API.Tests.ConverterTests
{
    [TestClass]
    public class XMLFileConverterTests 
    {
        [TestMethod]
        public async Task ExecuteXMLFile_Nominal_OK()
        {
            var converter = new XMLFileConverter();

            var bytes = System.IO.File.ReadAllBytes("samples\\sample_data.xml");

            var result = await converter.ByteArrayToAddressInfo(bytes);
            
            Assert.IsTrue(result.Count > 0);
        }
    }
}
