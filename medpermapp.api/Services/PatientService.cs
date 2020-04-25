using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using medpermapp.api.Data;
using medpermapp.api.ModelsThrift;
using medpermapp.api.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace medpermapp.api.Services
{
    public partial class PatientService
    {
        private ConcurrentDictionary<string, WebSocket> _users = new ConcurrentDictionary<string, WebSocket>();
        private readonly IServiceScopeFactory _factory;

        public PatientService(IServiceScopeFactory scopeFactory)
        {
            _factory = scopeFactory;
        }

        public async Task AddUser(WebSocket socket)
        {
            try
            {
                System.Console.WriteLine("New connection...");
                var name = GenerateName();
                var userAddedSuccessfully = _users.TryAdd(name, socket);
                while (!userAddedSuccessfully)
                {
                    name = GenerateName();
                    userAddedSuccessfully = _users.TryAdd(name, socket);
                }
                SendPatients(socket).Wait();
                SendCities(socket).Wait();
                SendCounties(socket).Wait();
                SendCountries(socket).Wait();

                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    WebSocketReceiveResult socketResponse;
                    var package = new List<byte>();
                    
                    do
                    {
                        socketResponse = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        package.AddRange(new ArraySegment<byte>(buffer, 0, socketResponse.Count));
                    } while (!socketResponse.EndOfMessage);

                    var bufferAsString = System.Text.Encoding.ASCII.GetString(package.ToArray());
                    
                    if (!string.IsNullOrEmpty(bufferAsString))
                    {
                        var socketMessage =(SocketMessage<Patient>) JsonConvert.DeserializeObject(bufferAsString, typeof(SocketMessage<Patient>));
                        System.Console.WriteLine("Am primit request:" + socketMessage.MessageType);
                        if(socketMessage.MessageType.Equals("save")) 
                        {
                            savePatient(socketMessage.Payload);
                        }
                        else if(socketMessage.MessageType.Equals("update")) 
                        {
                            updatePatient(socketMessage.Payload);
                        }
                        else if(socketMessage.MessageType.Equals("delete")) 
                        {
                            deletePatient(socketMessage.Payload);
                        }
                    }
                }
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                System.Console.WriteLine("Closed connection...");
            }
            catch (Exception)
            { }
        }

        private async void deletePatient(Patient patient)
        {
            using(var scope = _factory.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var thePatient = await context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).Where(pat => pat.Equals(patient)).FirstAsync();
                context.Patients.Remove(thePatient);
                context.Addresses.Remove(thePatient.Address);
                context.SaveChanges();
                await SendPatientsToAll();
            }
        }

        private async void updatePatient(Patient patient)
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
                await SendPatientsToAll();
            }
        }

        private async void savePatient(Patient patient)
        {
            using(var scope = _factory.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var city = await context.Cities.Where(city => city.Equals(patient.Address.City)).FirstAsync();
                var county = await context.Counties.Where(county => county.Equals(patient.Address.County)).FirstAsync();
                var country = await context.Countries.Where(country => country.Equals(patient.Address.Country)).FirstAsync();
                patient.Address.City = city;
                patient.Address.County = county;
                patient.Address.Country = country;
                patient.RegistrationDate = DateTime.Now.ToShortDateString();
                await context.Addresses.AddAsync(patient.Address);
                await context.Patients.AddAsync(patient);
                context.SaveChanges();
                await SendPatientsToAll();
            }
        }

        private async Task Send(string message, params WebSocket[] socketsToSendTo)
        {
            var sockets = socketsToSendTo.Where(s => s.State == WebSocketState.Open);
            foreach (var theSocket in sockets)
            {
                var stringAsBytes = System.Text.Encoding.ASCII.GetBytes(message);
                var byteArraySegment = new ArraySegment<byte>(stringAsBytes, 0, stringAsBytes.Length);
                await theSocket.SendAsync(byteArraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private async Task SendAll(string message)
        {
            await Send(message, _users.Values.ToArray());
        }

        private async Task SendPatientById(WebSocket socket, int id) {
            using(var scope = _factory.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                Patient pat = await context.Patients.Where(pat => pat.Id == id).FirstOrDefaultAsync();

                var message = new SocketMessage<Patient>()
                {
                    MessageType = "findById",
                    Payload = pat
                };
                await Send(System.Text.Json.JsonSerializer.Serialize(message));
            }
        }

        private async Task SendPatients(WebSocket socket)
        {
            using(var scope = _factory.CreateScope()) 
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
                List<Patient> patients = context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).ToList();
                var message = new SocketMessage<List<Patient>>()
                {
                    
                    MessageType = "patients",
                    Payload = patients
                };
                var str = System.Text.Json.JsonSerializer.Serialize(message);
                await Send(str, socket);
            }
        }

        private async Task SendCities(WebSocket socket)
        {
            using(var scope = _factory.CreateScope()) 
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
                List<City> cities = context.Cities.ToList();
                var message = new SocketMessage<List<City>>()
                {
                    
                    MessageType = "cities",
                    Payload = cities
                };
                var str = System.Text.Json.JsonSerializer.Serialize(message);
                await Send(str, socket);
            }
        }

        private async Task SendCounties(WebSocket socket)
        {
            using(var scope = _factory.CreateScope()) 
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
                List<County> counties = context.Counties.ToList();
                var message = new SocketMessage<List<County>>()
                {
                    
                    MessageType = "counties",
                    Payload = counties
                };
                var str = System.Text.Json.JsonSerializer.Serialize(message);
                await Send(str, socket);
            }
        }

        private async Task SendCountries(WebSocket socket)
        {
            using(var scope = _factory.CreateScope()) 
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
               
                List<Country> countries = context.Countries.ToList();
                var message = new SocketMessage<List<Country>>()
                {
                    
                    MessageType = "countries",
                    Payload = countries
                };
                var str = System.Text.Json.JsonSerializer.Serialize(message);
                await Send(str, socket);
            }
        }

        private async Task SendPatientsToAll() {
            using(var scope = _factory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var patients = context.Patients.Include(pat => pat.Address).ThenInclude(add => add.City).Include(pat => pat.Address).ThenInclude(add => add.County).Include(pat => pat.Address).ThenInclude(add => add.Country).ToList();
                var message = new SocketMessage<List<Patient>>()
                {
                    MessageType = "patients",
                    Payload = patients
                };

            await SendAll(System.Text.Json.JsonSerializer.Serialize(message));
            } 
        }

        private string GenerateName()
        {
            var prefix = "WebUser";
            Random ran = new Random();
            var name = prefix + ran.Next(1, 1000);
            while (_users.ContainsKey(name))
            {
                name = prefix + ran.Next(1, 1000);
            }
            return name;
        }
    }
}