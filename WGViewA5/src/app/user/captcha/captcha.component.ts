import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-captcha',
  templateUrl: './captcha.component.html',
  styleUrls: ['./captcha.component.css']
})
export class CaptchaComponent implements OnInit {
  message: string;
  validReset: boolean = true;
  public formModel: FormModel = {};

  @Output() messageEvent = new EventEmitter<string>();

  resolved(captchaResponse: string) {
    this.message = (`${captchaResponse}`);
    this.messageEvent.emit(this.message);
    this.validReset = false;
  }

  constructor() {
  }

  ngOnInit() {
  }
}

export interface FormModel {
  captcha?: string;
}
