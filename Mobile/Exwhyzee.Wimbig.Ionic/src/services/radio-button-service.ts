import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings';
import { ToastService } from './toast-service';
import { LoadingService } from './loading-service';

@Injectable()
export class RadioButtonService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'radioButton';

    getTitle = (): string => 'Radio Button';

    getAllThemes = (): Array<any> => {
        return [
          {"title" : "Simple", "theme"  : "layout1"},
          {"title" : "With avatars", "theme"  : "layout2"},
          {"title" : "Simple 2", "theme"  : "layout3"}
        ];
    };

    getDataForTheme = (menuItem: any): any => {
        return this[
            'getDataFor' +
            menuItem.theme.charAt(0).toUpperCase() +
            menuItem.theme.slice(1)
        ]();
    };

    //Simple data
    getDataForLayout1 = (): any => {
        return {
            "selectedItem": 3,
            "items" : [
              {"id" : 1, "title": "Mayotte"},
              {"id" : 2, "title": "El Salvador"},
              {"id" : 4, "title": "Slovak Republic"},
              {"id" : 3, "title": "Myanmar"},
              {"id" : 5, "title": "Sudan"},
              {"id" : 6, "title": "Venezuela"},
              {"id" : 7, "title": "Canada"},
              {"id" : 8, "title": "French Polynesia"},
              {"id" : 9, "title": "Zambia"},
              {"id" : 10, "title": "Libya"},
              {"id" : 11, "title": "Swaziland"},
              {"id" : 12, "title": "Uruguay"},
              {"id" : 13, "title": "Ireland"}
            ]
          };
    };

    //With avatars data
    getDataForLayout2 = (): any => {
        return {
            "selectedItem": 4,
            "items" : [
              {"id" : 1, "title": "Julia Petersen", "subtitle": "@julia333", "avatar": "assets/images/avatar/0.jpg"},
              {"id" : 2, "title": "Holman Valencia", "subtitle": "@holmanval", "avatar": "assets/images/avatar/1.jpg"},
              {"id" : 4, "title": "Marisa Cain", "subtitle": "@marisa", "avatar": "assets/images/avatar/2.jpg"},
              {"id" : 3, "title": "Dejesus Norris", "subtitle": "@norris", "avatar": "assets/images/avatar/3.jpg"},
              {"id" : 5, "title": "Gayle Gaines", "subtitle": "@Gayle", "avatar": "assets/images/avatar/4.jpg"},
              {"id" : 6, "title": "Prince Phelps", "subtitle": "@phelps123", "avatar": "assets/images/avatar/5.jpg"},
              {"id" : 7, "title": "Keri Hudson", "subtitle": "@kerri333", "avatar": "assets/images/avatar/6.jpg"},
              {"id" : 8, "title": "Duran Clayton", "subtitle": "@duran44", "avatar": "assets/images/avatar/7.jpg"},
              {"id" : 9, "title": "Lara Lynn", "subtitle": "@lara", "avatar": "assets/images/avatar/8.jpg"},
              {"id" : 10, "title": "Perry Bradley", "subtitle": "@bradley", "avatar": "assets/images/avatar/9.jpg"}
            ]
          };
    };

    //Simple 2 data
    getDataForLayout3 = (): any => {
        return {
            "selectedItem": 4,
            "items" : [
              {"id" : 1, "title": "Brogan", "subtitle": "Chad"},
              {"id" : 2, "title": "Rehrersburg", "subtitle": "Romania"},
              {"id" : 4, "title": "Durham", "subtitle": "Mauritania"},
              {"id" : 3, "title": "Callaghan", "subtitle": "Tonga"},
              {"id" : 5, "title": "Manitou", "subtitle": "Norway"},
              {"id" : 6, "title": "Weedville", "subtitle": "Northern Mariana Islands"},
              {"id" : 7, "title": "Curtice", "subtitle": "Nauru"},
              {"id" : 8, "title": "Barronett", "subtitle": "Iran"},
              {"id" : 9, "title": "Urie", "subtitle": "Swaziland"},
              {"id" : 10, "title": "Blackgum", "subtitle": "Uruguay"},
              {"id" : 11, "title": "Bannock", "subtitle": "Mayotte"},
              {"id" : 12, "title": "Singer", "subtitle": "El Salvador"},
              {"id" : 13, "title": "Nutrioso", "subtitle": "Slovak Republic"}
            ]
          };
    };

     getEventsForTheme = (menuItem: any): any => {
        var that = this;
        return {
            'onSelect': function(item: any) {
                that.toastCtrl.presentToast(item.title);
            },
        };
    };

    prepareParams = (item: any) => {
        let result = {
            title: item.title,
            data: [],
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
                    .object('radioButton/' + item.theme)
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
