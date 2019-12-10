import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegistrationValidator } from '../../material/register.validator';
import { VMUser, VMUserPassword } from '../../_models/VMUser';
import { UserService } from '../../_services/user.service';
import { MatSnackBar, MatDialog } from '@angular/material';
import { AuthService } from '../../_services/auth.service';
import { TranslateService } from '@ngx-translate/core';
import { environment } from '../../../environments/environment';
import { DialogChangePasswordComponent } from '../../material/dialog-change-password/dialog-change-password.component';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  registrationFormGroup: FormGroup;
  passwordFormGroup: FormGroup;
  passwordBackup: string;
  hide: boolean = true;
  hidestring: string = 'password';
  visibility: string = 'visibility_off';
  model: VMUserPassword = {
    id: this.authService.decodedToken.nameid,
    oldPassword: '',
    password: ''
  }

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar,
    private authService: AuthService,
    private translate: TranslateService,
    private dialog: MatDialog
  ) {
    this.passwordFormGroup = this.formBuilder.group({
      newPassword: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }, {
        validator: RegistrationValidator.validate.bind(this)
      });
    this.registrationFormGroup = this.formBuilder.group({
      oldPassword: ['', Validators.required],
      passwordFormGroup: this.passwordFormGroup
    });
  }

  ngOnInit() {
  }

  changeHide() {
    if (this.hide) {
      this.hidestring = 'text';
      this.visibility = 'visibility';
    }
    else {
      this.hidestring = 'password';
      this.visibility = 'visibility_off';
    }
    this.hide = !this.hide;
  }
  changePassword() {
    this.userService.editPassword(this.model).subscribe((m: any) => {
      this.openDialog();
      this.logout();
    }, error => {
      if (error.status == 406) {
        this.snackBar.open(this.translate.instant('SnackBar.ChangePasswordIncorrect'), "", { duration: 2000, });
      }
      else {
        this.snackBar.open(this.translate.instant('SnackBar.ChangePasswordFailed'), "", { duration: 2000, });
      }
    });
  }

  cancel() {
    this.router.navigate(['suggestion-list']);
  }
  logout() {
    environment.requestedURL = null;
    environment.currentstatusfilter = 0;
    this.authService.userToken = null;
    this.authService.decodedToken = null;
    localStorage.removeItem('token');
    this.router.navigate(['/home']);
  }
  openDialog(): void {
    let dialogRef = this.dialog.open(DialogChangePasswordComponent, {
      width: '250px',
      data: {}
    });
  }
}
