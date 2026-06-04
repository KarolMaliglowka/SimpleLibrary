import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import { User } from '../models/user';
@Injectable({
    providedIn: 'root'
})
export class UsersService {
    private url = 'users';

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

    CreateUser(user: User) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/create`,
            RequestBody: user
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }

    SetNotActive(user: User) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/deactivate`,
            RequestBody: user
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    SetActive(user: User) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/activate`,
            RequestBody: user
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    GetAllUsersDictionary() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/getusers`
        };

        return firstValueFrom(
            this.httpService.get<Record<string, string>[]>(apiRequest)
        ).then(arr =>
            arr.flatMap(obj =>
                Object.entries(obj).map(([key, value]) => ({
                    name: value,
                    id: key
                }))
            )
        );
    }
}
