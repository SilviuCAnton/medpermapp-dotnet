using System.Collections.Generic;

namespace medpermapp.api.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public Country()
        {
        }

        public Country(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Country(string name) : this(0, name)
        {
      
        }

        public override bool Equals(object obj)
        {
            return obj != null && !(obj is System.DBNull) && Id == ((Country) obj).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}