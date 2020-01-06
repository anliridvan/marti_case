using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MartiCase.API.DataContracts;
using MartiCase.API.DataContracts.Requests;
using MartiCase.Services.Contracts;
using System;
using System.Threading.Tasks;
using S = MartiCase.Converters.Model;
using System.IO;
using MartiCase.Converters.Model;

namespace MartiCase.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/file")]//required for default versioning
    [Route("api/v{version:apiVersion}/file")]
    [ApiController]
    public class FileController : Controller
    {
        private readonly IFileService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<FileController> _logger;

#pragma warning disable CS1591
        public FileController(IFileService service, IMapper mapper, ILogger<FileController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }
#pragma warning restore CS1591


        #region POST
        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="value"></param>
        /// <response code="201">Returns the newly created item.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FileContentResult))]
        public async Task<FileContentResult> Execute([FromBody]FileRequest value)
        {
            _logger.LogDebug($"FileControllers::Post::");

            if (value == null)
                throw new ArgumentNullException("value");

            var extension = Path.GetExtension(value.FileToUpload.FileName).Trim('.');
            var name = Path.GetFileNameWithoutExtension(value.FileToUpload.FileName);
            byte[] file;
            if (value.FileToUpload == null || value.FileToUpload.Length == 0)
                throw new ArgumentNullException("value.FileToUpload");

            using (var ms = new MemoryStream())
            {
                value.FileToUpload.CopyTo(ms);
                file = ms.ToArray();
            }

            var serviceInput = _mapper.Map<ExecutionModel>(value);
            serviceInput.File = file;
            serviceInput.Extension = extension;
            serviceInput.Name = name;

            var data = await _service.ExecuteAsync(serviceInput);

            if (data != null)
                return File(data.File, data.ContentType, data.Name + "." + data.Extension);
            else
                return null;

        }
        #endregion

    }
}
