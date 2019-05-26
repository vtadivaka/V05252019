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
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/catch");
require("rxjs/add/operator/map");
var OptionsService = /** @class */ (function () {
    function OptionsService(http) {
        this.http = http;
    }
    OptionsService.prototype.getCurrentBn = function () {
        return this.http.get("/api/values/GetNextBN")
            .map(this.extractData)
            .catch(this.handleError);
    };
    OptionsService.prototype.getCurrentBn2 = function () {
        return this.http.get("/api/values/GetNextBN")
            .map(this.extractData)
            .catch(this.handleError);
    };
    OptionsService.prototype.getFinalBn = function () {
        return this.http.get("/api/values/GetFinalBN")
            .map(this.extractData)
            .catch(this.handleError);
    };
    OptionsService.prototype.getNextN = function () {
        return this.http.get("/api/values/GetNextN")
            .map(this.extractData)
            .catch(this.handleError);
    };
    OptionsService.prototype.getRefreshData = function () {
        return this.http.get("/api/values/GetRefreshData").map(this.extractData)
            .catch(this.handleError);
        ;
    };
    OptionsService.prototype.extractData = function (res) {
        var body = res.json();
        // this.appComponent.setPage(1);
        return body || {};
    };
    OptionsService.prototype.handleError = function (error) {
        var errMsg;
        if (error instanceof http_1.Response) {
            var body = error.json() || '';
            var err = body.error || JSON.stringify(body);
            errMsg = error.status + " - " + (error.statusText || '') + " " + err;
        }
        else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable_1.Observable.throw(errMsg);
    };
    OptionsService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.Http])
    ], OptionsService);
    return OptionsService;
}());
exports.OptionsService = OptionsService;
//# sourceMappingURL=optionsservice.service.js.map