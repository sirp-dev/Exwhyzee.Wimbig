import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings'
import { ToastService } from './toast-service'
import { LoadingService } from './loading-service'

@Injectable()
export class FormService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'form';

    getTitle = (): string => 'Form';

    getAllThemes = (): Array<any> => {
        return [
          {"title" : "Form + Write Comment", "theme"  : "layout1"},
          {"title" : "Form + Write Review", "theme"  : "layout2"},
          {"title" : "Form With Address", "theme"  : "layout3"},
          {"title" : "Form add Photo Or  Video", "theme"  : "layout4"}
        ];
    };

    getDataForTheme = (menuItem: any): any => {
        return this[
            'getDataFor' +
            menuItem.theme.charAt(0).toUpperCase() +
            menuItem.theme.slice(1)
        ]();
    };

    getEventsForTheme = (menuItem: any): any => {
        var that = this;
        return {
            onButton: function(item: any) {
                that.toastCtrl.presentToast(item.title);
            },
            onSubmit: function(item: any) {
                that.toastCtrl.presentToast(JSON.stringify(item));
            },
            onAddVideoPhoto: function(item: any) {
                that.toastCtrl.presentToast(item.description);
            }
        };
    };

    getDataForLayout1 = (): any => {
        return {
          "yourName": "Your Name",
          "title": "Title",
          "description": "Enter a description",
          "button": " Write Comment"
      };
    };

    getDataForLayout2 = (): any => {
        return {
            "title":"Continue",
            "rateTitle":"Rate",
            "descriptionPlaceholder":"Description",
            "btnSubmit": "Write Comment",
            "iconsStars": [
                {
                    "isActive": true,
                    "icon": "star"
                },
                {
                    "isActive": true,
                    "icon": "star"
                },
                {
                    "isActive": true,
                    "icon": "star"
                },
                {
                    "isActive": true,
                    "icon": "star"
                },
                {
                    "isActive": false,
                    "icon": "star"
                }
            ],
        };
    };

    getDataForLayout3 = (): any => {
        return {
          "firstName": "Firs Name",
          "lastName": "Last Name",
          "addressLine1": "Address Line 1",
          "addressLine2": "Address Line 2",
          "city": "City",
          "zipCode": "Zip Code",
          "button": "Write Comment"

      };
    };

    getDataForLayout4 = (): any => {
        return {
            "title":"Your comment",
            "rateTitle":"Rate",
            "descriptionPlaceholder":"Description",
            "btnSubmit": "Write Comment",
            "btnAddPhotoOrVideo":"Add Photo or Video",
            "iconsStars": [
                {
                    "isActive": true,
                    "icon": "star"
                },
                {
                    "isActive": true,
                    "icon": "star"
                },
                {
                    "isActive": true,
                    "icon": "star"
                },
                {
                    "isActive": true,
                    "icon": "star"
                },
                {
                    "isActive": false,
                    "icon": "star"
                }
            ],
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
                    .object('form/' + item.theme)
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
