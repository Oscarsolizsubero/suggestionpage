import { VMUserSimple } from "./VMUser";

export interface VMTenant {
  id?: number;
  tenantURL?: string;
  moderation?: boolean;
}

export interface VMTenantEdit extends VMTenant {
  loggedUserId: number;
  loggedUserRole: number;
}

export interface VMTenantImage extends VMTenant
{
  urlImage: string;
  urlTenant: string;
}
