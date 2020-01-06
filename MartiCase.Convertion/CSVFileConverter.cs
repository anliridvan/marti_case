using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MartiCase.Converters.Model;

namespace MartiCase.Converters
{
    public class CSVFileConverter : IFileConverter
    {
        public Task<byte[]> AddressInfoByteArray(List<AddressInfo> input)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CityName,CityCode,DistrictName,ZipCode");

            foreach (var item in input)
            {
                sb.AppendLine($"{item.CityName},{item.CityCode},{item.DistrictName},{item.ZipCode}");
            }

            return Task.FromResult(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        public Task<List<AddressInfo>> ByteArrayToAddressInfo(byte[] input)
        {
            List<AddressInfo> addressInfos = new List<AddressInfo>();

            string str = Encoding.UTF8.GetString(input);

            var rows = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < rows.Length; i++)
            {
                var columns = rows[i].Split(',');

                var addressInfo = new AddressInfo();

                addressInfo.CityName = columns[0];
                addressInfo.CityCode = columns[1];
                addressInfo.DistrictName = columns[2];
                addressInfo.ZipCode = columns[3];

                addressInfos.Add(addressInfo);
            }

            return Task.FromResult(addressInfos);
        }

        public string GetContentType()
        {
            return "text/csv";
        }
    }
}
