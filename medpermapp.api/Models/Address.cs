
using System.Collections.Generic;

namespace medpermapp.api.Models
{
    public class Address
    {
        public int Id { get; set; }
        public  City City { get; set; }
        public  County County { get; set; }
        public  Country Country { get; set; }
        public Patient Patient { get; set; }
        public string Details { get; set; }
        public string PostalCode { get; set; }


        public Address()
        {
        }

        public Address(int id, City city, County county, Country country, string details, string postalCode)
        {
            Id = id;
            City = city;
            County = county;
            Country = country;
            Details = details;
            PostalCode = postalCode;
        }

        public Address(City city, County county, Country country, string details, string postalCode) : this(0, city,
            county, country, details, postalCode)
        {
            
        }

        public override bool Equals(object obj)
        {
            return obj != null && !(obj is System.DBNull) && this.Id == ((Address) obj).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}