using System.Collections.Generic;

namespace medpermapp.api.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
       


        public City()
        {
        }
        
        public City(string name) : this(0, name)
        {
      
        }

        public City(int id, string name)
        {
            Id = id;
            Name = name;
        }

        

        public override bool Equals(object obj)
        {
            return obj != null && !(obj is System.DBNull) && Id == ((City) obj).Id;
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