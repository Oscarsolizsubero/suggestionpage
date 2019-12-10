import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { tokenNotExpired, JwtHelper, AuthHttp } from 'angular2-jwt';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment'
import { VMSuggestionAdd, VMSuggestion } from '../_models/VMSuggestion';
import { RequestPaginatorWithUser } from '../_models/VMFilter';
import { VMVote } from '../_models/VMVote';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class SuggestionService {
  baseUrl = environment.apiUrl;
  modeldetail: VMSuggestion;

  constructor(
    private authHttp: AuthHttp,
    private authService: AuthService,
    private http: Http
  ) { }

  create(model: VMSuggestionAdd) {
    model.idAuthor = this.authService.decodedToken.nameid;
    return this.authHttp.post(this.baseUrl + 'suggestion/create', model);
  }

  search(filter: RequestPaginatorWithUser): Observable<VMSuggestion[]> {
    return this.http.post(this.baseUrl + 'suggestion/search/', filter, this.requestOptions()).map(response => <VMSuggestion[]>response.json()).catch(this.handleError);
  }

  searchPrivate(filter: RequestPaginatorWithUser): Observable<VMSuggestion[]> {
    return this.authHttp.post(this.baseUrl + 'suggestion/searchprivate/', filter).map(response => <VMSuggestion[]>response.json()).catch(this.handleError);
  }

  size(paginator: RequestPaginatorWithUser): Observable<number>{
    return this.http.post(this.baseUrl + 'suggestion/size/', paginator, this.requestOptions()).map(response => <number>response.json());
  }

  sizePrivate(paginator: RequestPaginatorWithUser): Observable<number> {
    return this.authHttp.post(this.baseUrl + 'suggestion/sizeprivate/', paginator).map(response => <number>response.json());
  }

  getSingleSuggestion(id, userId) {
    return this.http.get(this.baseUrl + 'suggestion/load/' + this.authService.getDomain(window.location.href, true) + '/' + userId + '/' + id, this.requestOptions()).map((response: Response) => {
      return response.json();
    }).catch(this.handleError);
  }

  remove(id) {
    return this.authHttp.delete(this.baseUrl + 'suggestion/remove/' + id);
  }

  edit(model) {
    return this.authHttp.put(this.baseUrl + 'suggestion/edit', model);
  }
  editStatus(model) {
    return this.authHttp.put(this.baseUrl + 'suggestion/editstatus', model);
  }

  private handleError(error: any) {
    const applicationError = error.headers.get('Application-Error');
    if (applicationError) {
      return Observable.throw(applicationError);
    }
    const serverError = error.json();
    let modelStateErrors = '';
    if (serverError) {
      for (const key in serverError) {
        if (serverError[key]) {
          modelStateErrors += serverError[key] + '\n';
        }
      }
    }
    return Observable.throw(
      modelStateErrors || 'Server error'
    );
  }

  vote(model: VMVote) {
    return this.authHttp.post(this.baseUrl + 'suggestion/vote', model);
  }

  deletevote(model: VMVote) {
    return this.authHttp.delete(this.baseUrl + 'suggestion/vote/' + model.userId + '/' + model.suggestionId);
  }
  //headers for http request
  private requestOptions() {
    const headers = new Headers({ 'Content-type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }



}
