import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings';
import { ToastService } from './toast-service';
import { LoadingService } from './loading-service';

@Injectable()
export class LoginService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'login';

    getTitle = (): string => 'Login pages';

    getAllThemes = (): Array<any> => {
        return [
            { "title": "Login + logo 1", "theme": "layout1" },
            { "title": "Login + logo 2", "theme": "layout2" }
        ];
    };

    getDataForTheme = (menuItem: any): Array<any> => {
        return this[
            'getDataFor' +
            menuItem.theme.charAt(0).toUpperCase() +
            menuItem.theme.slice(1)
        ]();
    };
    // login + logo 1 data
    getDataForLayout1 = (): any => {
        return {
            "username": "Username",
            "password": "Password",
            "register": "Register",
            "login": "Login",
            "skip": "Skip",
            "logo": "assets/images/logo/login.png",
            "errorUser" : "Field can't be empty.",
            "errorPassword" : "Field can't be empty."
        };
    };

    //Login + logo 2 data
    getDataForLayout2 = (): any => {
        return {
            "username": "Username",
            "password": "Password",
            "register": "Register",
            "login": "Login",
            "skip": "Skip",
            "logo": "assets/images/logo/login.png",
            "errorUser" : "Field can't be empty.",
            "errorPassword" : "Field can't be empty."
        };
    };


    getEventsForTheme = (menuItem: any): any => {
        var that = this;
        return {
            onLogin: function (params) {
                that.toastCtrl.presentToast('onLogin:' + JSON.stringify(params));
            },
            onRegister: function (params) {
                that.toastCtrl.presentToast('onRegister:' + JSON.stringify(params));
            },
            onSkip: function (params) {
                that.toastCtrl.presentToast('onSkip:' + JSON.stringify(params));
            },
            onFacebook: function (params) {
                that.toastCtrl.presentToast('onFacebook:' + JSON.stringify(params));
            },
            onTwitter: function (params) {
                that.toastCtrl.presentToast('onTwitter:' + JSON.stringify(params));
            },
            onGoogle: function (params) {
                that.toastCtrl.presentToast('onGoogle:' + JSON.stringify(params));
            },
            onPinterest: function (params) {
                that.toastCtrl.presentToast('onPinterest:' + JSON.stringify(params));
            },
        };
    };


    prepareParams = (item: any) => {
        let result = {
            title: item.title,
            theme: item.theme,
            data: {},
            events: this.getEventsForTheme(item)
        };
        result[this.getShowItemId(item)] = true;
        return result;
    };

    getShowItemId = (item: any): string => {
        return this.getId() + item.theme.charAt(0).toUpperCase() + "" + item.theme.slice(1);
    }

    load(item: any): Observable<any> {
        var that = this;
        that.loadingService.show();
        if (AppSettings.IS_FIREBASE_ENABLED) {
            return new Observable(observer => {
                this.af
                    .object('login/' + item.theme)
                    .valueChanges()
                    .subscribe(snapshot => {
                        that.loadingService.hide();
                        observer.next(snapshot);
                        observer.complete();
                    }, err => {
                        that.loadingService.hide();
                        observer.error([]);
                        observer.complete();
                    });
            });
        } else {
            return new Observable(observer => {
                that.loadingService.hide();
                observer.next(this.getDataForTheme(item));
                observer.complete();
            });
        }
    }
}
