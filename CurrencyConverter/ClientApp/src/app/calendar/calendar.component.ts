import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { ICalendar } from '../../interfaces/ICalendar';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {
  @Input() minDate: ICalendar;
  @Input() maxDate: ICalendar;
  @Output() onDateSelected: EventEmitter<ICalendar> = new EventEmitter();

  constructor() {
  }

  ngOnInit() {
  }

  onDateSelection(date: NgbDate) {
    this.onDateSelected.emit({
      day: date.day,
      month: date.month,
      year: date.year
    } as ICalendar);
  }
}
