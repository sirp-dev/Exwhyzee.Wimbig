import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings';
import { ToastService } from './toast-service';
import { LoadingService } from './loading-service';

@Injectable()
export class ListViewExpandableService implements IService {

  constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

  getId = (): string => 'expandable';

  getTitle = (): string => 'Expandable';

  getAllThemes = (): Array<any> => {
    return [
      { "title": "List big image", "theme": "layout1" },
      { "title": "Full image with CTA", "theme": "layout2" },
      { "title": "Centered with header", "theme": "layout3" }
    ];
  };

  getDataForTheme = (menuItem: any): Array<any> => {
    return this[
      'getDataFor' +
      menuItem.theme.charAt(0).toUpperCase() +
      menuItem.theme.slice(1)
    ]();
  };

 // List big image data
  getDataForLayout1 = (): any => {
    return {
      "items":[
         {
            "id":1,
            "title":"Benton Willis",
            "description":"SINGER",
            "image":"assets/images/avatar/15.jpg",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant",
            "items":[
               {
                  "id":1,
                  "title":"Smokestack Lightning",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/10.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":2,
                  "title":"Boogie Chillen’",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/11.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":3,
                  "title":"Call It Stormy Mondaye",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/12.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":4,
                  "title":"I’m Tore Down",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/13.jpg",
                  "iconPlay":"icon-play-circle-outline"
               }
            ]
         },
         {
            "id":2,
            "title":"Jessica Miles",
            "description":"BASSO",
            "image":"assets/images/avatar/2.jpg",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant",
            "items":[
               {
                  "id":1,
                  "title":"Bell Bottom Blues",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/14.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":2,
                  "title":"Still Got The Blues",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/15.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":3,
                  "title":"Mustang Sally",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/14.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":4,
                  "title":"Ball N’ Chain",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/13.jpg",
                  "iconPlay":"icon-play-circle-outline"
               }
            ]
         },
         {
            "id":3,
            "title":"Holman Valencia",
            "description":"GUITARIST",
            "image":"assets/images/avatar/3.jpg",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant",
            "items":[
               {
                  "id":1,
                  "title":"Dust My Broom",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/11.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":2,
                  "title":"Hold On, I’m Coming",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/12.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":3,
                  "title":"The Little Red Rooster",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/13.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":4,
                  "title":"Bright Lights",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/14.jpg",
                  "iconPlay":"icon-play-circle-outline"
               }
            ]
         },
         {
            "id":4,
            "title":"Natasha Gambl",
            "description":"SINGER",
            "image":"assets/images/avatar/4.jpg",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant",
            "items":[
               {
                  "id":1,
                  "title":"Got My Mojo Working",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/0.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":2,
                  "title":"A Little Less Conversation",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/1.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":3,
                  "title":"Life By The Drop",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/2.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":4,
                  "title":"Boom Boom",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/3.jpg",
                  "iconPlay":"icon-play-circle-outline"
               }
            ]
         },
         {
            "id":5,
            "title":"Carol Kelly",
            "description":"DRUMMER",
            "image":"assets/images/avatar/5.jpg",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant",
            "items":[
               {
                  "id":1,
                  "title":"Thing Called Love",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/14.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":2,
                  "title":"Green Onions",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/15.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":3,
                  "title":"The Midnight Special",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/6.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":4,
                  "title":"Mess Around",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/7.jpg",
                  "iconPlay":"icon-play-circle-outline"
               }
            ]
         },
         {
            "id":6,
            "title":"Mildred Clark",
            "description":"DRUMMER",
            "image":"assets/images/avatar/3.jpg",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant",
            "items":[
               {
                  "id":1,
                  "title":"Little Wing",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/14.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":2,
                  "title":"Bad Penny",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/15.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":3,
                  "title":"Farther on Up the Road",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/6.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":4,
                  "title":"Mannish Boy",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/7.jpg",
                  "iconPlay":"icon-play-circle-outline"
               }
            ]
         },
         {
            "id":7,
            "title":"Megan Singleton",
            "description":"DRUMMER",
            "image":"assets/images/avatar/4.jpg",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant",
            "items":[
               {
                  "id":1,
                  "title":"Trouble No More",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/14.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":2,
                  "title":"Hellhound On My Trail",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/15.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":3,
                  "title":"Help Me",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/6.jpg",
                  "iconPlay":"icon-play-circle-outline"
               },
               {
                  "id":4,
                  "title":"A Man Of Many Words",
                  "description":"Universal, 2016",
                  "image":"assets/images/avatar/7.jpg",
                  "iconPlay":"icon-play-circle-outline"
               }
            ]
         }
      ]
   };
  };

 // Full image with CTA
  getDataForLayout2 = (): any => {
    return {
      "items":[
         {
            "id":1,
            "title":"Rubus idaeus Pi",
            "backgroundImage":"assets/images/background/22.jpg",
            "button":"BUY",
            "items":[
               "PAY WITH PAYPAL",
               "PAY WITH VISA CARD",
               "PAY WITH MAESTRO CARD"
            ]
         },
         {
            "id":2,
            "title":"Nidum Thermostat",
            "backgroundImage":"assets/images/background/23.jpg",
            "button":"BUY",
            "items":[
               "PAY WITH PAYPAL",
               "PAY WITH VISA CARD",
               "PAY WITH MAESTRO CARD"
            ]
         },
         {
            "id":3,
            "title":"Baculum Magicum",
            "backgroundImage":"assets/images/background/24.jpg",
            "button":"BUY",
            "items":[
               "PAY WITH PAYPAL",
               "PAY WITH VISA CARD",
               "PAY WITH MAESTRO CARD"
            ]
         },
         {
            "id":4,
            "title":"Commodore LXIV",
            "backgroundImage":"assets/images/background/25.jpg",
            "button":"BUY",
            "items":[
               "PAY WITH PAYPAL",
               "PAY WITH VISA CARD",
               "PAY WITH MAESTRO CARD"
            ]
         },
         {
            "id":5,
            "title":"Palm Nauclerus",
            "backgroundImage":"assets/images/background/26.jpg",
            "button":"BUY",
            "items":[
               "PAY WITH PAYPAL",
               "PAY WITH VISA CARD",
               "PAY WITH MAESTRO CARD"
            ]
         },
         {
            "id":6,
            "title":"Optio Fridericus Hultsch Box",
            "backgroundImage":"assets/images/background/27.jpg",
            "button":"BUY",
            "items":[
               "PAY WITH PAYPAL",
               "PAY WITH VISA CARD",
               "PAY WITH MAESTRO CARD"
            ]
         },
         {
            "id":7,
            "title":"Oculi Odium",
            "backgroundImage":"assets/images/background/28.jpg",
            "button":"BUY",
            "items":[
               "PAY WITH PAYPAL",
               "PAY WITH VISA CARD",
               "PAY WITH MAESTRO CARD"
            ]
         }
      ]
   }
  };

   // Centered with header
  getDataForLayout3 = (): any => {
    return {
      "title":"New York",
      "headerImage":"assets/images/background-small/7.jpg",
      "items":[
         {
            "title":"Where to go",
            "icon":"icon-map-marker-radius",
            "items":[
               "Monuments",
               "Sightseeing",
               "Historical",
               "Sport"
            ]
         },
         {
            "title":"Where to sleep",
            "icon":"icon-hotel",
            "items":[
               "Hotels",
               "Hostels",
               "Motels",
               "Rooms"
            ]
         },
         {
            "title":"Where to eat",
            "icon":"icon-silverware-variant",
            "items":[
               "Fast Food",
               "Restorants",
               "Pubs",
               "Hotels"
            ]
         },
         {
            "title":"Where to drink",
            "icon":"icon-martini",
            "items":[
               "Caffes",
               "Bars",
               "Pubs",
               "Clubs"
            ]
         },
         {
            "title":"Where to go",
            "icon":"icon-map-marker-radius",
            "items":[
               "Monuments",
               "Sightseeing",
               "Historical",
               "Sport"
            ]
         }
      ]
   };
  };

  getEventsForTheme = (menuItem: any): any => {
    var that = this;
    return {
      'onItemClick': function (item: any) {
          that.toastCtrl.presentToast(item);
      },
      'onLike': function (item: any) {
          that.toastCtrl.presentToast("Like");
      },
      'onFavorite': function (item: any) {
          that.toastCtrl.presentToast("Favorite");
      },
      'onShare': function (item: any) {
          that.toastCtrl.presentToast("Share");
      },
      'onFab': function (item: any) {
          that.toastCtrl.presentToast("Fab");
      },
    };
  };

  prepareParams = (item: any) => {
    let result = {
      title: item.title,
      theme: item.theme,
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
        that.toastCtrl.presentToast(item.theme)
        this.af
          .object('listView/expandable/' + item.theme)
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
