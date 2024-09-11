 import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings'
import { ToastService } from './toast-service'
import { LoadingService } from './loading-service'

@Injectable()
export class AlertService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'alert';

    getTitle = (): string => 'Alert';

    getAllThemes = (): Array<any> => {
        return [
          {"title" : "Alert Info", "theme"  : "layout1"},
          {"title" : "Alert Warning", "theme"  : "layout2"},
          {"title" : "Alert Subscribe", "theme"  : "layout3"}
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
            }
        };
    };

    getDataForLayout1 = (): any => {
        return {
          "items": [
              {
                  "id": 1,
                  "image": "assets/images/background/18.jpg",
                  "title": "Roasted Vegetables",
                  "subtitle": "Category: Food",
              },
              {
                "id": 2,
                "image": "assets/images/background/19.jpg",
                "title": "Simple Turkey Chili",
                "subtitle": "Category: Food",
              },
              {
                "id": 3,
                "image": "assets/images/background/20.jpg",
                "title": "Playgroup Granola Bars",
                "subtitle": "Category: Food",
              },
              {
                "id": 4,
                "image": "assets/images/background/21.jpg",
                "title": "Greek Chicken Pasta",
                "subtitle": "Category: Food",
              }
          ]
      };
    };

    getDataForLayout2 = (): any => {
        return {
          "items": [
              {
                  "id": 1,
                  "category": "best offer",
                  "image": "assets/images/background/22.jpg",
                  "title": "New Floral Printing Slim Turndown Neck ",
                  "iconLike": "thumbs-up",
                  "iconComment": "ios-chatbubbles",
                  "numberLike": "12",
                  "numberCommnet": "4",
              },
              {
                  "id": 2,
                  "category": "best offer",
                  "image": "assets/images/background/23.jpg",
                  "title": "Lapel Patchwork Long Sleeve Shirt",
                  "iconLike": "thumbs-up",
                  "iconComment": "ios-chatbubbles",
                  "numberLike": "12",
                  "numberCommnet": "4",
              },
              {
                  "id": 3,
                  "category": "best offer",
                  "image": "assets/images/background/24.jpg",
                  "title": "Tidebuy Unique Floral Mens Casual Shirt",
                  "iconLike": "thumbs-up",
                  "iconComment": "ios-chatbubbles",
                  "numberLike": "12",
                  "numberCommnet": "4",
              },
              {
                  "id": 4,
                  "category": "best offer",
                  "image": "assets/images/background/25.jpg",
                  "title": "Plain Lapel Single Breasted Mens Shirt",
                  "iconLike": "thumbs-up",
                  "iconComment": "ios-chatbubbles",
                  "numberLike": "12",
                  "numberCommnet": "4",
              },
              {
                  "id": 5,
                  "category": "best offer",
                  "image": "assets/images/background/26.jpg",
                  "title": "Oblique Single Breasted Mens Slim Shirt",
                  "iconLike": "thumbs-up",
                  "iconComment": "ios-chatbubbles",
                  "numberLike": "12",
                  "numberCommnet": "4",
              },
              {
                  "id": 6,
                  "category": "best offe",
                  "image": "assets/images/background/27.jpg",
                  "title": "Embroidery Lapel Straight Mens Shirt",
                  "iconLike": "thumbs-up",
                  "iconComment": "ios-chatbubbles",
                  "numberLike": "12",
                  "numberCommnet": "4",
              }
          ]
      };
    };

    getDataForLayout3 = (): any => {
      return {
        "items": [
            {
                "id": 1,
                "image": "assets/images/background/18.jpg",
                "time": "MARCH 14, 2017",
                "title": "Canadian Caesar Salad",
                "description": "There's a little added heat in this Canadian twist on traditional Caesar salad…",
                "iconLike": "thumbs-up",
                "iconComment": "ios-chatbubbles",
                "numberLike": "12",
                "numberCommnet": "4",
            },
            {
                "id": 2,
                "image": "assets/images/background/19.jpg",
                "time": "MARCH 14, 2017",
                "title": "Carrot Pepperoni Caesar Salad",
                "description": "Tasty salad with a nice kick. It's juicy, crunchy, and good!!!!!",
                "iconLike": "thumbs-up",
                "iconComment": "ios-chatbubbles",
                "numberLike": "12",
                "numberCommnet": "4",
            },
            {
                "id": 3,
                "image": "assets/images/background/20.jpg",
                "time": "MARCH 14, 2017",
                "title": "Southwestern Caesar Salad",
                "description": "A delicious Southwestern twist on a grilled chicken Caesar, served with a spicy chipotle…",
                "iconLike": "thumbs-up",
                "iconComment": "ios-chatbubbles",
                "numberLike": "12",
                "numberCommnet": "4",
            },
            {
                "id": 4,
                "image": "assets/images/background/21.jpg",
                "time": "MARCH 14, 2017",
                "title": "Classic Restaurant Caesar Salad",
                "description": "The Caesar salad you love so much at fancy restaurants is surprisingly easy and quick to…",
                "iconLike": "thumbs-up",
                "iconComment": "ios-chatbubbles",
                "numberLike": "12",
                "numberCommnet": "4",
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
                    .object('alert/' + item.theme)
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
