import {Injectable} from "@angular/core";
import {HttpClient, HttpErrorResponse, HttpHeaders} from "@angular/common/http";
import {ApiRequestData} from "../domain/api.request.data";
import {environment} from "../../environments/environment";
import {catchError, Observable, retry, throwError} from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class ApiService {
    protected headers = new HttpHeaders({'Content-Type': 'application/json'});
    protected baseUrl: string;

    constructor(private httpClient: HttpClient) {
        this.baseUrl = environment.baseUrl;
    }

    private handleError(error: HttpErrorResponse) {
        console.error(error);

        return throwError(() => error);
    }

    public get<T>(data: ApiRequestData): Observable<T> {
        return this.httpClient.get<T>(`${this.baseUrl}/${data.Url}`, {
            headers: this.headers
        }).pipe(catchError(this.handleError));
    }

    public post(data: ApiRequestData): Observable<Response> {
        return this.httpClient.post<Response>(`${this.baseUrl}/${data.Url}`, data.RequestBody, {
            headers: this.headers
        }).pipe(catchError(this.handleError));
    }

    public delete(data: ApiRequestData): Observable<Response> {
        return this.httpClient.delete<Response>(`${this.baseUrl}/${data.Url}`, {
            headers: this.headers
        }).pipe(catchError(this.handleError));
    }

    public patch(data: ApiRequestData): Observable<Response> {
        return this.httpClient.patch<Response>(`${this.baseUrl}/${data.Url}`, data.RequestBody, {
            headers: this.headers,
            params: data.Params
        }).pipe(catchError(this.handleError));
    }
}
