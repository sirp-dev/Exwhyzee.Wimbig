import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings';
import { ToastService } from './toast-service';
import { LoadingService } from './loading-service';

@Injectable()
export class ListViewGoogleCardsService implements IService {

  constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

  getId = (): string => 'googleCards';

  getTitle = (): string => 'Google Cards';

  getAllThemes = (): Array<any> => {
    return [
      { "title": "Styled cards", "theme": "layout1" },
      { "title": "Styled cards 2", "theme": "layout2" },
      { "title": "Full image cards", "theme": "layout3" },
      { "title": "Post card", "theme": "layout4" }
    ];
  };

//Styled cards data
  getDataForLayout1 = (): any => {
    return {
      "title":"PlaylistName",
      "description":"Author:Username",
      "duration":"35:72",
      "items":[
         {
            "id":1,
            "title":"Guerrero Woodard",
            "image":"assets/images/avatar-small/0.jpg",
            "description":"Birth year: 1984",
            "shortDescription":"Country: Germany",
            "longDescription":"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant"
         },
         {
            "id":2,
            "title":"Fitzgerald Stanton",
            "image":"assets/images/avatar-small/1.jpg",
            "description":"Birth year: 1970",
            "shortDescription":"Country: Belgium",
            "longDescription":"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant"
         },
         {
            "id":3,
            "title":"Jessica Miles",
            "image":"assets/images/avatar-small/2.jpg",
            "description":"Birth year: 1982",
            "shortDescription":"Country: Netherlands Antilles",
            "longDescription":"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant"
         },
         {
            "id":4,
            "title":"Duran Clayton",
            "image":"assets/images/avatar-small/3.jpg",
            "description":"Birth year: 1986",
            "shortDescription":"Country: Russian Federation",
            "longDescription":"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant"
         },
         {
            "id":5,
            "title":"Julia Petersen",
            "image":"assets/images/avatar-small/4.jpg",
            "description":"Birth year: 1984",
            "shortDescription":"Country: Czech Republic",
            "longDescription":"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant"
         },
         {
            "id":6,
            "title":"Natasha Gamble",
            "image":"assets/images/avatar-small/5.jpg",
            "description":"Birth year: 1981",
            "shortDescription":"Country: United Kingdom",
            "longDescription":"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant"
         },
         {
            "id":7,
            "title":"Parsons Mcfadden",
            "image":"assets/images/avatar-small/6.jpg",
            "description":"Birth year: 1985",
            "shortDescription":"Country: Ireland",
            "longDescription":"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant"
         },
         {
            "id":8,
            "title":"Compton Dejesus",
            "image":"assets/images/avatar-small/7.jpg",
            "description":"Birth year: 1987",
            "shortDescription":"Country: Swaziland",
            "longDescription":"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do",
            "iconLike":"icon-thumb-up",
            "iconFavorite":"icon-heart",
            "iconShare":"icon-share-variant"
         }
      ]
   };
  };

  //Styled cards 2 data
  getDataForLayout2 = (): any => {
    return {
      "items":[
         {
            "id":1,
            "title":"Fortuitu ad aeroportus",
            "titleHeader":"Simul quanta praecinctionis",
            "description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
            "image":"assets/images/background/1.jpg",
            "button":"EXPLORE",
            "shareButton":"SHARE"
         },
         {
            "id":2,
            "title":"Hoc est exortum",
            "titleHeader":"Pedestres sub imprudentia contentum",
            "description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
            "image":"assets/images/background/2.jpg",
            "button":"EXPLORE",
            "shareButton":"SHARE"
         },
         {
            "id":3,
            "title":"Communications moderatoris",
            "titleHeader":"Technica et Internet habeat facultatem",
            "description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
            "image":"assets/images/background/5.jpg",
            "button":"EXPLORE",
            "shareButton":"SHARE"
         },
         {
            "id":4,
            "title":"Tabulas scripto munus agere providere",
            "titleHeader":"Ut adeptus est atrium",
            "description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
            "image":"assets/images/background/3.jpg",
            "button":"EXPLORE",
            "shareButton":"SHARE"
         },
         {
            "id":5,
            "title":"In outpatient nuntiatum ministerium",
            "titleHeader":"Testis unus",
            "description":"Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
            "image":"assets/images/background/1.jpg",
            "button":"EXPLORE",
            "shareButton":"SHARE"
         }
      ]
   };
  };

//Full image cards data
  getDataForLayout3 = (): any => {
    return {
      "refreshMessage":"Pull to refresh...",
      "items":[
         {
            "id":1,
            "image":"assets/images/background/0.jpg",
            "title":"Denique sexta",
            "subtitle":"Pilae per"
         },
         {
            "id":2,
            "image":"assets/images/background/9.jpg",
            "title":"Iura sem",
            "subtitle":"Incredibilem pecuniae"
         },
         {
            "id":3,
            "image":"assets/images/background/8.jpg",
            "title":"Partim players",
            "subtitle":"Minimum pretium"
         },
         {
            "id":4,
            "image":"assets/images/background/10.jpg",
            "title":"Prope unstoppable",
            "subtitle":"Bonum defensus"
         },
         {
            "id":5,
            "image":"assets/images/background/13.jpg",
            "title":"Primum par ludere",
            "subtitle":"Et stadium contendas in Humsko"
         },
         {
            "id":6,
            "image":"assets/images/background/11.jpg",
            "title":"Vestibulum non eleison",
            "subtitle":"Notissima"
         },
         {
            "id":7,
            "image":"assets/images/background/12.jpg",
            "title":"Optimum natus",
            "subtitle":"Non saltem racemum reliquissent"
         },
         {
            "id":8,
            "image":"assets/images/background/0.jpg",
            "title":"Quod magnum firmamentum",
            "subtitle":"Ingens tailwind"
         }
      ]
   };
  };

// Post card data
  getDataForLayout4 = (): any => {
    return [
          {
              id: 1,
              title: 'Jessica Miles',
              avatar: 'assets/images/avatar/2.jpg',
              image: 'assets/images/background/1.jpg',
              description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry',
              shortDescription: 'November 05, 1955',
              firstButton: 'LIKE',
              secondButton: 'SHARE'
          },
          {
              id: 2,
              title: 'Holman Valencia',
              avatar: 'assets/images/avatar/0.jpg',
              image: 'assets/images/background/2.jpg',
              description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry',
              shortDescription: 'November 05, 1955',
              firstButton: 'LIKE',
              secondButton: 'SHARE'
          },
          {
              id: 3,
              title: 'Gayle Gaines',
              avatar: 'assets/images/avatar/1.jpg',
              image: 'assets/images/background/3.jpg',
              description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry',
              shortDescription: 'November 05, 1955',
              firstButton: 'LIKE',
              secondButton: 'SHARE'
          },
          {
              id: 4,
              title: 'Josefa Gardner',
              avatar: 'assets/images/avatar/4.jpg',
              image: 'assets/images/background/4.jpg',
              description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry',
              shortDescription: 'November 05, 1955',
              firstButton: 'LIKE',
              secondButton: 'SHARE'
          },
          {
              id: 5,
              title: 'Barbara Bernard',
              avatar: 'assets/images/avatar/5.jpg',
              image: 'assets/images/background/5.jpg',
              description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry',
              shortDescription: 'November 05, 1955',
              firstButton: 'LIKE',
              secondButton: 'SHARE'
          },
          {
              id: 6,
              title: 'Valdez Bruce',
              avatar: 'assets/images/avatar/6.jpg',
              image: 'assets/images/background/6.jpg',
              description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry',
              shortDescription: 'November 05, 1955',
              firstButton: 'LIKE',
              secondButton: 'SHARE'
          },
          {
              id: 7,
              title: 'Wilkerson Hardin',
              avatar: 'assets/images/avatar/7.jpg',
              image: 'assets/images/background/7.jpg',
              description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry',
              shortDescription: 'November 05, 1955',
              firstButton: 'LIKE',
              secondButton: 'SHARE'
          }
      ];
  };



  getDataForTheme = (menuItem: any): Array<any> => {
    return this[
      'getDataFor' +
      menuItem.theme.charAt(0).toUpperCase() +
      menuItem.theme.slice(1)
    ]();
  };

  getEventsForTheme = (menuItem: any): any => {
    var that = this;
    return {
      'onItemClick': function (item: any) {
          that.toastCtrl.presentToast(item);
      },
      'onExplore': function (item: any) {
          that.toastCtrl.presentToast("Explore");
      },
      'onShare': function (item: any) {
          that.toastCtrl.presentToast("Share");
      },
      'onLike': function (item: any) {
          that.toastCtrl.presentToast("Like");
      },
      'onFavorite': function (item: any) {
          that.toastCtrl.presentToast("Favorite");
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
        debugger
        this.af
          .object('listView/googleCards/' + item.theme)
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
