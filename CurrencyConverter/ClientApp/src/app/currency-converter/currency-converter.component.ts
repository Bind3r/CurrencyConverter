import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, combineLatest, interval, Subscription } from 'rxjs';
import { map } from "rxjs/operators";
import { environment } from '../../environments/environment';
import { ICalendar } from '../../interfaces/ICalendar';
import { IKeyValuePair } from '../../interfaces/IKeyValuePair';
import { ISingleDayCurrencies } from '../../interfaces/ISingleDayCurrencies';

@Component({
  selector: 'app-currency-converter',
  templateUrl: './currency-converter.component.html',
  styleUrls: ['./currency-converter.component.css']
})

export class CurrencyConverterComponent implements OnInit {
  private currenciesRefreshInterval = interval(environment.currencyRefreshIntervalInMinutes * 60 * 1000);
  private baseApiUrl = environment.apiUrl;
  private refreshIntervalSubscription: Subscription;

  private calendarMinDate: ICalendar;
  private calendarMaxDate: ICalendar;

  private currenciesDate: string;
  private currencies: IKeyValuePair[];
  private ratioFrom = new BehaviorSubject<number>(1);
  private ratioTo = new BehaviorSubject<number>(1);
  private amount = new BehaviorSubject<number>(1);

  conversionResult$ = combineLatest([this.ratioFrom, this.ratioTo, this.amount])
    .pipe(
      map(([ratioFrom, ratioTo, amount]) => this.calculateResult(ratioFrom, ratioTo, amount)));

  ratioDisplay$ = combineLatest([this.ratioFrom, this.ratioTo])
    .pipe(
      map(([ratioFrom, ratioTo]) => this.calculateSelectedCurrenciesRatio(ratioFrom, ratioTo)));

  constructor(private httpClient: HttpClient) {
    this.setupCalendar();
    this.getDailyCurrency();
    //this.getOldCurrency("2020-07-08");
  };

  ngOnInit(): void {
    this.refreshIntervalSubscription = this.currenciesRefreshInterval.subscribe(x => this.getDailyCurrency());
  }

  ngOnDestroy(): void {
    this.refreshIntervalSubscription.unsubscribe();
  }

  onFromCurrencyChange(value: number) {
    this.ratioFrom.next(value);
  }

  onToCurrencyChange(value: number) {
    this.ratioTo.next(value);
  }

  onAmountChange(value: number) {
    this.amount.next(value);
  }

  private setupCalendar() {
    this.calendarMaxDate = { year: 1999, month: 1, day: 4 } as ICalendar;

    let today = new Date();
    this.calendarMaxDate = {
      year: today.getFullYear(),
      month: today.getMonth() + 1,
      day: today.getDate()
    } as ICalendar;
  }

  private calculateResult(ratioFrom: number, ratioTo: number, amount: number) {
    return Math.round((amount / ratioFrom) * ratioTo * 100) / 100;
  }

  private calculateSelectedCurrenciesRatio(ratioFrom: number, ratioTo: number) {
    return Math.round((1 / ratioFrom) / (1 / ratioTo) * 100) / 100;
  }

  private getDailyCurrency() {
    return this.httpClient.get<ISingleDayCurrencies>(this.baseApiUrl + '/currency').subscribe(result => {
      this.currenciesDate = result.date;
      this.currencies = result.currencies.map(cur => {
        return {
          key: cur.code,
          value: cur.ratio
        } as IKeyValuePair
      }, error => alert("No Currencies were found"))
    }, error => alert("Failed to fetch currencies"));
  }

  private getOldCurrency(date: string) {
    return this.httpClient.get<ISingleDayCurrencies>(this.baseApiUrl + '/currency/' + date).subscribe(result => {
      this.currenciesDate = result.date;
      this.currencies = result.currencies.map(cur => {
        return {
          key: cur.code,
          value: cur.ratio
        } as IKeyValuePair
      }, error => alert("No Currencies were found"))
    }, error => alert("Failed to fetch currencies"));
  }
}
