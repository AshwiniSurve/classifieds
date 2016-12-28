import { Component } from '@angular/core';
import { AppState } from '../../../app.service';

let styles = require('../styles/header.component.scss').toString();
let tpls = require('../tpls/header.component.html').toString();

@Component({
  selector:'header'
  styles:[styles],
  template: tpls
})
export class HeaderComponent{

}