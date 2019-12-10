import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRouteSnapshot, NavigationEnd } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { MatSnackBar, MatDialog } from '@angular/material';
import { environment } from '../../environments/environment';
import { TranslateService } from '@ngx-translate/core';
import { VMTenant, VMTenantImage } from '../_models/VMTenant';
import { TenantService } from '../_services/tenant.service';
import { DialogLogoutComponent } from '../material/dialog-logout/dialog-logout.component';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  FullTenant: VMTenantImage = {
    urlImage: '',
    urlTenant: ''
  };

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private translate: TranslateService,
    private tenantService: TenantService,
    private dialog: MatDialog,
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    }

    this.router.events.subscribe((evt) => {
      if (evt instanceof NavigationEnd) {
        // trick the Router into believing it's last link wasn't previously loaded
        this.router.navigated = false;
        // if you need to scroll back to top, here is the right place
        window.scrollTo(0, 0);
      }
    });
  }

  ngOnInit() {
    this.FullTenant = this.getImage(this.authService.getDomain(window.location.href, true));
  }

  logout() {
    let dialogRef = this.dialog.open(DialogLogoutComponent, {
      width: '250px',
      data: {}
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        environment.requestedURL = null;
        environment.currentstatusfilter = 0;
        this.authService.userToken = null;
        this.authService.decodedToken = null;
        localStorage.removeItem('token');
        this.snackBar.open(this.translate.instant('SnackBar.LogoutSuccessful'), "", { duration: 2000, });
        this.router.navigate(['/home']);
      }
    });

    
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  showLogin() {
    if (!this.loggedIn()) {
      if (this.router.url == '/home') {
        return false;
      }
      else {
        return true;
      }
    }
    else {
      return false;
    }
  }

  gotoChangePassword() {
    this.router.navigate(['change-password']);
  }

  gotoModeration() {
    this.router.navigate(['moderation']);
  }

  gotoAbout() {
    this.router.navigate(['about']);
  }

  gotoList() {
    this.router.navigate(['suggestion-list']);
  }

  enableSettings() {
    if (this.authService.decodedToken) {
      if (this.authService.decodedToken.role == 2) {
        return true;
      }
      else { return false; }
    }
  }

  goSiteTenant() {
    window.location.href = this.FullTenant.urlTenant;
  }

  getImage(urlWG): VMTenantImage {
    this.tenantService.getUrlImage(urlWG).subscribe(vmTenantImage => {
      this.FullTenant = vmTenantImage.json();
      return this.FullTenant;
    }, error => {
      return this.FullTenant;
    });
    return this.FullTenant;
  }

  useLanguage(language: string) {
    this.translate.use(language);
  }
}
