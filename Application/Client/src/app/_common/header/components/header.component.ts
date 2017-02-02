import { Component,OnInit } from '@angular/core';
import { AppState } from '../../../app.service';
import {AuthenticationWindowService} from '../../authentication/services/authentication.service';
import {SettingsService} from '../../services/setting.service';
import {Http, Headers, RequestOptions, Jsonp} from '@angular/http';
import {UserInformation} from '../../authentication/entity/userInformation.entity';
import {Session} from '../../authentication/entity/session.entity';
import {Router} from '@angular/router';
import {CookieService} from 'angular2-cookie/core';





let styles = require('../styles/header.component.scss').toString();
let tpls = require('../tpls/header.component.html').toString();

@Component({
  selector:'header',
  styles:[styles],
  providers:[AuthenticationWindowService, SettingsService, CookieService],
  template: tpls
})
export class HeaderComponent implements OnInit{
  private session : Session;
  private activeSession:boolean = false;
  public isCollapsed:boolean = true;
  public  isCreateCardVisible: boolean = false;


  constructor(public _authenticationWindowService: AuthenticationWindowService,
              private _settingsService: SettingsService,
              private _http: Http,
              private _router:Router,
              private _cookieService:CookieService){}

  ngOnInit(){
    this.session = new Session( this._cookieService.getObject( 'SESSION_PORTAL' ) );
    this.activeSession = (this.session && this.session.isValid());
    //console.log(this.activeSession);
    if(!this.activeSession){
      this.doLogout();
    }
  }

    public collapsed(event:any):void {
        //console.log(event);
    }

    public expanded(event:any):void {
        //console.log(event);
    }

  createCard() {
    console.log('Card show');
    this.isCreateCardVisible = true;

  }

  doLogout(){
    this._authenticationWindowService.doLogOut();
  }


}

