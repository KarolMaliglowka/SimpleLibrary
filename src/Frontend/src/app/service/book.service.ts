import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import { Book } from '../models/book';
import { MultiSelectModule } from 'primeng/multiselect';

@Injectable({
    providedIn: 'root'
})
export class BooksService {
    private url = 'books';

    constructor(private httpService: ApiService) {
    }

    GetAllBooks() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<Book[]>(apiRequest));

    }

    GetAllBooksDictionary() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/getbooks`
        };
        return firstValueFrom(this.httpService.get<Record<string, string>[]>(apiRequest));
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
             Url: `${this.url}/delete/${id}`,
             RequestBody: id
         };
         return firstValueFrom(this.httpService.delete(apiRequest));
     }

    UpdateBook(book: Book) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/update`,
            RequestBody: book
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    CreateBook(book: Book) {
        console.log("ksiązka: ", book);
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/create`,
            RequestBody: book
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }
}
