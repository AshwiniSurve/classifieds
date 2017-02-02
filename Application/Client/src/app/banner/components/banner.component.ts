import { Component,Input,OnInit } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';

let styles = require('../styles/banner.component.scss').toString();
let tpls = require('../tpls/banner.component.html').toString();

@Component({
    selector: 'banner',
    styles : [ styles ],
    providers:[SettingsService],
    template : tpls
})

export class BannerComponent {
    private settings : any ;
    private listingsData : any ;
    localState = { value: '' };
    constructor(public appState: AppState,private _settingsService: SettingsService) {}

  @Input()
    categories;

    ngOnInit() {
        this.listingsData=this._settingsService.getBannerListingsData();
        //console.log('dsf = ',this.listingsData);
    }

    submitState(value: string) {
        //console.log('submitState', value);
        this.appState.set('value', value);
        this.localState.value = '';
    }
}