
import { Component, OnInit } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'


import { OptionsData } from './optionsData';
import { OptionsService } from "./optionsservice.service"


@Component({
    moduleId: module.id,
    selector: 'my-app',
    templateUrl: 'app.component.html'
})

export class AppComponent {

    private  allItems: OptionsData[]
    errorMessage: string;
    // pager object
    pager: any = {};

    // paged items
    pagedItems: OptionsData[];

    RefreshValue: any;

    constructor(private http: Http, private optionsService: OptionsService) {
        this.optionsService.getCurrentBn().subscribe(
            emps => {
                this.allItems = emps
                this.setPage(1);
            },
            error => this.errorMessage = <any>error
        );
    }

    getRefreshData(http: Http, optionsService: OptionsService) {
        this.optionsService.getRefreshData().subscribe(
            refreshValue => {
                this.RefreshValue = refreshValue;
            },
            error => this.errorMessage = <any>error
        );
    }

    getCurrentBn2(http: Http, optionsService: OptionsService) {
        this.optionsService.getCurrentBn2().subscribe(
            emps => {
                this.allItems = emps
                this.setPage(1);
            },
            error => this.errorMessage = <any>error
        );
    }


    getFinalBn(http: Http, optionsService: OptionsService) {
        this.optionsService.getFinalBn().subscribe(
            emps => {
                this.allItems = emps
                this.setPage(1);
            },
            error => this.errorMessage = <any>error
        );
    }

    getNextN(http: Http, optionsService: OptionsService) {
        this.optionsService.getNextN().subscribe(
            emps => {
                this.allItems = emps
                this.setPage(1);
            },
            error => this.errorMessage = <any>error
        );
    }
    setPage(page: number) {

        if (this.allItems == undefined) {
            // get pager object from service
            this.pager = this.getPager(100, page); //this.allItems.length
        }
        else {
            // get pager object from service
            this.pager = this.getPager(this.allItems.length, page); //this.allItems.length
        }
        if (this.allItems != undefined) {
            // get current page of items
           this.pagedItems = this.allItems.slice(this.pager.startIndex, this.pager.endIndex + 1);
        }
    }
    getPager(totalItems: number, currentPage: number = 1, pageSize: number = 10) {
        // calculate total pages
        let totalPages = Math.ceil(totalItems / pageSize);

        // ensure current page isn't out of range
        if (currentPage < 1) {
            currentPage = 1;
        } else if (currentPage > totalPages) {
            currentPage = totalPages;
        }

        let startPage: number, endPage: number;
        if (totalPages <= 10) {
            // less than 10 total pages so show all
            startPage = 1;
            endPage = totalPages;
        } else {
            // more than 10 total pages so calculate start and end pages
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }

        // calculate start and end item indexes
        let startIndex = (currentPage - 1) * pageSize;
        let endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

        // create an array of pages to ng-repeat in the pager control
        let pages = Array.from(Array((endPage + 1) - startPage).keys()).map(i => startPage + i);

        // return object with all pager properties required by the view
        return {
            totalItems: totalItems,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages
        };
    }
}