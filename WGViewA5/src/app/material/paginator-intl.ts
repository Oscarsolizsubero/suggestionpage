import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material';
import { TranslateService } from "@ngx-translate/core";

export class PaginatorIntlService  extends MatPaginatorIntl {
  translate: TranslateService;

  getRangeLabel = (page: number, pageSize: number, length: number): string => {
    const of = this.translate ? this.translate.instant('Paginator.LabelOf') : 'of';
    if (length === 0 || pageSize === 0) {
      return '0 ' + of + ' ' + length;
    }
    length = Math.max(length, 0);
    const startIndex = ((page * pageSize) > length) ?
      (Math.ceil(length / pageSize) - 1) * pageSize :
      page * pageSize;
    const endIndex = Math.min(startIndex + pageSize, length);
    return startIndex + 1 + ' - ' + endIndex + ' ' + of + ' ' + length;
  };

  injectTranslateService(translate: TranslateService) {
    this.translate = translate;
    this.translate.onLangChange.subscribe(() => {
      this.translateLabels();
    });
    this.translateLabels();
  }

  translateLabels() {
    this.itemsPerPageLabel = this.translate.instant('Paginator.LabelItemsPerPage');
    this.firstPageLabel = this.translate.instant('Paginator.LabelFirstPage');
    this.lastPageLabel = this.translate.instant('Paginator.LabelLastPage');
    this.previousPageLabel = this.translate.instant('Paginator.LabelPreviousPage');
    this.nextPageLabel = this.translate.instant('Paginator.LabelNextPage');
  }

}
