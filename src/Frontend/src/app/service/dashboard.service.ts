import {Injectable} from '@angular/core';
import {ApiService} from '../../shared/services/http.service';
import {firstValueFrom} from 'rxjs';
import {ApiRequestData} from "../../shared/domain/api.request.data";
import { Dashboard } from '../models/dashboard';
@Injectable({
    providedIn: 'root'
})
export class DashboardService {
    private url = 'dashboard';

    constructor(private httpService: ApiService) {
    }

    GetDasboard() {
        let apiRequest = <ApiRequestData>{
            Url: `${this.url}`
        };
        return firstValueFrom(this.httpService.get<Dashboard>(apiRequest));
    }
}
