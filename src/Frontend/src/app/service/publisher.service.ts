import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import { Publisher } from '../models/publisher';
@Injectable({
    providedIn: 'root'
})
export class PublishersService {
    private url = 'publishers';

    constructor(private httpService: ApiService) {
    }

    GetAllPublishers() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<Publisher[]>(apiRequest));
    }

    GetAllPublishersDictionary() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/getpublishers`
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

    GetPublisherById(id: string) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/${id}`,
            RequestBody: id
        };
        return firstValueFrom(this.httpService.get<Publisher>(apiRequest));
    }

     DeletePublisher(id: string) {
         let apiRequest = <ApiRequestData>{
             Url: `${this.url}/${id}`,
             RequestBody: id
         };
         return firstValueFrom(this.httpService.delete(apiRequest));
     }

    UpdatePublisher(publisher: Publisher) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`,
            RequestBody: publisher
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    CreatePublisher(publisher: Publisher) {
        console.log(publisher);
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`,
            RequestBody: publisher
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }
}
