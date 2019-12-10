import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { AuthService } from '../_services/auth.service';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../environments/environment';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private translate: TranslateService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    environment.requestedURL = state.url;
    if (this.authService.loggedIn()) {
      return true;
    }
    this.snackBar.open(this.translate.instant('SnackBar.RestrictedArea'), "", { duration: 2000, });
    this.router.navigate(['/home']);
    return false;
  }
}
