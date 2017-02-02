import { Component,ViewChildren,OnInit, HostListener, Inject, ElementRef } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../_common/services/http.service';
import 'rxjs/Rx';


let styles = require('../styles/home.component.scss').toString();
let tpls = require('../tpls/home.component.html').toString();

@Component({
  selector: 'home',
  styles : [ styles ],
  providers:[SettingsService, CService],
  template : tpls
})

export class HomeComponent implements OnInit {
  private settings : any ;
  private baseUrl : any ;
  private  data : any;
  private cardUrl = 'http://in-it0289/ListingAPI/api/Listings/GetTopListings';
  private bannerUrl = 'http://in-it0289/MasterDataAPI/api/category/GetAllCategory';
  private selectedFilter: string = '';
  public initialCardData: any;
  public bannerData: any;
  public filterCat:any;
  public navIsFixed: boolean = false;
  private topNavbar:any;
  private affixEl:any;
  private affixElOffsetTop: number;

  @ViewChildren("cheader") CHeader;

  constructor(
      public appState: AppState,
      private _settingsService: SettingsService,
      private _cservice:CService,
      private el:ElementRef) {
  }
  ngOnInit() {
    this.baseUrl=this._settingsService.getBaseUrl();
    this.getInitialCards();
    this.getBannerListing();

    this.getAffixElOffsetTop();
  }
  getAffixElOffsetTop(){
    this.affixEl = this.el.nativeElement.querySelector('#cheader1');
    this.affixElOffsetTop = this.affixEl.offsetTop;
    //console.log('off = ',this.affixElOffsetTop);
  }
  getInitialCards (){
    this._cservice.observableGetHttp(this.cardUrl,null,false)
      .subscribe((res:Response)=> {
          this.initialCardData = res;
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }

   getBannerListing (){
     this._cservice.observableGetHttp(this.bannerUrl,null,false)
       .subscribe((res:Response)=> {
           this.bannerData = res;
         },
         error => {
           console.log("error in response");
         },
         ()=>{
           console.log("Finally");
         })
   }

    showOutput(category){
        this.initialCardData = category;
    }
    getSelectedFilter(selectedOpt){
      this.selectedFilter = selectedOpt;
    }

    @HostListener("window:scroll", ['$event'])
    onWindowScroll(e) {
        let number = e.target.body.scrollTop+125;
        if (number > this.affixElOffsetTop) {
            this.navIsFixed = true;
        } else if (this.navIsFixed && number < this.affixElOffsetTop) {
            this.navIsFixed = false;
        }
    }
}
