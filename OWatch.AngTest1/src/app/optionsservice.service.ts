import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { OptionsData } from "./OptionsData"

@Injectable()
export class OptionsService {
    constructor(private http: Http) { }
    getCurrentBn(): Observable<OptionsData[]> {
        return this.http.get("/api/values/GetNextBN")
            .map(this.extractData)
            .catch(this.handleError);
    }

    getCurrentBn2(): Observable<OptionsData[]> {
        return this.http.get("/api/values/GetNextBN")
            .map(this.extractData)
            .catch(this.handleError);
    }

    getFinalBn(): Observable<OptionsData[]> {
        return this.http.get("/api/values/GetFinalBN")
            .map(this.extractData)
            .catch(this.handleError);
    }

    getNextN(): Observable<OptionsData[]> {
        return this.http.get("/api/values/GetNextN")
            .map(this.extractData)
            .catch(this.handleError);
    }

    getRefreshData(): Observable<Response> {
        return this.http.get("/api/values/GetRefreshData").map(this.extractData)
            .catch(this.handleError);;
    }

    private extractData(res: Response) {
        let body = res.json();
       // this.appComponent.setPage(1);
        return body || {};
    }
    private handleError(error: Response | any) {
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        }
        else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}