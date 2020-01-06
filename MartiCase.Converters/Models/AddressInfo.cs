using System.Collections.Generic;

namespace MartiCase.Converters.Model
{
    public class AddressInfo
    {
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string DistrictName { get; set; }
        public string ZipCode { get; set; }
        //public List<City> Cities { get; set; }
    }

    public class City
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<District> Districts { get; set; }
    }
    public class District
    {
        public string Name { get; set; }
        public List<string> ZipCode { get; set; }
    }

}
