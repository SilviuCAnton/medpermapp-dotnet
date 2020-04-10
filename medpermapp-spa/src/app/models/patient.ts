import { Address } from './address';

export class Patient {
    Id: number;
    FirstName: string;
    LastName: string;
    Cnp: string;
    FInitLetter: string;
    RegistrationDate: Date;
    Address: Address;
}
