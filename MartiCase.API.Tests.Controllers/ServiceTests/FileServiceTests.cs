
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

namespace MartiCase.API.Tests.ServiceTests
{
    [TestClass]
    public class FileServiceTests : TestBase
    {
        IFileService _fileService;

        public FileServiceTests() : base()
        {
            var serviceProvider = _services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            _fileService = new FileService(Options.Create(_configurationRoot.GetSection("AppSettings").Get<AppSettings>()), mapper);

        }

        [TestMethod]
        public async Task ExecuteCSVToXmlFile_Nominal_OK()
        {
            var output = await _fileService.ExecuteAsync(
                new Converters.Model.ExecutionModel
                {
                    Extension = "csv",
                    File = System.IO.File.ReadAllBytes("samples\\sample_data.csv"),
                    FilterKey = "CityName",
                    FilterValue = "Ankara",
                    Name = "sample_data",
                    RequestedFormat = "xml",
                    SortOrder = "desc",
                    SortProperty = "ZipCode"
                }
                );


            Assert.IsTrue(output.Name != null && output.File != null);
        }

        [TestMethod]
        public async Task ExecuteXMLToCSVFile_Nominal_OK()
        {
            var output = await _fileService.ExecuteAsync(
                new Converters.Model.ExecutionModel
                {
                    Extension = "xml",
                    File = System.IO.File.ReadAllBytes("samples\\sample_data.xml"),
                    FilterKey = "CityName",
                    FilterValue = "Ankara",
                    Name = "sample_data",
                    RequestedFormat = "csv",
                    SortOrder = "desc",
                    SortProperty = "ZipCode"
                }
                );


            Assert.IsTrue(output.Name != null && output.File != null);
        }
    }
}
