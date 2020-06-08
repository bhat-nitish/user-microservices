import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { CreateUser, UserServiceResponseDto } from '../add-user/dto/index';
import { UserView, UserModify } from '../user-list/dto/index';
import { Observable, of } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    constructor(private http: HttpClient) { }

    public getAll = (): Observable<Array<UserView>> => {
        return this.http.get<Array<UserView>>(this.createCompleteRoute("users", environment.apiBaseUrl));
    }

    public create = (user: CreateUser): Observable<UserServiceResponseDto> => {
        return this.http.post<UserServiceResponseDto>(this.createCompleteRoute(`users`, environment.apiBaseUrl), user, this.generateHeaders());
    }

    public update = (id: number, body: UserModify): Observable<UserServiceResponseDto> => {
        return this.http.put<UserServiceResponseDto>(this.createCompleteRoute(`users/${id}`, environment.apiBaseUrl), body, this.generateHeaders());
    }

    public delete = (id: number): Observable<UserServiceResponseDto> => {
        return this.http.delete<UserServiceResponseDto>(this.createCompleteRoute(`users/${id}`, environment.apiBaseUrl));
    }

    private createCompleteRoute = (route: string, envAddress: string) => {
        return `${envAddress}/${route}`;
    }

    private generateHeaders = () => {
        return {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        }
    }
}