import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IAlert } from '../../interfaces/IAlert';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css'],
})

export class AlertComponent implements OnInit {
  @Input() alerts: IAlert[];
  @Output() onAlertClicked: EventEmitter<number> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onAlertClick(index: number) {
    this.onAlertClicked.emit(index);
  }
}
