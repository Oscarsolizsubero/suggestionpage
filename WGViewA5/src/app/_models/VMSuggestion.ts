import { VMUserSimple } from "./VMUser";
import { VMStatus } from "./VMStatus";

export interface VMSuggestionAdd {
  description: string;
  title: string;
  idAuthor: number;
}

export interface VMSuggestion extends VMSuggestionAdd {
  id: number;
  quantityVote?: number;
  createdDate?: Date;
  updatedDate?: Date;
  author: VMUserSimple;
  isVoted: boolean;
  status: VMStatus;
}

export interface VMSuggestionEditStatus {
  id?: number;
  statusid?: number;
  userid: number;
}
