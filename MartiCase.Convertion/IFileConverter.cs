using MartiCase.Converters.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MartiCase.Converters
{
    public interface IFileConverter
    {
        Task<List<AddressInfo>> ByteArrayToAddressInfo(byte[] input);
        Task<byte[]> AddressInfoByteArray(List<AddressInfo> input);
        string GetContentType();
    }
}
