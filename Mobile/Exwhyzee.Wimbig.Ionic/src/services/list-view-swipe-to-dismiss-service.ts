import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings';
import { ToastService } from './toast-service';
import { LoadingService } from './loading-service';

@Injectable()
export class ListViewSwipeToDismissService implements IService {

  constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

  getId = (): string => 'swipeToDismiss';

  getTitle = (): string => 'Swipe to dismiss';

  getAllThemes = (): Array<any> => {
    return [
      { "title": "Small item + header", "theme": "layout1" },
      { "title": "Products + CTA", "theme": "layout2" },
      { "title": "Full width image", "theme": "layout3" },
      { "title": "Large item with text", "theme": "layout4" }
    ];
  };

  getDataForTheme = (menuItem: any): Array<any> => {
    return this[
      'getDataFor' +
      menuItem.theme.charAt(0).toUpperCase() +
      menuItem.theme.slice(1)
    ]();
  };

//Small item + header data
  getDataForLayout1 = (): any => {
    return {
      "title":"HeaderTitle",
      "description":"HeaderSubtitle",
      "shortDescription":"35:72",
      "iconLike":"icon-thumb-up",
      "iconFavorite":"icon-heart",
      "iconShare":"icon-share-variant",
      "iconPlay":"icon-play-circle-outline",
      "items":[
         {
            "id":1,
            "title":"Hoochie Coochie Man",
            "description":"Muddy Waters",
            "shortDescription":"3:42",
            "image":"assets/images/avatar/0.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":2,
            "title":"Six Strings Down",
            "description":"Jimmie Vaughn",
            "shortDescription":"3:42",
            "image":"assets/images/avatar/1.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":3,
            "title":"Cherry Red Wine",
            "description":"Luther Allison",
            "shortDescription":"3:42",
            "image":"assets/images/avatar/2.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":4,
            "title":"Blue and Lonesome",
            "description":"Rolling Stones",
            "shortDescription":"3:42",
            "image":"assets/images/avatar/3.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":5,
            "title":"The Sky is Crying",
            "description":"Stevie Ray Vaughan",
            "shortDescription":"3:42",
            "image":"assets/images/avatar/4.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":6,
            "title":"Everybody Needs Somebody To Love",
            "description":"Solomon Burke",
            "shortDescription":"3:42",
            "image":"assets/images/avatar/5.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":7,
            "title":"Walking by Myself",
            "description":"Jimmy Rogers",
            "shortDescription":"3:42",
            "image":"assets/images/avatar/6.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":8,
            "title":"Sinner’s Prayer",
            "description":"Ray Charles",
            "shortDescription":"3:42",
            "image":"assets/images/avatar/7.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         }
      ]
   };
  };

//Products + CTA data
  getDataForLayout2 = (): any => {
    return {
      "items":[
         {
            "id":1,
            "title":"Black Shirt",
            "description":"Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            "image":"assets/images/avatar/17.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":2,
            "title":"Black Sweater",
            "description":"Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            "image":"assets/images/avatar/18.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":3,
            "title":"White Shirt",
            "description":"Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            "image":"assets/images/avatar/19.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":4,
            "title":"Shirt",
            "description":"Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            "image":"assets/images/avatar/20.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":5,
            "title":"T Shirt",
            "description":"Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            "image":"assets/images/avatar/21.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":6,
            "title":"White T Shirt",
            "description":"Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            "image":"assets/images/avatar/22.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":7,
            "title":"Hoodies",
            "description":"Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            "image":"assets/images/avatar/23.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":8,
            "title":"Shirt",
            "description":"Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            "image":"assets/images/avatar/17.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":9,
            "title":"Sweater",
            "description":"Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            "image":"assets/images/avatar/18.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         }
      ]
   };
  };

//Full width image data
  getDataForLayout3 = (): any => {
    return {
      "items":[
         {
            "id":1,
            "title":"Weedville",
            "description":"Northern Mariana Islands",
            "image":"assets/images/background-small/7.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":2,
            "title":"Curtice",
            "description":"Nauru",
            "image":"assets/images/background-small/9.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":3,
            "title":"Norvelt",
            "description":"Indonesia",
            "image":"assets/images/background-small/10.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":4,
            "title":"Vincent",
            "description":"Antarctica",
            "image":"assets/images/background-small/11.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":5,
            "title":"Fairacres",
            "description":"Colombia",
            "image":"assets/images/background-small/12.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":6,
            "title":"Greenwich",
            "description":"Tajikistan",
            "image":"assets/images/background-small/13.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":7,
            "title":"Ryderwood",
            "description":"Sao Tome and Principe",
            "image":"assets/images/background-small/14.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         },
         {
            "id":8,
            "title":"Lithium",
            "description":"Puerto Rico",
            "image":"assets/images/background-small/15.jpg",
            "iconDelate":"icon-delete",
            "iconUndo":"icon-undo-variant"
         }
      ]
   }
  };
// Large item with text data
  getDataForLayout4 = (): any => {
    return {
      "items":[
        {
           id: 1,
           title: '@Monument walk tour',
           description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
           iconDelate:"icon-delete",
           image: 'assets/images/avatar/3.jpg',
           iconUndo: 'icon-undo-variant'
       },
       {
           id: 2,
           title: '@River walk tour',
           description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
           iconDelate:"icon-delete",
           image: 'assets/images/avatar/4.jpg',
           iconUndo: 'icon-undo-variant'
       },
       {
           id: 3,
           title: '@City walk tour',
           description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
           iconDelate:"icon-delete",
           image: 'assets/images/avatar/5.jpg',
           iconUndo: 'icon-undo-variant'
       },
       {
           id: 4,
           title: '@Park walk tour',
           description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
           iconDelate:"icon-delete",
           image: 'assets/images/avatar/6.jpg',
           iconUndo: 'icon-undo-variant'
       },
       {
           id: 5,
           title: '@Vilage walk tour',
           description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
           iconDelate:"icon-delete",
           image: 'assets/images/avatar/7.jpg',
           iconUndo: 'icon-undo-variant'
       },
       {
           id: 6,
           title: '@Lake walk tour',
           description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
           iconDelate:"icon-delete",
           image: 'assets/images/avatar/8.jpg',
           iconUndo: 'icon-undo-variant'
       },
       {
           id: 7,
           title: '@Castle walk tour',
           description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
           iconDelate:"icon-delete",
           image: 'assets/images/avatar/9.jpg',
           iconUndo: 'icon-undo-variant'
       },
       {
           id: 8,
           title: '@Beach walk tour',
           description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
           iconDelate:"icon-delete",
           image: 'assets/images/avatar/10.jpg',
           iconUndo: 'icon-undo-variant'
       }
      ]
   }
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
      data: {},
      theme: item.theme,
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
          .object('listView/swipeToDismiss/' + item.theme)
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
