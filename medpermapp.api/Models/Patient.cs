using System;

namespace medpermapp.api.Models
{
    public class Patient
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Cnp { get; set; }
        
        public char FInitLetter { get; set; }
        
        public DateTime RegistrationDate { get; set; }
        
        public Address Address { get; set; }


        public Patient()
        {
        }

        public Patient(int id, string firstName, string lastName, string cnp, char fInitLetter, DateTime registrationDate, Address address)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Cnp = cnp;
            FInitLetter = fInitLetter;
            RegistrationDate = registrationDate;
            Address = address;
        }

        public Patient(string firstName, string lastName, string cnp, char fInitLetter, DateTime registrationDate, Address address) : this(0, firstName, lastName, cnp, fInitLetter, registrationDate, address)
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return obj != null && !(obj is System.DBNull);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return FirstName + LastName + Address;
        }
    }
}