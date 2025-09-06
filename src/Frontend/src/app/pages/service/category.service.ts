import {Injectable} from '@angular/core';
import {ApiService} from '../../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../../shared/domain/api.request.data";
import { Category } from '../category/category';
@Injectable({
    providedIn: 'root'
})
export class CategoriesService {
    private url = 'category';

    constructor(private httpService: ApiService) {
    }

    GetAllCategories() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<Category[]>(apiRequest));

    }

    GetCategoryById(id: string) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/${id}`,
            RequestBody: id
        };
        return firstValueFrom(this.httpService.get<Category>(apiRequest));
    }

    // DeleteBook(id: string) {
    //     let apiRequest = <ApiRequestData>{
    //         Url: `${this.url}/book/${id}`,
    //         RequestBody: id
    //     };
    //     return firstValueFrom(this.httpService.put(apiRequest));
    // }

    UpdateCategory(category: any) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/update`,
            RequestBody: category
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    CreateCategory(category: any) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/create`,
            RequestBody: category
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }
}
