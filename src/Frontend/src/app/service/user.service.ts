import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import { User } from '../models/user';
@Injectable({
    providedIn: 'root'
})
export class UsersService {
    private url = 'user';

    constructor(private httpService: ApiService) {
    }

    GetAllUsers() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<User[]>(apiRequest));

    }

    GetUserById(id: string) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/${id}`,
            RequestBody: id
        };
        return firstValueFrom(this.httpService.get<User>(apiRequest));
    }

    // DeleteBook(id: string) {
    //     let apiRequest = <ApiRequestData>{
    //         Url: `${this.url}/book/${id}`,
    //         RequestBody: id
    //     };
    //     return firstValueFrom(this.httpService.put(apiRequest));
    // }

    UpdateUser(user: any) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/update`,
            RequestBody: user
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    CreateUser(user: any) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/create`,
            RequestBody: user
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }
}
