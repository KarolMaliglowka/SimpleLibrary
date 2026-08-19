import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import {Author} from '../models/author';

@Injectable({
    providedIn: 'root'
})
export class AuthorsService {
    private url = 'authors';

    constructor(private httpService: ApiService) {
    }

    GetAllAuthors() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<Author[]>(apiRequest));
    }

    GetAuthorById(id: string) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/${id}`,
            RequestBody: id
        };
        return firstValueFrom(this.httpService.get<Author>(apiRequest));
    }

    DeleteAuthor(id: string) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/${id}`,
            RequestBody: id
        };
        return firstValueFrom(this.httpService.delete(apiRequest));
    }

    UpdateAuthor(author: Author) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`,
            RequestBody: author
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    CreateAuthor(author: Author) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`,
            RequestBody: author
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }

    GetAllAuthorsDictionary() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/getAuthors`
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
