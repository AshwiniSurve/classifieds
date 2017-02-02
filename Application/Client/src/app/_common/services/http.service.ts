import {Injectable} from '@angular/core';
import {Response, Http, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';
import {CookieService} from 'angular2-cookie/core';

declare var $: any;

@Injectable()
export class CService {

  public componentVersion: string;
  public componentName: string;
  public token: string;
  public code: string;
  private sessionObj;

  constructor( public _http: Http,private _cookieService:CookieService) {
  }

  private getHeaders(): Headers {

    let headers = new Headers();
    this.sessionObj = this._cookieService.getObject('SESSION_PORTAL');
    headers.append( 'Content-Type', 'application/jsonp; charset=UTF-8' );
    headers.append('AccessToken',this.sessionObj.token);
    headers.append('UserEmail',this.sessionObj.useremail);
    return headers;
  }

  public getRequestOptions(): RequestOptions {
    let requestOptions = new RequestOptions();
    //requestOptions.withCredentials = false;
    requestOptions.headers = this.getHeaders();
    return requestOptions;
  }

  public observableGetHttp( url: string, options: RequestOptions, native: boolean ): Observable<Response> {
    return this._http.get( url, (native ? options: this.getRequestOptions()) )
      .map( this.extractData )
      .catch( this.handleErrorObservable );
  }

  public observablePostHttp( url: string, data: any, options: RequestOptions, native: boolean ): Observable<Response> {
    return this._http.post( url, data, (native ? options: this.getRequestOptions()) )
      .map( this.extractData )
      .catch( this.handleErrorObservable );
  }

  public observablePutHttp( url: string, data: any, options: RequestOptions, native: boolean ): Observable<Response> {
    return this._http.put( url, data, (native ? options: this.getRequestOptions()) )
      .map( this.extractData )
      .catch( this.handleErrorObservable );
  }

  public observableDeleteHttp( url: string, options: RequestOptions, native: boolean ): Observable<Response> {
    return this._http.delete( url, (native ? options: this.getRequestOptions()) )
      .map( this.extractData )
      .catch( this.handleErrorObservable );
  }

  public promiseGetHttp( url: string, options: RequestOptions, native: boolean ): Promise<any> {
    return this._http.get( url, (native ? options: this.getRequestOptions()) )
      .toPromise()
      .then( this.extractData )
      .catch( this.handlePromise );
  }

  public promisePostHttp( url: string, data: any, options: RequestOptions, native: boolean ): Promise<any> {
    return this._http.post( url, data, (native ? options: this.getRequestOptions()) )
      .toPromise()
      .then( this.extractData )
      .catch( this.handlePromise );
  }

  public promisePutHttp( url: string, data: any, options: RequestOptions, native: boolean ): Promise<any> {
    return this._http.put( url, data, (native ? options: this.getRequestOptions()) )
      .toPromise()
      .then( this.extractData )
      .catch( this.handlePromise );
  }

  public promiseDeleteHttp( url: string, options: RequestOptions, native: boolean ): Promise<any> {
    return this._http.delete( url, (native ? options: this.getRequestOptions()) )
      .toPromise()
      .then( this.extractData )
      .catch( this.handlePromise );
  }

  public extractData( response: Response ) {
    let body = (response.status == 200);
    try {
      let json = response.json();
      body = json;
    } catch ( e ) {
    }
    return body || null;
  }

  public handleErrorObservable( error: any ) {
    return Observable.throw( {'status':error.status,'message': (error.message) ? error.message: error.status ? `${error.status} - ${error.statusText}`: 'Server error'} );
  }

  public handlePromise( error: any ) {
    return Promise.reject( {'status':error.status,'message': (error.message) ? error.message: error.status ? `${error.status} - ${error.statusText}`: 'Server error'} );
  }

  public objectToArray( _json: any, _type: any ): Array<any> {
    return Object.keys( _json ).map( ( key )=> {
      if ( _type ) {
        return new _type( _json[ key ] );
      } else {
        return _json[ key ];
      }
    } );
  }

  public arrayToArrayType( _array: any, _type: any ): Array<any> {
    let array: Array<any> = [];
    _array.forEach( ( val, key ) => {
      array.push( new _type( _array[ key ] ) );
    } );
    return this.cleanArray( array );
  }

  public cleanArray( _array: any ) {
    let array: Array<any> = [];
    _array.forEach( ( val, key ) => {
      if ( _array[ key ] && val ) {
        array.push( _array[ key ] );
      }
    } );
    return array;
  };

}
