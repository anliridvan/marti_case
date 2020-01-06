using MartiCase.Converters.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MartiCase.Converters
{
    public class XMLFileConverter : IFileConverter
    {
        public object XmlReader { get; private set; }

        public Task<byte[]> AddressInfoByteArray(List<AddressInfo> input)
        {
            throw new NotImplementedException();
        }


        public Task<List<AddressInfo>> ByteArrayToAddressInfo(byte[] input)
        {
            List<AddressInfo> addressInfos = null;
            string str = Encoding.UTF8.GetString(input);
            using (XmlReader reader = new XmlTextReader(str))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "City"))
                    {

                    }

                    if ((reader.NodeType == XmlNodeType.Attribute) && (reader.Name == "code"))
                    {

                    }
                }

                XElement addressInfo = XElement.Load(reader, LoadOptions.SetBaseUri);
                IEnumerable<XElement> cities = addressInfo.DescendantsAndSelf("City");

                foreach (var city in cities)
                {
                    string cityName =  city.Attribute("name").Value;
                    string cityCode = city.Attribute("code").Value;
                    string cityBody = city.Element("District").Value;

                }

            }


           
            return Task.FromResult(addressInfos);
        }
    }
}
