import { Injectable } from '@angular/core';
import { Patient } from './models/patient';
import { BehaviorSubject } from 'rxjs';
import { SocketMessage } from './models/socket-message';
import { City } from './models/city';
import { County } from './models/county';
import { Country } from './models/country';
import { INT_TYPE } from '@angular/compiler/src/output/output_ast';

@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  constructor() { }

  private socket: WebSocket;
  patients$: BehaviorSubject<Patient[]> = new BehaviorSubject<Patient[]>([]);
  onePatient$: BehaviorSubject<Patient> = new BehaviorSubject<Patient>(null);
  cities$: BehaviorSubject<City[]> = new BehaviorSubject<City[]>([]);
  counties$: BehaviorSubject<County[]> = new BehaviorSubject<County[]>([]);
  countries$: BehaviorSubject<Country[]> = new BehaviorSubject<Country[]>([]);
  returnedPatinet = false;

  startSocket() {
    this.socket = new WebSocket('wss://localhost:5001/ws');
    this.socket.addEventListener('open', (ev => {
      console.log('opened');
    }));
    this.socket.addEventListener('close', (ev => {
      console.log('closed');
    }));
    this.socket.addEventListener('message', (ev => {
      const messageBox: SocketMessage = JSON.parse(ev.data);
      console.log('message object', messageBox);
      switch (messageBox.MessageType) {
        case 'announce':
          break;
        case 'patients':
          this.patients$.next(messageBox.Payload);
          break;
        case 'cities':
          this.cities$.next(messageBox.Payload);
          break;
        case 'counties':
          this.counties$.next(messageBox.Payload);
          break;
        case 'countries':
          this.countries$.next(messageBox.Payload);
          break;
        case 'findById':
          this.onePatient$.next(messageBox.Payload);
          console.log(messageBox.Payload);
          break;
        default:
          break;
      }
    }));
  }

  stopSocket() {
    this.socket.close();
  }

  getPatient(patientId: string) {
    if (this.socket.OPEN) {
      this.onePatient$.next(this.patients$.value.find(pat => pat.Id === parseInt(patientId, 10)));
    }
  }

  sendRequest(patientMessage: SocketMessage) {
    this.socket.send(JSON.stringify(patientMessage));
  }
}
