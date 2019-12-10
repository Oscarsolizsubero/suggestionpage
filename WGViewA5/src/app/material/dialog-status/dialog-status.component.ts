import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-dialog-status',
  templateUrl: './dialog-status.component.html',
  styleUrls: ['./dialog-status.component.css']
})
export class DialogStatusComponent implements OnInit {
  warning: string = this.getWarning();
  statusDescription: string;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private translate: TranslateService
  ) { }

  ngOnInit() {
  }

  status(objStatus) {
    return objStatus.description;
  }

  getWarning() {
    if (this.data.objStatus.id == 2) {
      return this.translate.instant('Dialog.StatusWarningSuggested');
    }
    else {
      return this.translate.instant('Dialog.StatusWarning');
    }
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
