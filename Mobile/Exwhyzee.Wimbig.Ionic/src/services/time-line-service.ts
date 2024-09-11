import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings'
import { ToastService } from './toast-service'
import { LoadingService } from './loading-service'

@Injectable()
export class TimeLineService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'timeline';

    getTitle = (): string => 'Time Line';

    getAllThemes = (): Array<any> => {
        return [
          {"title" : "Timeline With Cards", "theme"  : "layout1"},
          {"title" : "Timeline With Avatar", "theme"  : "layout2"},
          {"title" : "Timeline With Comments", "theme"  : "layout3"},
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
            "onButton": function(item: any) {
                that.toastCtrl.presentToast(item.title);
            },
            "onItemClick": function(item: any) {
                that.toastCtrl.presentToast(item.title);
            }
        };
    };

    getDataForLayout1 = (): any => {
        return {
          "items": [
              {
                  "id": 1,
                  "title": "Sydney, Australia",
                  "time": "TODAY AT 2:20PM",
                  "image": "assets/images/background/2.jpg"
              },
              {
                  "id": 2,
                  "title": "Basel, Switzerland ",
                  "time": "TODAY AT 1:30PM",
                  "image": "assets/images/background/3.jpg"
              },
              {
                  "id": 3,
                  "title": "Copenhagen, Denmark",
                  "time": "TODAY AT 14:20PM",
                  "image": "assets/images/background/9.jpg"
              },
              {
                  "id": 4,
                  "title": "Geneva, Switzerland",
                  "time": "TODAY AT 15:15PM",
                  "image": "assets/images/background/30.jpg"
              }
          ]
      };
    };

    getDataForLayout2 = (): any => {
        return {
          "items": [
              {
                  "id": 1,
                  "title": "Black Shirt",
                  "time": "TODAY AT 2:20PM",
                  "avatar": "assets/images/avatar/17.jpg",
                  "price": "$ 3.23"
              },
              {
                  "id": 2,
                  "title": "Black Sweater",
                  "time": "TODAY AT 1:30PM",
                  "avatar": "assets/images/avatar/18.jpg",
                  "price": "$ 3.23"
              },
              {
                  "id": 3,
                  "title": "White Shirt",
                  "time": "TODAY AT 14:20PM",
                  "avatar": "assets/images/avatar/19.jpg",
                  "price": "$ 3.23"
              },
              {
                  "id": 4,
                  "title": "Shirt",
                  "time": "TODAY AT 15:15PM",
                  "avatar": "assets/images/avatar/20.jpg",
                  "price": "$ 3.23"
              },
              {
                  "id": 5,
                  "title": "T Shirt",
                  "time": "TODAY AT 15:15PM",
                  "avatar": "assets/images/avatar/21.jpg",
                  "price": "$ 3.23"
              },
              {
                  "id": 6,
                  "title": "White T Shirt",
                  "time": "TODAY AT 18:15PM",
                  "avatar": "assets/images/avatar/22.jpg",
                  "price": "$ 3.23"
              },
              {
                  "id": 7,
                  "title": "Hoodies",
                  "time": "TODAY AT 19:55PM",
                  "avatar": "assets/images/avatar/23.jpg",
                  "price": "$ 3.23"
              }
          ]
      };
    };

    getDataForLayout3 = (): any => {
        return {
          "items": [
              {
                  "id": 1,
                  "time": "TODAY AT 2:20PM",
                  "avatar": "assets/images/avatar/10.jpg",
                  "title": "Alice Ellis",
                  "subtitle": "@alice",
                  "description": "All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks."
              },
              {
                  "id": 2,
                  "time": "TODAY AT 1:30PM",
                  "avatar": "assets/images/avatar/11.jpg",
                  "title": "Natasha Cox",
                  "subtitle": "@natasha",
                  "description": "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia."
              },
              {
                  "id": 3,
                  "time": "TODAY AT 14:20PM",
                  "avatar": "assets/images/avatar/12.jpg",
                  "title": "Caroline Wright",
                  "subtitle": "@caroline",
                  "description": "It is a long established fact that a reader will be distracted by the readable."
              },
              {
                  "id": 4,
                  "time": "TODAY AT 14:20PM",
                  "avatar": "assets/images/avatar/13.jpg",
                  "title": "Cameron Rogers",
                  "subtitle": "@cameron",
                  "description": "Lorem Ipsum is simply dummy text of the printing and typesetting industry."
              }
          ]
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
                    .object('timeline/' + item.theme)
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
