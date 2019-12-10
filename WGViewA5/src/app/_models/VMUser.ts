export interface VMUser extends VMUserSimple {
  level: number;
}

export interface VMUserSimple {
  id?: number;
  fullName?: string;
}

export interface VMUserforList extends VMUserSimple {
  email?: string;
  username?: string;
  moderator?: boolean;
}

export interface VMUserforListEdit extends VMUserforList {
  loggedUserId: number;
  loggedUserRole: number;
  LoggedUserTenant: string;
}

export interface VMUserAdd extends VMUserforList {
  password: string;
  name: string;
  lastName: string;
  tenant?: string;
}

export interface VMUserPassword extends VMUserSimple {
  oldPassword?: string;
  password?: string;
}
