import { Component, OnInit, Input, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { MatSnackBar, MatDialog } from '@angular/material';
import { AuthService } from '../../_services/auth.service';
import { SuggestionService } from '../../_services/suggestion.service';
import { ReplyService } from '../../_services/reply.service';
import { VMVote } from '../../_models/VMVote';
import { VMSuggestion, VMSuggestionEditStatus } from '../../_models/VMSuggestion';
import { VMReply, VMReplyAdd } from '../../_models/VMReply';
import { DialogDelComponent } from '../../material/dialog-del/dialog-del.component';
import { FormControl } from '@angular/forms';
import { VMStatus } from '../../_models/VMStatus';
import { DialogStatusComponent } from '../../material/dialog-status/dialog-status.component';
import { environment } from '../../../environments/environment';
import { TranslateService } from '@ngx-translate/core';
import { StatusService } from '../../_services/status.service';

@Component({
  selector: 'app-suggestion-detail',
  templateUrl: './suggestion-detail.component.html',
  styleUrls: ['./suggestion-detail.component.css']
})

export class SuggestionDetailComponent implements OnInit {
  isDataLoaded: boolean = false;
  showExpansion = 0;
  model: VMSuggestion;
  replies: VMReply[];
  idsuggestion: number;
  edit: boolean = false;
  titlebackup: string;
  descriptionbackup: string;
  showSelectStatus = false;
  showMargin: boolean = false;
  statuses: VMStatus[];
  statusEdit: VMSuggestionEditStatus = {
    userid: this.getId()
  };
  URLDetail: string = this.authService.getDomain(window.location.href, true);
  statusDescription: string;

  reply: VMReplyAdd = {
    idAuthor: this.getId()
  };

  modelvote: VMVote = {
    userId: this.getId()
  };

  constructor(
    private authService: AuthService,
    private suggestionService: SuggestionService,
    private replyService: ReplyService,
    private statusService: StatusService,
    private http: Http,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private translate: TranslateService
  ) {}
  
  ngOnInit() {

    this.showDetail();
    this.searchReplies();
  }

  getId(): number {
    if (this.authService.decodedToken) {
      return this.authService.decodedToken.nameid;
    }
    else {
      return 0;
    }
  }

  showDetail() {
    this.route.params.subscribe(params => {
      this.idsuggestion = params['id'];
    });
    this.model = this.suggestionService.modeldetail;
    if (this.model == undefined) {
      this.suggestionService.getSingleSuggestion(this.idsuggestion, this.modelvote.userId).subscribe((model: VMSuggestion) => {
        this.model = model;
        this.reply.idSuggestion = model.id;
        this.statuses = this.getStatuses(this.model);
        this.isDataLoaded = true;
      }, error => {
        environment.requestedURL = "/suggestion-detail/" + this.idsuggestion;
        this.router.navigate(['suggestion-list']);
      })
    }
    else {
      this.reply.idSuggestion = this.suggestionService.modeldetail.id;
      this.statuses = this.getStatuses(this.model);
      this.isDataLoaded = true;
    }
  }

  searchReplies() {
    this.reply.description = null;
    this.replyService.search(this.idsuggestion).subscribe((replies: VMReply[]) => {
      this.replies = replies;
    }, error => {
      console.log(error);
    });
  }

  createReply() {
    this.replyService.create(this.reply).subscribe((text: VMReply) => {
      this.snackBar.open(this.translate.instant('SnackBar.ReplySuggestionSuccessful'), "", { duration: 2000, });
      this.replies.push(text);
      this.reply.description = "";
      this.closeExpansion();
    }, error => {
      this.snackBar.open(this.translate.instant('SnackBar.ReplySuggestionFailed'), "", { duration: 2000, });
    });
  }

  setStep(index: number) {
    this.showExpansion = index;
  }

  closeExpansion() {
    this.showExpansion++;
  }

  //Buttons
  editbutton() {
    this.titlebackup = this.model.title;
    this.descriptionbackup = this.model.description;
    this.edit = true;
    return this.edit;
  }

  confirmedit() {
    this.model.updatedDate = new Date();
    this.suggestionService.edit(this.model).subscribe();
    this.snackBar.open(this.translate.instant('SnackBar.EditSuggestionSuccessful'), "", { duration: 2000, });
    this.edit = false;
  }

  canceledit() {
    this.edit = false;
    this.model.title = this.titlebackup;
    this.model.description = this.descriptionbackup;
    this.snackBar.open(this.translate.instant('SnackBar.EditSuggestionCanceled'), "", { duration: 2000, });
    return this.edit;
  }

  votebutton() {
    if (this.authService.decodedToken) {
      this.modelvote.suggestionId = this.model.id;
      if (this.model.isVoted) {
        this.suggestionService.deletevote(this.modelvote).subscribe(next => {
          this.model.isVoted = false;
          this.model.quantityVote--;
          this.snackBar.open(this.translate.instant('SnackBar.VoteSuggestionCanceled'), "", { duration: 2000, });
        }, error => {
          if (error.status == 406) {
            this.snackBar.open(this.translate.instant('SnackBar.UnvoteFullfilled'), "", { duration: 2000, });
          }
          else {
            this.snackBar.open(this.translate.instant('SnackBar.VoteSuggestionFailed'), "", { duration: 2000, });
          }
        });

      }
      else {
        this.suggestionService.vote(this.modelvote).subscribe(next => {
          this.model.isVoted = true;
          this.model.quantityVote++;
          this.snackBar.open(this.translate.instant('SnackBar.VoteSuggestionSuccessful'), "", { duration: 2000, });
        }, error => {
          if (error.status == 406) {
            this.snackBar.open(this.translate.instant('SnackBar.VoteFullfilled'), "", { duration: 2000, });
          }
          else {
            this.snackBar.open(this.translate.instant('SnackBar.VoteSuggestionFailed'), "", { duration: 2000, });
          }
        });
      }
    }
    else {
      environment.requestedURL = "/suggestion-detail/" + this.model.id;
      this.router.navigate(['home']);
    }
  }

  openDialog(): void {
    let dialogRef = this.dialog.open(DialogDelComponent, {
      width: '250px',
      data: {}
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        this.suggestionService.remove(this.model.id).subscribe(response => {
          this.router.navigate(['suggestion-list']);
        });
      }
    });
  }

  //booleans check
  enableOptions() {
    if (!this.enableEdit() && !this.enableDelete() || this.model.status.id == 4) { return false; }
    if (this.authService.decodedToken.nameid == this.model.author.id || this.authService.decodedToken.role == 2 || this.authService.decodedToken.role == 3) { return true; }
    else { return false; }
  }

  enableStatus() {
    if (this.authService.decodedToken) {
      if (this.authService.decodedToken.role != 4 && this.model.status.id != 4 && this.model.status.id != 3) { return true; }
    }
    else { return false; }
  }

  enableVote() {
    if (this.authService.decodedToken) {
      if (this.authService.decodedToken.nameid == this.model.author.id) { return false; }
    }
    return true;
  }

  enableEdit() {
    if (this.authService.decodedToken) {
      if ((this.authService.decodedToken.nameid == this.model.author.id) && (this.model.quantityVote < 2)) { return true; }
    }
    else { return false; }
  }

  enableDelete() {
    if (this.authService.decodedToken) {
      if ((this.enableEdit()) || this.authService.decodedToken.role == 2 || this.authService.decodedToken.role == 3) { return true; }
    }
    else { return false; }
  }

  enableReply() {
    if (this.authService.decodedToken) {
      if ((this.authService.decodedToken.role == 2 || this.authService.decodedToken.role == 3) && (this.model.status.id != 4)) { return true; }
    }
    else { return false; }
  }

  getColor() {
    if (this.model.isVoted) {
      return '#52c823';
    }
    else {
      return '#bdc1c4';
    }
  }

  getStatuses(model): VMStatus[] {
    if (this.authService.decodedToken) {
      if (this.authService.decodedToken.role == 2 || this.authService.decodedToken.role == 3) {
        this.statusService.searchForEdit(model.id).subscribe(next => {
          return this.statuses = next;
        }, error => {
          this.router.navigate(['/suggestion-detail/' + this.model.id]);
        });
      }
    }
    return null;
  }

  changeStatus(value) {
    this.statusEdit.id = this.model.id;
    this.statusEdit.statusid = value.id;
    this.openDialogStatus(value);
  }

  openDialogStatus(value): void {
    let dialogRef = this.dialog.open(DialogStatusComponent, {
      width: '250px',
      data: { objStatus: value }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {

        this.suggestionService.editStatus(this.statusEdit).subscribe(next => {
          this.model.status.id = this.statusEdit.statusid;
          this.model.status.description = value.description;
          this.statuses = this.getStatuses(this.model);
          this.snackBar.open(this.translate.instant('SnackBar.ChangeStatusSuccessful'), "", { duration: 2000, });
        }, error => {
          this.snackBar.open(this.translate.instant('SnackBar.ChangeStatusFailed'), "", { duration: 2000, });
          this.suggestionService.getSingleSuggestion(this.idsuggestion, this.modelvote.userId).subscribe((model: VMSuggestion) => {
            this.model = model;
            this.reply.idSuggestion = model.id;
            this.statuses = this.getStatuses(this.model);
            this.isDataLoaded = true;
          }, error => {
            this.router.navigate(['suggestion-list']);
          })
        });
      }
    });
  }

  showToolbarStatus() {
    if (this.authService.decodedToken) {
      if (this.authService.decodedToken.role == 2 || this.authService.decodedToken.role == 3) {
        return true;
      }
      else {
        if (this.authService.decodedToken.role == 4 && this.model.status.id == 4) {
          return true;
        }
        else {
          return false;
        }
      }
    }
    else {
      if (this.model.status.id == 4) {
        return true;
      }
      else {
        return false;
      }
    }
  }

  getColorStatus(model) {
    switch (model.status.id) {
      case 1: { return '#637985' }
      case 2: { return '#006bb1' }
      case 3: { return '#a30000' }
      case 4: { return '#52c823' }
      default: { return '#006bb1' }
    }
  }

  shareFacebook(urlDetail) {
    window.open(this.getURLDetailFaceebok(urlDetail), '_blank');
  }

  getURLDetailFaceebok(urlDetail): string {
    return "https://www.facebook.com/sharer/sharer.php?u=http%3A//" + this.URLDetail + "/%23/suggestion-detail/" + this.idsuggestion;
  };

  getURLDetailCopy(urlDetail): string {
    this.snackBar.open(this.translate.instant('SnackBar.CopyUrl'), "", { duration: 2000, });
    return this.translate.instant('SuggestionDetail.textClipBoard') + " http://" + this.URLDetail + "/#/suggestion-detail/" + this.idsuggestion;
  };

  copyText(val: string) {
    let selBox = document.createElement('textarea');
    selBox.style.position = 'fixed';
    selBox.style.left = '0';
    selBox.style.top = '0';
    selBox.style.opacity = '0';
    selBox.value = val;
    document.body.appendChild(selBox);
    selBox.focus();
    selBox.select();
    document.execCommand('copy');
    document.body.removeChild(selBox);
  }

  getStatusById(idStatus) {
    this.getDescriptionStatus(idStatus);
    return (this.translate.instant('Status.' + this.statusDescription));
  }

  getDescriptionStatus(idStatus) {
    switch (idStatus) {
      case 1: {
        this.statusDescription = 'New';
        break;
      }
      case 2: {
        this.statusDescription = 'Suggested';
        break;
      }
      case 3: {
        this.statusDescription = 'Rejected';
        break;
      }
      default: {
        this.statusDescription = 'Fulfilled';
      }
    }
  }
}
