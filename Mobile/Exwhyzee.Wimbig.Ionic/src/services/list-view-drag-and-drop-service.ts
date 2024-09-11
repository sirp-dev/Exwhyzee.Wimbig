import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings';
import { ToastService } from './toast-service';
import { LoadingService } from './loading-service';

@Injectable()
export class ListViewDragAndDropService implements IService {

  constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) {
  }

  getId = (): string => 'dragAndDrop';

  getTitle = (): string => 'Drag and Drop';

  getAllThemes = (): Array<any> => {
    return [
      { "title": "Small item + header", "theme": "layout1" },
      { "title": "Products + CTA header", "theme": "layout2" },
      { "title": "Medium item with avatar", "theme": "layout3" },
      { "title": "Medium item with image", "theme": "layout4" }
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
      "title":"Playlist Name",
      "description":"Author: Username",
      "duration":"35:72",
      "icon":"icon-check",
      "items":[
         {
            "id":1,
            "title":"Hoochie Coochie Man",
            "author":"Author: Muddy Waters",
            "image":"assets/images/avatar/0.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":2,
            "title":"The Thrill is Gone",
            "author":"Author: B.B. King",
            "image":"assets/images/avatar/1.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":3,
            "title":"Me And The Devil Blues",
            "author":"Author: Robert Johnson",
            "image":"assets/images/avatar/2.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":4,
            "title":"Stone Crazy",
            "author":"Author: Buddy Guy",
            "image":"assets/images/avatar/3.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":5,
            "title":"I’d Rather Go Blind",
            "author":"Author: Etta James",
            "image":"assets/images/avatar/4.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":6,
            "title":"I’m Tore Down",
            "author":"Author: Freddie King",
            "image":"assets/images/avatar/5.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":7,
            "title":"Call It Stormy Monday",
            "author":"Author: T-Bone Walker",
            "image":"assets/images/avatar/6.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":8,
            "title":"Smokestack Lightning",
            "author":"Author: Howlin’ Wolf",
            "image":"assets/images/avatar/0.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":9,
            "title":"Bell Bottom Blues",
            "author":"Author: Derek and the Dominoes",
            "image":"assets/images/avatar/1.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":10,
            "title":"Still Got The Blues",
            "author":"Author: Gary Moore",
            "image":"assets/images/avatar/2.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":11,
            "title":"Mustang Sally",
            "author":"Author: Wilson Pickett",
            "image":"assets/images/avatar/3.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":12,
            "title":"Ball N’ Chain",
            "author":"Author: Big Mama Thornton",
            "image":"assets/images/avatar/4.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":13,
            "title":"Dust My Broom",
            "author":"Author: Elmore James",
            "image":"assets/images/avatar/5.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":14,
            "title":"Hold On, I’m Coming",
            "author":"Author: Eric Clapton",
            "image":"assets/images/avatar/6.jpg",
            "leftIcon":"icon-play-circle",
            "rightIcon":"icon-unfold-more"
         }
      ]
   };
  };

//Products + CTA header data
  getDataForLayout2 = (): any => {
    return {
      "title":"Order No. 1",
      "description":"Will be shipped: 15.5.2016.",
      "buttonText":"PROCEED",
      "headerImage":"assets/images/background/22.jpg",
      "price":"$42.99",
      "items":[
         {
            "id":1,
            "title":"Black Shirt",
            "seller":"Seller Name",
            "image":"assets/images/avatar/17.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":2,
            "title":"Black Sweater",
            "seller":"Seller Name",
            "image":"assets/images/avatar/18.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":3,
            "title":"Shirt",
            "seller":"Seller Name",
            "image":"assets/images/avatar/19.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":4,
            "title":"White Shirt",
            "seller":"Seller Name",
            "image":"assets/images/avatar/20.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":5,
            "title":"White T shirt",
            "seller":"Seller Name",
            "image":"assets/images/avatar/21.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":6,
            "title":"T shirt",
            "seller":"Seller Name",
            "image":"assets/images/avatar/22.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "rightIcon":"icon-unfold-more"
         },
         {
            "id":7,
            "title":"Hoodies",
            "seller":"Seller Name",
            "image":"assets/images/avatar/23.jpg",
            "oldPrice":"$42.99",
            "newPrice":"$35.99",
            "rightIcon":"icon-unfold-more"
         }
      ]
   };
  };

//Medium item with avatar data
  getDataForLayout3 = (): any => {
    return {
      "items":[
         {
            "id":1,
            "title":"Isaac Reid",
            "description":"from Las Vegas",
            "image":"assets/images/avatar/0.jpg"
         },
         {
            "id":2,
            "title":"Jason Graham",
            "description":"from Brogan",
            "image":"assets/images/avatar/1.jpg"
         },
         {
            "id":3,
            "title":"Abigail Ross",
            "description":"from Bannock",
            "image":"assets/images/avatar/2.jpg"
         },
         {
            "id":4,
            "title":"Justin Rutherford",
            "description":"from Madrid",
            "image":"assets/images/avatar/3.jpg"
         },
         {
            "id":5,
            "title":"Nicholas Henderson",
            "description":"from Bridgetown",
            "image":"assets/images/avatar/4.jpg"
         },
         {
            "id":6,
            "title":"Elizabeth Mackenzie",
            "description":"from Nipinnawasee",
            "image":"assets/images/avatar/5.jpg"
         },
         {
            "id":7,
            "title":"Melanie Ferguson",
            "description":"from Dixonville",
            "image":"assets/images/avatar/6.jpg"
         },
         {
            "id":8,
            "title":"Fiona Kelly",
            "description":"from Orovada",
            "image":"assets/images/avatar/7.jpg"
         },
         {
            "id":9,
            "title":"Nicholas King",
            "description":"from Interlochen",
            "image":"assets/images/avatar/8.jpg"
         },
         {
            "id":10,
            "title":"Victoria Mitchell",
            "description":"from Sanders",
            "image":"assets/images/avatar/9.jpg"
         },
         {
            "id":11,
            "title":"Sophie Lyman",
            "description":"from Boonville",
            "image":"assets/images/avatar/10.jpg"
         },
         {
            "id":12,
            "title":"Carl Ince",
            "description":"from Wattsville",
            "image":"assets/images/avatar/11.jpg"
         },
         {
            "id":13,
            "title":"Michelle Slater",
            "description":"from Harrison",
            "image":"assets/images/avatar/12.jpg"
         },
         {
            "id":14,
            "title":"Ryan Mathis",
            "description":"from Montura",
            "image":"assets/images/avatar/13.jpg"
         },
         {
            "id":15,
            "title":"Julia Grant",
            "description":"from Onton",
            "image":"assets/images/avatar/14.jpg"
         },
         {
            "id":16,
            "title":"Hannah Martin",
            "description":"from Emison",
            "image":"assets/images/avatar/15.jpg"
         }
      ]
   };
  };
// Medium item with image data
  getDataForLayout4 = (): any => {
    return {
      "items":[
            {
                id: 1,
                title: 'Monument walk tour',
                description: '23min walk from center',
                icon: 'star',
                mark: '4.1',
                image: 'assets/images/background-small/16.jpg'
            },
            {
                id: 2,
                title: 'Park walk tour',
                description: '23min walk from center',
                icon: 'star',
                mark: '4.4',
                image: 'assets/images/background-small/17.jpg'
            },
            {
                id: 3,
                title: 'River walk tour',
                description: '23min walk from center',
                icon: 'star',
                mark: '3.6',
                image: 'assets/images/background-small/18.jpg'
            },
            {
                id: 4,
                title: 'City walk tour',
                description: '23min walk from center',
                icon: 'star',
                mark: '4.2',
                image: 'assets/images/background-small/19.jpg'
            },
            {
                id: 5,
                title: 'Lake walk tour',
                description: '23min walk from center',
                icon: 'star',
                mark: '3.5',
                image: 'assets/images/background-small/20.jpg'
            },
            {
                id: 6,
                title: 'Vilage walk tour',
                description: '23min walk from center',
                icon: 'star',
                mark: '4.5',
                image: 'assets/images/background-small/21.jpg'
            },
            {
                id: 7,
                title: 'Castle walk tour',
                description: '23min walk from center',
                icon: 'star',
                mark: '4.7',
                image: 'assets/images/background-small/22.jpg'
            },
            {
                id: 8,
                title: 'Beach walk tour',
                description: '23min walk from center',
                icon: 'star',
                mark: '3.1',
                image: 'assets/images/background-small/23.jpg'
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
      'onProceed': function (item: any) {
          that.toastCtrl.presentToast("Proceed");
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
        this.af
          .object('listView/dragAndDrop/' + item.theme)
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
  };
}
