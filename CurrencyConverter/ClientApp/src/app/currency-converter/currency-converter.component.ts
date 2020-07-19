import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, combineLatest, interval, Subscription } from 'rxjs';
import { map } from "rxjs/operators";
import { AlertType } from '../../enums/AlertType';
import { environment } from '../../environments/environment';
import { IAlert } from '../../interfaces/IAlert';
import { ICalendar } from '../../interfaces/ICalendar';
import { IKeyValuePair } from '../../interfaces/IKeyValuePair';
import { ISingleDayCurrencies } from '../../interfaces/ISingleDayCurrencies';

@Component({
  selector: 'app-currency-converter',
  templateUrl: './currency-converter.component.html',
  styleUrls: ['./currency-converter.component.css']
})

export class CurrencyConverterComponent implements OnInit {
  private baseApiUrl = environment.apiUrl;
  private currenciesRefreshInterval = interval(environment.currencyRefreshIntervalInMinutes * 60 * 1000);
  private refreshIntervalSubscription: Subscription;

  private calendarMinDate: ICalendar;
  private calendarMaxDate: ICalendar;

  private currenciesDate: string;
  private currencies: IKeyValuePair[];
  private alerts: IAlert[];

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
    this.getCurrencies();
  };

  ngOnInit(): void {
    this.refreshIntervalSubscription = this.currenciesRefreshInterval.subscribe(x => this.getCurrencies());
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

  onCalendarDateSelect(date: ICalendar) {
    this.getCurrencies(`${date.year}-${date.month}-${date.day}`);
  }

  onAlertClick(index: number) {
    let selectedAlert = this.alerts[index];
    this.alerts = this.alerts.filter(x => x !== selectedAlert);
  }

  private addAlert(type: AlertType, text: string) {
    if (this.alerts === undefined) {
      this.alerts = [] as IAlert[];
    }

    this.alerts.push({ type: type, text: text } as IAlert);
  }

  private setupCalendar() {
    this.calendarMinDate = { year: 1999, month: 1, day: 4 };

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

  private getCurrencies(date?: string) {
    let url = `${this.baseApiUrl}/currency`;
    if (date) {
      url += `/${date}`;
    }

    return this.httpClient.get<ISingleDayCurrencies>(url).subscribe(result => {
      if (result === null) {
        this.addAlert(AlertType.Warning, "No Currencies Found");
        return;
      }

      this.currenciesDate = result.date;
      this.currencies = result.currencies.map(cur => {
        return {
          key: cur.code,
          value: cur.ratio
        } as IKeyValuePair
      }, error => this.addAlert(AlertType.Error, "Failed to handle Currencies"));
    }, error => this.addAlert(AlertType.Error, "Failed to fetch Currencies"));
  }
}
