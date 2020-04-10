import { Component, OnInit, OnDestroy } from '@angular/core';
import { Patient } from 'src/app/models/patient';
import { WebSocketService } from 'src/app/web-socket.service';

@Component({
  selector: 'app-patient-socket',
  templateUrl: './patient.component.html',
  styleUrls: ['./patient.component.css']
})
export class PatientComponent implements OnInit, OnDestroy {
  messages: string[] = [];
  patients: Patient[] = [];


  constructor(private socketService: WebSocketService) {
    this.socketService.patients$.subscribe(pat => {
      this.patients = pat;
    });
   }

  ngOnInit() {
    this.socketService.startSocket();
  }

  ngOnDestroy() {
    this.socketService.stopSocket();
  }

  deleteEvent(patientId: number) {
    if (!(confirm('Are you sure you want to delete this patient?'))) {
      return false;
    }
    const patient = new Patient();
    patient.Id = patientId;
    const message = {MessageType: 'delete', Payload: patient};
    this.socketService.sendRequest(message);
  }

}
