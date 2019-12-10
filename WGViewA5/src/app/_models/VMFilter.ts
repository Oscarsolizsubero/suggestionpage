export interface RequestPaginatorWithUser extends RequestPaginatorWithSearch {
  idUser: number;
  tenant: string;
  statusId?: number;
}

export interface RequestPaginatorWithSearch extends RequestPaginator {
  filters: string;
}

export interface RequestPaginator {
  pageSize: number;
  pageNumber?: number;
}
