import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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

  onDateSelect(date: ICalendar) {
    this.onDateSelected.emit(date);
  }
}
