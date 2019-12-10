import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar, MatDialog } from '@angular/material';
import { AuthService } from '../../_services/auth.service';
import { SuggestionService } from '../../_services/suggestion.service';
import { VMSuggestionAdd } from '../../_models/VMSuggestion';
import { VMVote } from '../../_models/VMVote';
import { DialogCreateComponent } from '../../material/dialog-create/dialog-create.component';
import { VMTenant } from '../../_models/VMTenant';
import { TenantService } from '../../_services/tenant.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-suggestion-create',
  templateUrl: './suggestion-create.component.html',
  styleUrls: ['./suggestion-create.component.css']
})
export class SuggestionCreateComponent implements OnInit {
  model: VMSuggestionAdd = {
    title: '',
    description: '',
    idAuthor: 0
  };

  modelvote: VMVote = {
    userId: 0
  };

  @Output() cancelRegister = new EventEmitter();

  constructor(
    private suggestionService: SuggestionService,
    private router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService,
    private dialog: MatDialog,
    private tenantService: TenantService,
    private translate: TranslateService
  ) { }

  ngOnInit() {
    if (this.authService.decodedToken) {
      this.modelvote.userId = this.authService.decodedToken.nameid;
    }
  }

  create() {
    this.suggestionService.modeldetail = null;
    //crear
    this.suggestionService.create(this.model).subscribe(next => {
      this.modelvote.suggestionId = next.json().id;
      //votar automaticamente
      this.suggestionService.vote(this.modelvote).subscribe(next => {
        this.router.navigate(['suggestion-detail', this.modelvote.suggestionId]);
      });

      this.tenantService.loadTenant().subscribe((tenant: VMTenant) => {
        if (tenant.moderation && !(this.authService.decodedToken.role == 2 || this.authService.decodedToken.role == 3)) {
            this.openDialog();
        }
      });
    }, error => {
      this.snackBar.open(this.translate.instant('SnackBar.CreateSuggestionFailed'), "", { duration: 2000, });
    });
  }

  undo() {
    this.cancelRegister.emit(false);
    this.model.title = null;
    this.model.description = null;
  }

  cancel() {
    this.router.navigate(['suggestion-list']);
  }

  isInCreate(): boolean {
    if (this.router.url == '/suggestion-create') {
      return true;
    }
    else {
      return false;
    }
  }

  openDialog(): void {
    let dialogRef = this.dialog.open(DialogCreateComponent, {
      width: '250px',
      data: {}
    });
  }
}
