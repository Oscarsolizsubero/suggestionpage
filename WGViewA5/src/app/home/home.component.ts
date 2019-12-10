import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { AuthService } from '../_services/auth.service';
import { environment } from '../../environments/environment';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  hide = true;
  model: any = { tenant: this.getDomain(window.location.href, true) };

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private translate: TranslateService
  ) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(data => {
      this.snackBar.open(this.translate.instant('SnackBar.LoginSuccessful'), "", {duration:2000,});
    }, error => {
      this.snackBar.open(this.translate.instant('SnackBar.LoginFailed'), "", { duration: 2000, });
      }, () => {
        if (environment.requestedURL == null) {
          this.router.navigate(['suggestion-list']);
        }
        else {
          this.router.navigate([environment.requestedURL]);
        }
      });
  }
  
  loggedIn() {
    return this.authService.loggedIn();
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
