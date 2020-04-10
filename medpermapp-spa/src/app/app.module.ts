import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule, Routes} from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { WebSocketService } from './web-socket.service';
import { PatientComponent } from './patientWebSocket/patient.component';
import { PatientformComponent } from './patientform/patientform.component';

const appRoutes: Routes = [
   { path: 'patients', component: PatientComponent },
   { path: 'patientform', component: PatientformComponent },
   { path: 'patientform/:id', component: PatientformComponent },
   { path: '', redirectTo: '/patients', pathMatch: 'full'},
 ];

@NgModule({
   declarations: [
      AppComponent,
      PatientComponent,
      PatientformComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      RouterModule.forRoot(
         appRoutes,
         // { enableTracing: true } // <-- debugging purposes only
       )
   ],
   providers: [
      WebSocketService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
