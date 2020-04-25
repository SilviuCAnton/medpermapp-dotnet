using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using medpermapp.api.Data;
using medpermapp.api.ModelsThrift;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace medpermapp.api.Services
{
    public class PatientHandler : PatientServiceThrift.Iface
    {
        private readonly IServiceScopeFactory _factory;

        public PatientHandler(IServiceScopeFactory scopeFactory)
        {
            _factory = scopeFactory;
        }

        public void deletePatient(Patient patient)
        {
            using(var scope = _factory.CreateScope()) 
            {
                System.Console.WriteLine(patient);
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                // var thePatient = context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).Where(pat => pat.Id.Equals(patient.Id)).First();
                context.Patients.Remove(patient);
                context.Addresses.Remove(patient.Address);
                context.SaveChanges();
                // await SendPatientsToAll();
            }
        }

        // public async Task deletePatientAsync(Patient patient, CancellationToken cancellationToken = default)
        // {
        //     using(var scope = _factory.CreateScope()) {
        //         var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        //         var thePatient = await context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).Where(pat => pat.Equals(patient)).FirstAsync();
        //         context.Patients.Remove(thePatient);
        //         context.Addresses.Remove(thePatient.Address);
        //         context.SaveChanges();
        //     }
        // }

        public Patient findPatient(int id)
        {
            using(var scope = _factory.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                Patient patient = context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).Where(pat => pat.Id.Equals(id)).First();

                Patient res = new Patient();
                System.Console.WriteLine(patient.Address.City.Name);
                res.Id = patient.Id;
                res.FInitLetter = patient.FInitLetter;
                res.FirstName = patient.FirstName;
                res.LastName = patient.LastName;
                res.Address = patient.Address;
                res.Address.Id = patient.Address.Id;
                res.Address.City = patient.Address.City;
                res.Address.Country = patient.Address.Country;
                res.Address.County = patient.Address.County;
                res.Address.Country.Id = patient.Address.Country.Id;
                res.Address.County.Id = patient.Address.County.Id;
                res.Address.City.Id = patient.Address.City.Id;
                res.Address.Country.Name = patient.Address.Country.Name;
                res.Address.County.Name = patient.Address.County.Name;
                res.Address.City.Name = patient.Address.City.Name;
                res.Cnp = patient.Cnp;
                res.Address.PostalCode = patient.Address.PostalCode;
                res.RegistrationDate = patient.RegistrationDate;

                return res;
            }
        }

        // public async Task<Patient> findPatientAsync(int id, CancellationToken cancellationToken = default)
        // {
        //     using(var scope = _factory.CreateScope()) {
        //         var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        //         Patient pat = await context.Patients.Where(pat => pat.Id == id).FirstOrDefaultAsync();

        //         return pat;
        //     }
        // }

        public List<City> getCities()
        {
            using(var scope = _factory.CreateScope()) 
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
                List<City> cities = context.Cities.ToList();
                List<City> result = new List<City>();
                cities.ForEach(city => 
                {
                    City res = new City();
                    res.Id = city.Id;
                    res.Name = city.Name;
                    result.Add(res);
                });
                return result;
            }
        }

        // public async Task<List<City>> getCitiesAsync(CancellationToken cancellationToken = default)
        // {
        //     using(var scope = _factory.CreateScope()) 
        //     {
        //         var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
        //         List<City> cities = await context.Cities.ToListAsync();
              
        //         return cities;
        //     }
        // }

        public List<County> getCounties()
        {
            using(var scope = _factory.CreateScope()) 
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
                List<County> counties = context.Counties.ToList();
                List<County> result = new List<County>();
                counties.ForEach(county => 
                {
                    County res = new County();
                    res.Id = county.Id;
                    res.Name = county.Name;
                    result.Add(res);
                });
                return result;
            }
        }

        // public async Task<List<County>> getCountiesAsync(CancellationToken cancellationToken = default)
        // {
        //     using(var scope = _factory.CreateScope()) 
        //     {
        //         var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
        //         List<County> counties = await context.Counties.ToListAsync();
                
        //         return counties;
        //     }
        // }

        public List<Country> getCountries()
        {
            using(var scope = _factory.CreateScope()) 
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
                List<Country> countries = context.Countries.ToList();
                List<Country> result = new List<Country>();
                countries.ForEach(country => 
                {
                    Country res = new Country();
                    res.Id = country.Id;
                    res.Name = country.Name;
                    result.Add(res);
                });
                return result;
            }
        }

        // public async Task<List<Country>> getCountriesAsync(CancellationToken cancellationToken = default)
        // {
        //     using(var scope = _factory.CreateScope()) 
        //     {
        //         var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
        //         List<Country> countries = await context.Countries.ToListAsync();
                
        //         return countries;
        //     }
        // }

        public List<Patient> getPatients()
        {
            using(var scope = _factory.CreateScope()) 
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                List<Patient> patients = context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).ToList();
                List<Patient> result = new List<Patient>();
                patients.ForEach(patient => 
                {
                    Patient res = new Patient();
                    res.Id = patient.Id;
                    res.FInitLetter = patient.FInitLetter;
                    res.FirstName = patient.FirstName;
                    res.LastName = patient.LastName;
                    res.Address = patient.Address;
                    res.Address.Id = patient.Address.Id;
                    res.Address.City = patient.Address.City;
                    res.Address.Country = patient.Address.Country;
                    res.Address.County = patient.Address.County;
                    res.Address.Country.Id = patient.Address.Country.Id;
                    res.Address.County.Id = patient.Address.County.Id;
                    res.Address.City.Id = patient.Address.City.Id;
                    res.Address.Country.Name = patient.Address.Country.Name;
                    res.Address.County.Name = patient.Address.County.Name;
                    res.Address.City.Name = patient.Address.City.Name;
                    res.Cnp = patient.Cnp;
                    res.Address.PostalCode = patient.Address.PostalCode;
                    res.RegistrationDate = patient.RegistrationDate;
                    result.Add(res);
                });
                return result;
            }
        }

        // public async Task<List<Patient>> getPatientsAsync(CancellationToken cancellationToken = default)
        // {
        //     using(var scope = _factory.CreateScope())
        //     {
        //         var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        //         var patients = await context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).ToListAsync();

        //         return patients;
        //     } 
        // }

        public void savePatient(Patient patient)
        {
            using(var scope = _factory.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var city =  context.Cities.Where(city => city.Id.Equals(patient.Address.City.Id)).First();
                var county =  context.Counties.Where(county => county.Id.Equals(patient.Address.County.Id)).First();
                var country =  context.Countries.Where(country => country.Id.Equals(patient.Address.Country.Id)).First();
                patient.Address.City = city;
                patient.Address.County = county;
                patient.Address.Country = country;
                patient.RegistrationDate = DateTime.Now.ToShortDateString();
                 context.Addresses.Add(patient.Address);
                 context.Patients.Add(patient);
                context.SaveChanges();
                // await SendPatientsToAll();
            }
        }

        // public async Task savePatientAsync(Patient patient, CancellationToken cancellationToken = default)
        // {
        //     using(var scope = _factory.CreateScope()) {
        //         var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        //         var city = await context.Cities.Where(city => city.Equals(patient.Address.City)).FirstAsync();
        //         var county = await context.Counties.Where(county => county.Equals(patient.Address.County)).FirstAsync();
        //         var country = await context.Countries.Where(country => country.Equals(patient.Address.Country)).FirstAsync();
        //         patient.Address.City = city;
        //         patient.Address.County = county;
        //         patient.Address.Country = country;
        //         patient.RegistrationDate = DateTime.Now.ToShortDateString();
        //         await context.Addresses.AddAsync(patient.Address);
        //         await context.Patients.AddAsync(patient);
        //         context.SaveChanges();
        //     }
        // }

        public async void updatePatient(Patient patient)
        {
            using(var scope = _factory.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var thePatient = await context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).Where(pat => pat.Equals(patient)).FirstAsync();
                var city = await context.Cities.Where(city => city.Equals(patient.Address.City)).FirstAsync();
                var county = await context.Counties.Where(county => county.Equals(patient.Address.County)).FirstAsync();
                var country = await context.Countries.Where(country => country.Equals(patient.Address.Country)).FirstAsync();
                thePatient.Address.City = city;
                thePatient.Address.County = county;
                thePatient.Address.Country = country;
                thePatient.FirstName = patient.FirstName;
                thePatient.LastName = patient.LastName;
                thePatient.Cnp = patient.Cnp;
                thePatient.FInitLetter = patient.FInitLetter;
                thePatient.Address.Details = patient.Address.Details;
                thePatient.Address.PostalCode = patient.Address.PostalCode;
                context.SaveChanges();
                // await SendPatientsToAll();
            }
        }

        // public async Task updatePatientAsync(Patient patient, CancellationToken cancellationToken = default)
        // {
        //     using(var scope = _factory.CreateScope()) {
        //         var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        //         var thePatient = await context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).Where(pat => pat.Equals(patient)).FirstAsync();
        //         var city = await context.Cities.Where(city => city.Equals(patient.Address.City)).FirstAsync();
        //         var county = await context.Counties.Where(county => county.Equals(patient.Address.County)).FirstAsync();
        //         var country = await context.Countries.Where(country => country.Equals(patient.Address.Country)).FirstAsync();
        //         thePatient.Address.City = city;
        //         thePatient.Address.County = county;
        //         thePatient.Address.Country = country;
        //         thePatient.FirstName = patient.FirstName;
        //         thePatient.LastName = patient.LastName;
        //         thePatient.Cnp = patient.Cnp;
        //         thePatient.FInitLetter = patient.FInitLetter;
        //         thePatient.Address.Details = patient.Address.Details;
        //         thePatient.Address.PostalCode = patient.Address.PostalCode;
        //         context.SaveChanges();
        //     }
        // }
    }
}