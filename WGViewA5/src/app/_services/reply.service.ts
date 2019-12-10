import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { AuthHttp } from 'angular2-jwt';
import { AuthService } from './auth.service';
import { VMReplyAdd, VMReply } from '../_models/VMReply';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class ReplyService {
  baseUrl = environment.apiUrl;
  
  constructor(
    private authHttp: AuthHttp,
    private authService: AuthService,
    private http: Http
  ) { }

  create(model: VMReplyAdd): Observable<VMReply>  {
    return this.authHttp.post(this.baseUrl + 'reply/create/', model).map(response => <VMReply>response.json());
  }

  search(suggestionId: number): Observable<VMReply[]> {
    return this.http.get(this.baseUrl + 'reply/search/' + suggestionId, this.requestOptions()).map(response => <VMReply[]>response.json());
  }
  private requestOptions() {
    const headers = new Headers({ 'Content-type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }
}
