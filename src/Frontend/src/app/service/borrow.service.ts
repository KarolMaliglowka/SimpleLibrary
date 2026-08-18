import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import {Borrow, CreateBorrowRequest} from '../models/borrow';

@Injectable({
    providedIn: 'root'
})
export class BorrowsService {
    private url = 'borrows';

    constructor(private httpService: ApiService) { }

    GetAllBorrows() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<Borrow[]>(apiRequest));
    }
    DeleteBorrow(borrow: any) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/delete/${borrow}`,
            RequestBody: borrow
        };
        return firstValueFrom(this.httpService.delete(apiRequest));
    }

    CreateBorrow(createBorrowRequest: CreateBorrowRequest) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/create`,
            RequestBody: createBorrowRequest
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }
}
