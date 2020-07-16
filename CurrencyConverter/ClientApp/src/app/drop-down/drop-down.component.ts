import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IKeyValuePair } from '../../interfaces/IKeyValuePair';

@Component({
  selector: 'app-drop-down',
  templateUrl: './drop-down.component.html',
  styleUrls: ['./drop-down.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DropDownComponent implements OnInit {
  @Input() pairs: Map<any, any>;
  @Output() onValueChanged: EventEmitter<any> = new EventEmitter();

  constructor() {
  }

  ngOnInit() {
  }

  onValueChange(value: any) {
    this.onValueChanged.emit(value)
  }

}
