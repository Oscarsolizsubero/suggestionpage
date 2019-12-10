import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AuthHttp } from 'angular2-jwt';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs/Observable';
import { RequestPaginatorWithUser } from '../_models/VMFilter';
import { VMUserforList, VMUserAdd } from '../_models/VMUser';
import { Http, RequestOptions, Headers, Response } from '@angular/http';

@Injectable()
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(
    private authHttp: AuthHttp,
    private authService: AuthService,
    private http: Http) { }

  search(filter: RequestPaginatorWithUser): Observable<VMUserforList[]> {
    return this.authHttp.post(this.baseUrl + 'user/search/', filter).map(response => <VMUserforList[]>response.json());
  }

  size(paginator: RequestPaginatorWithUser): Observable<number>{
    return this.authHttp.post(this.baseUrl + 'user/size/', paginator).map(response => <number>response.json());
  }

  edit(model) {
    return this.authHttp.put(this.baseUrl + 'user/edit', model);
  }

  create(model: VMUserAdd) {
    return this.http.post(this.baseUrl + 'user/create', model, this.requestOptions());
  }

  verificationEmail(model: VMUserAdd) {
    return this.http.post(this.baseUrl + 'user/verificationEmail', model, this.requestOptions());
  }

  editPassword(model) {
    return this.authHttp.put(this.baseUrl + 'user/editPassword', model);
  }

  resetPassword(model) {
    return this.http.post(this.baseUrl + 'user/resetPassword', model);
  }

  getValidationUser(token) {
    return this.http.get(this.baseUrl + 'user/validation/' + this.authService.getDomain(window.location.href, true) + '/' + token, this.requestOptions()).map((response: Response) => {
      return response.json();
    });
  }

  private requestOptions() {
    const headers = new Headers({ 'Content-type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }
}
