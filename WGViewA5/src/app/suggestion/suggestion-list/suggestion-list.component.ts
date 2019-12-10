import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { MaxLengthValidator } from '@angular/forms/src/directives/validators';
import { MatSnackBar } from '@angular/material';
import { AuthService } from '../../_services/auth.service';
import { SuggestionService } from '../../_services/suggestion.service';
import { VMSuggestionAdd, VMSuggestion } from '../../_models/VMSuggestion';
import { VMVote } from '../../_models/VMVote';
import { RequestPaginatorWithUser } from '../../_models/VMFilter';
import { element } from 'protractor';
import { TranslateService } from '@ngx-translate/core';
import { VMStatus } from '../../_models/VMStatus';
import { StatusService } from '../../_services/status.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-suggestion-list',
  templateUrl: './suggestion-list.component.html',
  styleUrls: ['./suggestion-list.component.css']
})
export class SuggestionListComponent implements OnInit {
  models: VMSuggestion[];
  statuses: VMStatus[] = this.getStatuses();
  showMoreButton = true;
  showSearchLabel = false;
  showResultLabel = false;
  disabled = false;
  pages: number;
  searched: string;
  URLDetail: string = this.authService.getDomain(window.location.href, true);
  statusDescription: string;

  paginator: RequestPaginatorWithUser = {
    idUser: 0,
    filters: "",
    pageSize: 10,
    tenant: this.authService.getDomain(window.location.href, true),
    statusId: environment.currentstatusfilter
  }

  modelvote: VMVote = {
    userId: 0
  };

  constructor(
    private authService: AuthService,
    private suggestionService: SuggestionService,
    private statusService: StatusService,
    private router: Router,
    private snackBar: MatSnackBar,
    private translate: TranslateService
  ) { }

  ngOnInit() {
    if (this.authService.decodedToken) {
      this.paginator.idUser = this.authService.decodedToken.nameid;
      this.paginator.tenant = this.authService.decodedToken.aud;
      this.modelvote.userId = this.authService.decodedToken.nameid
    }
    this.search();
  }
  getStatuses(): VMStatus[] {
    if (this.authService.decodedToken) {
      this.statusService.searchPrivate().subscribe((statuses: VMStatus[]) => {
        this.statuses = statuses;
        return statuses;
      });

    }
    else {
      this.statusService.searchPublic().subscribe((statuses: VMStatus[]) => {
        this.statuses = statuses;
        return statuses;
      });
    }
    return null;
  }

  ifToken() {
    if (this.authService.decodedToken) {
      return true;
    }
    return false;
  }

  search() {
    this.pageCount();
    if (this.authService.decodedToken) {
      this.suggestionService.searchPrivate(this.paginator).subscribe((models: VMSuggestion[]) => {
        this.models = models;
        if (this.paginator.filters != "" && this.paginator.pageNumber != 1) {
          this.pages = this.paginator.pageNumber;
        }
        this.updateSearchLabel();
        this.searched = "''" + this.paginator.filters + "''.";
        this.updateShowMoreButton();
      }, error => {
        console.log(error);
      });
    }
    else {
      this.suggestionService.search(this.paginator).subscribe((models: VMSuggestion[]) => {
        this.models = models;
        if (this.paginator.filters != "" && this.paginator.pageNumber != 1) {
          this.pages = this.paginator.pageNumber;
        }
        this.updateSearchLabel();
        this.searched = "''" + this.paginator.filters + "''.";
        this.updateShowMoreButton();
      }, error => {
        console.log(error);
      });
    }
  }

  searchByStatus(statusId) {
    this.paginator.statusId = statusId;
    environment.currentstatusfilter = statusId;
    this.search();
  }

  nextPage() {
    this.disabled = true;
    this.paginator.pageNumber++;
    if (this.authService.decodedToken) {
      this.suggestionService.searchPrivate(this.paginator).subscribe((models: VMSuggestion[]) => {
        this.models.push.apply(this.models, models);
        this.updateShowMoreButton();
        this.disabled = false;
      }, error => {
        console.log(error);
      })
    }
    else {
      this.suggestionService.search(this.paginator).subscribe((models: VMSuggestion[]) => {
        this.models.push.apply(this.models, models);
        this.updateShowMoreButton();
        this.disabled = false;
      }, error => {
        console.log(error);
      })
    }
  }

  pageCount() {
    this.paginator.pageNumber = 1;
    this.pages = 1;
    if (this.authService.decodedToken) {
      this.suggestionService.sizePrivate(this.paginator).subscribe((count: number) => {
        this.pages = parseInt((count / this.paginator.pageSize).toPrecision());
        if ((count % this.paginator.pageSize) != 0) {
          this.pages++;
        }
        if (count < 1) {
          this.showResultLabel = true;
        }
        else {
          this.showResultLabel = false;
        }
      });
    }
    else {
      this.suggestionService.size(this.paginator).subscribe((count: number) => {
        this.pages = parseInt((count / this.paginator.pageSize).toPrecision());
        if ((count % this.paginator.pageSize) != 0) {
          this.pages++;
        }
        if (count < 1) {
          this.showResultLabel = true;
        }
        else {
          this.showResultLabel = false;
        }
      });
    }
  }

  updateShowMoreButton() {
    if (this.paginator.pageNumber == this.pages || this.pages == 0) {
      this.showMoreButton = false;
    }
    else {
      this.showMoreButton = true;
    }
  }

  updateSearchLabel() {
    if (this.paginator.filters != "") {
      this.showSearchLabel = true;
    }
    else {
      this.showSearchLabel = false;
    }
  }

  getDetail(model: VMSuggestion) {
    this.suggestionService.modeldetail = model;
    this.router.navigate(['suggestion-detail', model.id]);
  }

  isPermit() {
    return this.authService.loggedIn();
  }

  enableVote(model) {
    if (this.authService.decodedToken.nameid == model.author.id) { return false; }
    else { return true; }
  }

  //make the button of voting change color when user has already voted
  getColor(isVoted: boolean) {
    if (isVoted) {
      return '#52c823';
    }
    else {
      return '#bdc1c4';
    }
  }

  //increase or decrease the vote of the selected suggestion when click on the thumb button
  votebutton(model: VMSuggestion) {
    if (this.authService.decodedToken) {
      this.modelvote.suggestionId = model.id;
      if (model.isVoted) {
        this.suggestionService.deletevote(this.modelvote).subscribe(next => {
          model.isVoted = false;
          model.quantityVote--;
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
          model.isVoted = true;
          model.quantityVote++;
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
      this.router.navigate(['home']);
    }
  }

  showToolbarStatus(model) {
    if (this.authService.decodedToken) {
      if (this.authService.decodedToken.role == 2 || this.authService.decodedToken.role == 3) {
        return true;
      }
      else {
        if (this.authService.decodedToken.role == 4 && model.status.id == 4) {
          return true;
        }
        else {
          return false;
        }
      }
    }
    else {
      if (model.status.id == 4) {
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

  shareFacebook(model) {
    window.open(this.getURLDetailFaceebok(model), '_blank');
  }

  getURLDetailCopy(model): string {
    this.snackBar.open(this.translate.instant('SnackBar.CopyUrl'), "", { duration: 2000, });
    return this.translate.instant('SuggestionDetail.textClipBoard') + " http://" + this.URLDetail + "/#/suggestion-detail/";
  };

  getURLDetailFaceebok(model: VMSuggestion): string {
    return "https://www.facebook.com/sharer/sharer.php?u=http%3A//" + this.URLDetail + "/%23/suggestion-detail/" + model.id;
  }

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

  getCurrentStatus() {
    this.getDescriptionStatus(environment.currentstatusfilter);
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
      case 4: {
        this.statusDescription = 'Fulfilled';
        break;
      }
      default: {
        this.statusDescription = 'All';
      }
    }
  }
}
