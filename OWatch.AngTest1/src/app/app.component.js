"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
var optionsservice_service_1 = require("./optionsservice.service");
var AppComponent = /** @class */ (function () {
    function AppComponent(http, optionsService) {
        var _this = this;
        this.http = http;
        this.optionsService = optionsService;
        // pager object
        this.pager = {};
        this.optionsService.getCurrentBn().subscribe(function (emps) {
            _this.allItems = emps;
            _this.setPage(1);
        }, function (error) { return _this.errorMessage = error; });
    }
    AppComponent.prototype.getRefreshData = function (http, optionsService) {
        var _this = this;
        this.optionsService.getRefreshData().subscribe(function (refreshValue) {
            _this.RefreshValue = refreshValue;
        }, function (error) { return _this.errorMessage = error; });
    };
    AppComponent.prototype.getCurrentBn2 = function (http, optionsService) {
        var _this = this;
        this.optionsService.getCurrentBn2().subscribe(function (emps) {
            _this.allItems = emps;
            _this.setPage(1);
        }, function (error) { return _this.errorMessage = error; });
    };
    AppComponent.prototype.getFinalBn = function (http, optionsService) {
        var _this = this;
        this.optionsService.getFinalBn().subscribe(function (emps) {
            _this.allItems = emps;
            _this.setPage(1);
        }, function (error) { return _this.errorMessage = error; });
    };
    AppComponent.prototype.getNextN = function (http, optionsService) {
        var _this = this;
        this.optionsService.getNextN().subscribe(function (emps) {
            _this.allItems = emps;
            _this.setPage(1);
        }, function (error) { return _this.errorMessage = error; });
    };
    AppComponent.prototype.setPage = function (page) {
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
    };
    AppComponent.prototype.getPager = function (totalItems, currentPage, pageSize) {
        if (currentPage === void 0) { currentPage = 1; }
        if (pageSize === void 0) { pageSize = 10; }
        // calculate total pages
        var totalPages = Math.ceil(totalItems / pageSize);
        // ensure current page isn't out of range
        if (currentPage < 1) {
            currentPage = 1;
        }
        else if (currentPage > totalPages) {
            currentPage = totalPages;
        }
        var startPage, endPage;
        if (totalPages <= 10) {
            // less than 10 total pages so show all
            startPage = 1;
            endPage = totalPages;
        }
        else {
            // more than 10 total pages so calculate start and end pages
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            }
            else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            }
            else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }
        // calculate start and end item indexes
        var startIndex = (currentPage - 1) * pageSize;
        var endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);
        // create an array of pages to ng-repeat in the pager control
        var pages = Array.from(Array((endPage + 1) - startPage).keys()).map(function (i) { return startPage + i; });
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
    };
    AppComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'my-app',
            templateUrl: 'app.component.html'
        }),
        __metadata("design:paramtypes", [http_1.Http, optionsservice_service_1.OptionsService])
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map