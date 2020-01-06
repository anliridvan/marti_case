using MartiCase.Converters.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MartiCase.Converters
{
    public class XMLFileConverter : IFileConverter
    {

        public Task<byte[]> AddressInfoByteArray(List<AddressInfo> input)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("AddressInfo");

                foreach (var city in input.Select(x => new string[] { x.CityName, x.CityCode }).GroupBy(x => x[0]))
                {
                    writer.WriteStartElement("City");
                    writer.WriteAttributeString("code", city.First()[1]);
                    writer.WriteAttributeString("name", city.First()[0]);

                    foreach (var district in input.Where(x => x.CityName == city.First()[0]).Select(x => x.DistrictName).Distinct())
                    {
                        writer.WriteStartElement("District");
                        writer.WriteAttributeString("name", district);

                        foreach (var zipCode in input.Where(x => x.CityName == city.First()[0] && x.DistrictName == district).Select(x => x.ZipCode).Distinct())
                        {
                            writer.WriteStartElement("Zip");
                            writer.WriteAttributeString("code", zipCode);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            return Task.FromResult(Encoding.UTF8.GetBytes(sb.ToString()));
        }


        public Task<List<AddressInfo>> ByteArrayToAddressInfo(byte[] input)
        {
            List<AddressInfo> addressInfos = new List<AddressInfo>();
            string str = Encoding.UTF8.GetString(input);
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (_byteOrderMarkUtf8.Length > 0 && str.StartsWith(_byteOrderMarkUtf8))
            {
                str = str.Replace(_byteOrderMarkUtf8, "");
            }
            using (XmlReader reader = XmlReader.Create(new StringReader(str)))
            {
                XElement addressInfo = XElement.Load(reader, LoadOptions.SetBaseUri);
                IEnumerable<XElement> cities = addressInfo.DescendantsAndSelf("City");

                foreach (var city in cities)
                {
                    string cityName = city.Attribute("name").Value;
                    string cityCode = city.Attribute("code").Value;
                    IEnumerable<XElement> districts = city.DescendantsAndSelf("District");
                    foreach (var district in districts)
                    {
                        string districtName = district.Attribute("name").Value;
                        IEnumerable<XElement> zipCodes = district.DescendantsAndSelf("Zip");
                        foreach (var codes in zipCodes)
                        {
                            string zipCode = codes.Attribute("code").Value;
                            addressInfos.Add(new AddressInfo
                            {
                                CityName = cityName,
                                CityCode = cityCode,
                                DistrictName = districtName,
                                ZipCode = zipCode
                            });
                        }
                    }
                }

            }

            return Task.FromResult(addressInfos);
        }

        public string GetContentType()
        {
            return "application/xml";
        }
    }
}
