import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { UserService } from '../../_services/user.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-account-validation',
  templateUrl: './account-validation.component.html',
  styleUrls: ['./account-validation.component.css']
})
export class AccountValidationComponent implements OnInit {
  model: string;
  constructor(
    private userService: UserService,
    private router: Router,
    private translate: TranslateService,
    private snackBar: MatSnackBar,
 ) {}

  ngOnInit() {
  }

  ValidationUser() {
    this.userService.getValidationUser(this.model).subscribe(next => {
      this.router.navigate(['home'])
      if (next == 201) {
        this.snackBar.open(this.translate.instant('SnackBar.ValidationCorrectCreate'), "", { duration: 3000, });
      }
      if (next == 202) {
        this.snackBar.open(this.translate.instant('SnackBar.ValidationCorrectReset'), "", { duration: 3000, });
      }
      
    }, error => {
      this.snackBar.open(this.translate.instant('SnackBar.CodeIncorrect'), "", { duration: 2000, });
    });
  }
}
