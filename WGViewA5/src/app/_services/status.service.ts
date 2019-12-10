import { Injectable } from '@angular/core';
import { AuthHttp } from 'angular2-jwt';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { VMStatus } from '../_models/VMStatus';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class StatusService {
  baseUrl = environment.apiUrl;

  constructor(
    private authHttp: AuthHttp,
    private authService: AuthService,
    private http: Http
  ) { }

  searchPublic(): Observable<VMStatus[]> {
    return this.http.get(this.baseUrl + 'status/search', this.requestOptions()).map(response => <VMStatus[]>response.json());
  }

  searchPrivate(): Observable<VMStatus[]> {
    return this.authHttp.get(this.baseUrl + 'status/searchprivate/' + this.authService.decodedToken.role).map(response => <VMStatus[]>response.json());
  }

  searchForEdit(idSuggestion): Observable<VMStatus[]> {
    return this.authHttp.get(this.baseUrl + 'status/searchstatusforedit/' + this.authService.decodedToken.role + '/' + idSuggestion).map(response => <VMStatus[]>response.json());
  }

  private requestOptions() {
    const headers = new Headers({ 'Content-type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }
}
