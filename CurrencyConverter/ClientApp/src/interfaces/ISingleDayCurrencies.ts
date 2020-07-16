import { ICurrency } from "./ICurrency";

export interface ISingleDayCurrencies {
  date: string;
  currencies: ICurrency[];
}
