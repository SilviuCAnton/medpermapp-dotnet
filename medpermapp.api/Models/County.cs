using System.Collections.Generic;

namespace medpermapp.api.Models
{
    public class County
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public County()
        {
        }

        public County(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public County(string name) : this(0, name)
        {
      
        }

        public override bool Equals(object obj)
        {
            return obj != null && !(obj is System.DBNull) && Id == ((County) obj).Id;
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