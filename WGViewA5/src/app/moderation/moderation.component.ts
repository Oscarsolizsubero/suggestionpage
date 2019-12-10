import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatSnackBar, MatPaginator, MatDialog, MatPaginatorIntl } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { VMUser, VMUserforList, VMUserforListEdit } from '../_models/VMUser';
import { RequestPaginatorWithUser } from '../_models/VMFilter';
import { TenantService } from '../_services/tenant.service';
import { VMTenant, VMTenantEdit } from '../_models/VMTenant';
import { DialogModerationComponent } from '../material/dialog-moderation/dialog-moderation.component';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-moderation',
  templateUrl: './moderation.component.html',
  styleUrls: ['./moderation.component.css']
})
export class ModerationComponent implements OnInit {
  pages: number;
  searched: string;
  modtoggle: boolean;
  tenant: VMTenant = { moderation: false };
  dataSource = new MatTableDataSource();
  displayedColumns = ['position', 'fullName', 'email', 'moderator'];
  index: number = 0;

  //Checkbox
  selection = new SelectionModel<Element>(true, []);

  //Filter
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim();
    filterValue = filterValue.toLowerCase();
    this.dataSource.filter = filterValue;
  }

  tenantForEdit: VMTenantEdit = {
    loggedUserId: this.authService.decodedToken.nameid,
    loggedUserRole: this.authService.decodedToken.role
  }

  userForEdit: VMUserforListEdit = {
    loggedUserId: this.authService.decodedToken.nameid,
    loggedUserRole: this.authService.decodedToken.role,
    LoggedUserTenant: this.authService.decodedToken.aud
  }

  paginator: RequestPaginatorWithUser = {
    idUser: 0,
    filters: "",
    pageSize: 1000000,
    tenant: this.authService.getDomain(window.location.href, true)
  }

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private tenantService: TenantService,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private translate: TranslateService
  ) { }

  ngOnInit() {
    if (this.authService.decodedToken.role != 2) {
      this.router.navigate(['suggestion-list']);
    }
    if (this.authService.decodedToken) {
      this.paginator.idUser = this.authService.decodedToken.nameid;
      this.paginator.tenant = this.authService.decodedToken.aud;
    }
    this.loadModeration();
    this.search();
  }

  @ViewChild(MatPaginator) paginator2: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  loadModeration() {
    this.tenantService.loadTenant().subscribe((tenant: VMTenant) => {
      this.tenant = tenant;
    })
  }

  search() {
    this.pageCount();
    this.userService.search(this.paginator).subscribe((models: VMUserforList[]) => {
      this.dataSource = new MatTableDataSource(models);
      this.dataSource.paginator = this.paginator2;
      this.dataSource.sort = this.sort;
      if (this.paginator.filters != "" && this.paginator.pageNumber != 1) {
        this.pages = this.paginator.pageNumber;
      }
      this.searched = "''" + this.paginator.filters + "''.";
    }, error => {
      console.log(error);
    });
  }

  pageCount() {
    this.paginator.pageNumber = 1;
    this.pages = 1;
    this.userService.size(this.paginator).subscribe((count: number) => {
      this.pages = parseInt((count / this.paginator.pageSize).toPrecision());
    });
  }

  changeLevel(model: VMUserforList) {
    this.userForEdit.id = model.id;
    this.userForEdit.fullName = model.fullName;
    this.userForEdit.email = model.email;
    this.userForEdit.username = model.username;
    model.moderator = !model.moderator;
    this.userForEdit.moderator = model.moderator;
    this.userService.edit(this.userForEdit).subscribe(response => {
      this.snackBar.open(this.translate.instant('SnackBar.ChangeUserLvlSuccess'), "", { duration: 2000, });
    });
  }

  changeModeration() {
    let dialogRef = this.dialog.open(DialogModerationComponent, {
      width: '250px',
    });
    this.tenant.moderation = !this.tenant.moderation;
    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        this.tenantForEdit.id = this.tenant.id;
        this.tenantForEdit.tenantURL = this.tenant.tenantURL;
        this.tenantForEdit.moderation = this.tenant.moderation;
        this.tenantService.edit(this.tenantForEdit).subscribe();
      }
      else { this.tenant.moderation = !this.tenant.moderation;}
    });
  }
}

export interface Element {
  position: number;
  fullName: string;
  email: string;
  moderator: boolean;
}
