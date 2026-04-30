import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import {Category} from '../models/category';

@Injectable({
    providedIn: 'root'
})
export class CategoriesService {
    private url = 'categories';

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

    DeleteCategory(id: string) {
         let apiRequest = <ApiRequestData>{
             Url: `${this.url}/delete/${id}`,
             RequestBody: id
         };
         return firstValueFrom(this.httpService.delete(apiRequest));
     }

    UpdateCategory(category: Category) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/update`,
            RequestBody: category
        };
        return firstValueFrom(this.httpService.patch(apiRequest));
    }

    CreateCategory(category: Category) {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/create`,
            RequestBody: category
        };
        return firstValueFrom(this.httpService.post(apiRequest));
    }

    GetAllCategoriesDictionary() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}/getcategories`
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
