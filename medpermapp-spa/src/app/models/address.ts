import { City } from './city';
import { County } from './county';
import { Country } from './country';


export class Address {
    Id: number;
    City: City;
    County: County;
    Country: Country;
    Details: string;
    PostalCode: string;
}
