import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Patient } from '../models/patient';

@Component({
  selector: 'app-patient-rest',
  templateUrl: './patient.component.html',
  styleUrls: ['./patient.component.css']
})
export class PatientComponent implements OnInit {
  patients: Patient[];

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getPatients();
  }

  getPatients() {
    this.http.get('http://localhost:5000/patients').subscribe(response => {
      this.patients = response as Patient[];
      console.log(this.patients);
    }, error => {
      console.log(error);
    });
  }

}
