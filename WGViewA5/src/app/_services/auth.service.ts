import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { tokenNotExpired, JwtHelper } from 'angular2-jwt';
import { environment } from '../../environments/environment'
import 'rxjs/add/operator/map';

@Injectable()
export class AuthService {
  baseUrl = environment.apiUrl;
  userToken: any;
  decodedToken: any;
  jwtHelper: JwtHelper = new JwtHelper();

  constructor(private http: Http) {}

  login(model: any) {
    return this.http.post(this.baseUrl + 'auth/login', model, this.requestOptions()).map((response: Response) => {
        const user = response.json();
        if (user && user.tokenString) {
          localStorage.setItem('token', user.tokenString);
          this.decodedToken = this.jwtHelper.decodeToken(user.tokenString);
          this.userToken = user.tokenString;
        }
    });
  }

  register(model: any) {
    console.log(model);
    return this.http.post(this.baseUrl + 'register', model, this.requestOptions());
  }

  loggedIn() {
    return tokenNotExpired('token');
  }

  private requestOptions() {
    const headers = new Headers({ 'Content-type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }

  getDomain(url, subdomain) {
    subdomain = subdomain || false;
    url = url.replace(/(https?:\/\/)?(www.)?/i, '');
    if (!subdomain) {
      url = url.split('.');
      url = url.slice(url.length - 2).join('.');
    }
    if (url.indexOf('/') !== -1) {
      return url.split('/')[0];
    }
    return url;
  }
}
