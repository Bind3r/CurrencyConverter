import { AlertType } from "../enums/AlertType";

export interface IAlert {
  type: AlertType;
  text: string;
}
