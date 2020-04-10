import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { WebSocketService } from '../web-socket.service';
import { Patient } from '../models/patient';
import { City } from '../models/city';
import { County } from '../models/county';
import { Country } from '../models/country';

@Component({
  selector: 'app-patientform',
  templateUrl: './patientform.component.html',
  styleUrls: ['./patientform.component.css']
})
export class PatientformComponent implements OnInit, OnDestroy {
  patient: Patient;
  cities: City[];
  counties: County[];
  countries: Country[];
  selectedCity: string;
  selectedCounty: string;
  selectedCountry: string;
  firstName: string;
  lastName: string;
  FInit: string;
  details: string;
  postal: string;
  cnp: string;

  constructor(private route: ActivatedRoute, private service: WebSocketService) {
    this.service.onePatient$.subscribe(pat => {
      this.patient = pat;
    });
    this.service.cities$.subscribe(cities => {
      this.cities = cities;
    });
    this.service.counties$.subscribe(counties => {
      this.counties = counties;
    });
    this.service.countries$.subscribe(countries => {
      this.countries = countries;
    });
  }

  ngOnInit() {
    this.service.startSocket();
    const patientId = this.route.snapshot.paramMap.get('id');
    this.service.getPatient(patientId);
    this.firstName = this.patient?.FirstName;
    this.lastName = this.patient?.LastName;
    this.FInit = this.patient?.FInitLetter;
    this.details = this.patient?.Address.Details;
    this.postal = this.patient?.Address.PostalCode;
    this.cnp = this.patient?.Cnp;
  }

  ngOnDestroy() {
    this.service.stopSocket();
  }

  saveEvent() {
    const selectedCity = this.cities.find(city => city.Id === parseInt(this.selectedCity, 10)) as City;
    const selectedCounty = this.counties.find(county => county.Id === parseInt(this.selectedCounty, 10)) as County;
    const selectedCountry = this.countries.find(country => country.Id === parseInt(this.selectedCountry, 10)) as Country;

    const patient = {
        Id: parseInt(this.route.snapshot.paramMap.get('id'), 10),
        FirstName : this.firstName,
        LastName : this.lastName,
        Cnp : this.cnp,
        FInitLetter: this.FInit,
        Address : {
          Country: { id: selectedCountry.Id, name: selectedCountry.Name},
          County: { id: selectedCounty.Id, name: selectedCounty.Name},
          City: { id: selectedCity.Id, name: selectedCity.Name},
          Details: this.details,
          PostalCode: this.postal
        },
    };
    const messsage = {MessageType: 'save', Payload: patient};
    this.sendSaveMessage(patient);
  }

  sendSaveMessage(patient) {
    let type = '';
    if (patient.Id === 0) {
      type = 'save';
    } else {
      type = 'update';
    }
    const message = {MessageType: type, Payload: patient};
    this.service.sendRequest(message);
  }
}
