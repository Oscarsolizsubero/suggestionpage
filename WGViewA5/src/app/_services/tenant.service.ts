import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AuthHttp } from 'angular2-jwt';
import { AuthService } from './auth.service';
import { VMTenant } from '../_models/VMTenant';
import { Observable } from 'rxjs/Observable';
import { Http, RequestOptions, Headers, Response } from '@angular/http';

@Injectable()
export class TenantService {
  baseUrl = environment.apiUrl;

  constructor(
    private authHttp: AuthHttp,
    private authService: AuthService,
    private http: Http
  ) { }

  loadTenant(): Observable<VMTenant> {
    return this.authHttp.get(this.baseUrl + 'tenant/load/' + this.authService.decodedToken.aud + '/' + this.authService.decodedToken.nameid + '/' + this.authService.decodedToken.role).map(response => <VMTenant>response.json());
  }

  edit(model) {
    return this.authHttp.put(this.baseUrl + 'tenant/edit', model);
  }

  getUrlImage(urlWG) {
    return this.http.get(this.baseUrl + 'tenant/image/' + urlWG, this.requestOptions());
  }

  private requestOptions() {
    const headers = new Headers({ 'Content-type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }
}
