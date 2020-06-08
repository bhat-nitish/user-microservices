import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { AdminUserView } from '../admin/Dto/AdminUserView';
import { Observable, of } from 'rxjs';
import { NotifyUserDto } from '../admin/Dto/NotifyUserDto';
import { AdminServiceResponseDto } from '../admin/Dto/AdminServiceResponseDto';



@Injectable({
    providedIn: 'root'
})
export class AdminService {

    constructor(private http: HttpClient) { }

    public getAll = (): Observable<Array<AdminUserView>> => {
        return this.http.get<Array<AdminUserView>>(this.createCompleteRoute("admin/users", environment.adminBaseUrl));
    }

    public notify = (body: NotifyUserDto): Observable<AdminServiceResponseDto> => {
        return this.http.post<AdminServiceResponseDto>(this.createCompleteRoute(`admin/users/notify`, environment.adminBaseUrl), body, this.generateHeaders());
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