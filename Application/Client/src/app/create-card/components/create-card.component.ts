import { Component,Input,OnInit } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';

let styles = require('../styles/create-card.component.scss').toString();
let tpls = require('../tpls/create-card.component.html').toString();

@Component({
  selector: 'create-card',
  styles : [ styles ],
  providers:[SettingsService],
  template : tpls
})

export class CreateCardComponent {
  private settings : any ;
  private listingsData : any ;
  localState = { value: '' };
  constructor(public appState: AppState,private _settingsService: SettingsService) {}

  @Input() categories;
  @Input() isCreateCardVisible;

  ngOnInit() {
    // this.listingsData=this._settingsService.getBannerListingsData();
    console.log("Create Cards");
  }

  closeCreateCard(){
    this.isCreateCardVisible =false;
  }
}
