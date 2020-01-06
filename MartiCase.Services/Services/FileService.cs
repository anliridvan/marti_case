using AutoMapper;
using MartiCase.API.Common.Settings;
using MartiCase.Services.Contracts;
using MartiCase.Converters.Model;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using MartiCase.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MartiCase.Services
{
    public class FileService : IFileService
    {
        private AppSettings _settings;
        private readonly IMapper _mapper;

        public FileService(IOptions<AppSettings> settings, IMapper mapper)
        {
            _settings = settings?.Value;
            _mapper = mapper;
        }

        public async Task<ResultModel> ExecuteAsync(ExecutionModel input)
        {
            List<AddressInfo> processData;
            ResultModel output = new ResultModel { Extension = input.RequestedFormat, Name = input.Name };

            #region plugIn Structure
            var outputConverterSettings = _settings.Converters.FirstOrDefault(p => p.Type == input.RequestedFormat);
            var inputConverterSettings = _settings.Converters.FirstOrDefault(p => p.Type == input.Extension);
            IFileConverter outputConverter = (IFileConverter)Activator.CreateInstance(outputConverterSettings.Assembly,
                outputConverterSettings.Definition).Unwrap();
            IFileConverter inputConverter = (IFileConverter)Activator.CreateInstance(inputConverterSettings.Assembly,
                inputConverterSettings.Definition).Unwrap();
            #endregion

            processData = await inputConverter.ByteArrayToAddressInfo(input.File);

            #region filtering
            if (!string.IsNullOrEmpty(input.FilterKey))
            {
                var param = Expression.Parameter(typeof(AddressInfo), "p");
                var filterExp = Expression.Lambda<Func<AddressInfo, bool>>(
                    Expression.Equal(
                        Expression.Property(param, input.FilterKey),
                        Expression.Constant(input.FilterValue)
                    ),
                    param
                );

                processData = processData.Where(filterExp.Compile()).ToList();
            }
            #endregion

            #region ordering - with thenBy
            if (!string.IsNullOrEmpty(input.SortProperty))
            {
                int orderSequance = 0;
                foreach (var item in input.SortProperty.Split(','))
                {
                    processData = OrderByNull<AddressInfo>(processData.AsQueryable(),
                        item, input.SortOrder.ToLower().Equals("desc"),
                        orderSequance != 0);
                }
            }
            #endregion

            output.File = await outputConverter.AddressInfoByteArray(processData);
            output.ContentType = outputConverter.GetContentType();
            return output;

        }

        #region Ordering Expression - Dynamic Linq Can be Replaced With - https://github.com/StefH/System.Linq.Dynamic.Core
        public List<TEntity> OrderByNull<TEntity>(IQueryable<TEntity> source,
            string orderByProperty, bool desc, bool thenBy = false)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            if (thenBy)
                command = desc ? "ThenByDescending" : "ThenBy";
            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");
            var target = Expression.Constant(null, type);
            var equalsMethod = Expression.Call(Expression.Property(parameter, orderByProperty), "Equals", null, target);
            var orderByExpression = Expression.Lambda(equalsMethod, parameter);
            var resultExpression = Expression.Call(
                typeof(Queryable),
                command,
                new Type[] { type, orderByExpression.ReturnType },
                source.Expression,
                Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression).ToList();
        }
        #endregion

    }
}
