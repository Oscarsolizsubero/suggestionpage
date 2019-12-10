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
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  showProfile: boolean = true;
  showAccount: boolean = false;
  validReCaptcha: boolean = false;
  message: string = '';
  registrationProfileGroup: FormGroup;
  passwordAccountGroup: FormGroup;

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
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private translate: TranslateService
  ) {
    this.passwordAccountGroup = this.formBuilder.group({
      newPassword: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }, {
        validator: RegistrationValidator.validate.bind(this)
      });
    this.registrationProfileGroup = this.formBuilder.group({
      name: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, this.isEmailValid('email')]],
    });
  }

  ngOnInit() {
  }

  enableAccount() {
    this.userService.verificationEmail(this.model).subscribe(next => {
      this.showProfile = false;
      this.showAccount = true;
    }, error => {
      this.snackBar.open(this.translate.instant('SnackBar.RegisterUserEmailInUse'), "", { duration: 3000, });
    });
  }

  enableProfile() {
    this.showAccount = false;
    this.showProfile = true;
  }

  userRegister() {
    this.userService.create(this.model).subscribe(next => {
      this.router.navigate(['home']);
      this.snackBar.open(this.translate.instant('SnackBar.RegisterUserSuccessfull'), "", { duration: 4000, });
    }, error => {
      this.snackBar.open(this.translate.instant('SnackBar.RegisterUserFailed'), "", { duration: 2000, });
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
