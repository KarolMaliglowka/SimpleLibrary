import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import {Borrow} from '../models/borrow';

@Injectable({
    providedIn: 'root'
})
export class BorrowsService {
    private url = 'borrow';

    constructor(private httpService: ApiService) {
    }

    GetAllBorrows() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<Borrow[]>(apiRequest));
    }
    DeleteBorrow(borrow: any) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/delete`,
            RequestBody: borrow
        };
        console.log(apiRequest);
        console.log('id ? ',apiRequest.RequestBody.id);
        return firstValueFrom(this.httpService.delete(apiRequest));
    }

}
