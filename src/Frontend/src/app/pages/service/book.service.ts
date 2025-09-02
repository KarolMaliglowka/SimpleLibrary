import {Injectable} from '@angular/core';
import {ApiService} from '../../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../../shared/domain/api.request.data";
import { Book } from '../book/book';
@Injectable({
    providedIn: 'root'
})
export class BooksService {
    private url = 'book';

    constructor(private httpService: ApiService) {
    }

    GetAllBooks() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<Book[]>(apiRequest));

    }

    GetBookById(id: string) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/${id}`,
            RequestBody: id
        };
        return firstValueFrom(this.httpService.get<Book>(apiRequest));
    }

    DeleteBook(id: string) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/book/${id}`,
            RequestBody: id
        };
        return firstValueFrom(this.httpService.delete(apiRequest));
    }

    UpdateBook(book: any) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/update`,
            RequestBody: book
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    CreateBook(book: any) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/create`,
            RequestBody: book
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }
}
