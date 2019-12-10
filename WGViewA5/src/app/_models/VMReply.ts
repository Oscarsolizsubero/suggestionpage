import { VMUserSimple } from "./VMUser";

export interface VMReplyAdd {
  description?: string;
  idAuthor: number;
  idSuggestion?: number;
}

export interface VMReply extends VMReplyAdd {
  id: number;
  createdDate?: Date;
  updatedDate?: Date;
  author: VMUserSimple;
}
