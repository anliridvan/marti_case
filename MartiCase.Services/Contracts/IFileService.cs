using MartiCase.Converters.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MartiCase.Services.Contracts
{
    public interface IFileService
    {
        Task<ResultModel> ExecuteAsync(ExecutionModel input);
    }
}
