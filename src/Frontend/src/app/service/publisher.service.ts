import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import { Publisher } from '../models/publisher';
@Injectable({
    providedIn: 'root'
})
export class PublishersService {
    private url = 'publisher';

    constructor(private httpService: ApiService) {
    }

    GetAllPublishers() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<Publisher[]>(apiRequest));

    }

    GetPublisherById(id: string) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/${id}`,
            RequestBody: id
        };
        return firstValueFrom(this.httpService.get<Publisher>(apiRequest));
    }

    // DeleteBook(id: string) {
    //     let apiRequest = <ApiRequestData>{
    //         Url: `${this.url}/book/${id}`,
    //         RequestBody: id
    //     };
    //     return firstValueFrom(this.httpService.put(apiRequest));
    // }

    UpdatePublisher(publisher: Publisher) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/update`,
            RequestBody: publisher
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    CreatePublisher(publisher: Publisher) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/create`,
            RequestBody: publisher
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }
}
