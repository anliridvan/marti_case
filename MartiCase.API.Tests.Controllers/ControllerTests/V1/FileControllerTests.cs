
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using MartiCase.Converters;
using MartiCase.API.Tests.Controllers.Definitions;
using MartiCase.Services;
using Microsoft.Extensions.DependencyInjection;
using MartiCase.Services.Contracts;
using AutoMapper;
using Microsoft.Extensions.Options;
using MartiCase.API.Common.Settings;
using Microsoft.Extensions.Configuration;
using MartiCase.API.Controllers.V1;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace MartiCase.API.Tests.ServiceTests
{
    [TestClass]
    public class FileControllerTests : TestBase
    {
        FileController _controller;

        public FileControllerTests() : base()
        {
            var serviceProvider = _services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<FileController>();

            var service = new FileService(Options.Create(_configurationRoot.GetSection("AppSettings").Get<AppSettings>()), mapper);

            _controller = new FileController(service, mapper, logger);

        }

        [TestMethod]
        public async Task TestCase1_Nominal_OK()
        {
            var file = new MemoryStream(Encoding.UTF8.GetBytes(System.IO.File.ReadAllText("samples\\sample_data.csv")));
            
            var output = await _controller.Execute(
             new DataContracts.Requests.FileRequest
             {
                 FileToUpload = new FormFile(file, 0, file.Length, "data", "sample_data.csv"),
                 FilterKey = "CityName",
                 FilterValue = "Antalya",
                 RequestedFormat = "xml"
             }
             );

            Assert.IsTrue(output.FileDownloadName.Contains("xml"));
        }

        [TestMethod]
        public async Task TestCase2_Nominal_OK()
        {
            var file = new MemoryStream(Encoding.UTF8.GetBytes(System.IO.File.ReadAllText("samples\\sample_data.csv")));

            var output = await _controller.Execute(
             new DataContracts.Requests.FileRequest
             {
                 FileToUpload = new FormFile(file, 0, file.Length, "data", "sample_data.csv"),
                 SortOrder = "asc",
                 SortProperty = "CityName,DistrictName",
                 RequestedFormat = "csv"
             }
             );

            Assert.IsTrue(output.FileDownloadName.Contains("csv"));
        }

        [TestMethod]
        public async Task TestCase3_Nominal_OK()
        {
            var file = new MemoryStream(Encoding.UTF8.GetBytes(System.IO.File.ReadAllText("samples\\sample_data.xml")));

            var output = await _controller.Execute(
             new DataContracts.Requests.FileRequest
             {
                 FileToUpload = new FormFile(file, 0, file.Length, "data", "sample_data.xml"),
                 FilterKey = "CityName",
                 FilterValue = "Ankara",
                 SortOrder = "desc",
                 SortProperty = "ZipCode",
                 RequestedFormat = "csv"
             }
             );

            Assert.IsTrue(output.FileDownloadName.Contains("csv"));
        }
    }
}
