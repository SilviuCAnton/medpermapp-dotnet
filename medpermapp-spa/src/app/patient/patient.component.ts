import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-patient',
  templateUrl: './patient.component.html',
  styleUrls: ['./patient.component.css']
})
export class PatientComponent implements OnInit {
  patients: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getPatients();
  }

  getPatients() {
    this.http.get('http://localhost:5000/patients').subscribe(response => {
      this.patients = response;
    }, error => {
      console.log(error);
    });
  }

}
