import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegistrationValidator } from '../../material/register.validator';
import { UserService } from '../../_services/user.service';
import { VMUserAdd } from '../../_models/VMUser';
import { MatDialog, MatSnackBar } from '@angular/material';
import { AuthService } from '../../_services/auth.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  validReCaptcha: boolean = false;
  message: string = '';
  resetPasswordGroup: FormGroup;

  model: VMUserAdd = {
    username: '',
    password: '',
    name: '',
    lastName: '',
    tenant: this.authService.getDomain(window.location.href, true)
  };

  constructor(
    private userService: UserService,
    private router: Router,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private translate: TranslateService
  ) {
    this.resetPasswordGroup = this.formBuilder.group({
      email: ['', [Validators.required, this.isEmailValid('email')]],
    });
  }

  ngOnInit() {
  }

  resetPassword() {
    this.userService.resetPassword(this.model).subscribe(next => {
      this.router.navigate(['home']);
      this.snackBar.open(this.translate.instant('SnackBar.ResetPasswordSuccessful'), "", { duration: 10000, });
    }, error => {
      this.snackBar.open(this.translate.instant('SnackBar.ResetPasswordFailed'), "", { duration: 2000, });
    });
  }

  isEmailValid(email) {
    return email => {
      var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      return regex.test(email.value) ? null : { invalidEmail: true };
    }
  }

  receiveMessage($event) {
    this.message = $event;
    if (this.message != '') {
      this.validReCaptcha = true;
    }
  }
}
