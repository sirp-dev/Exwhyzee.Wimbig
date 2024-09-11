import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings'
import { ToastService } from './toast-service'
import { LoadingService } from './loading-service'

@Injectable()
export class CommentService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'comment';

    getTitle = (): string => 'Comment';

    getAllThemes = (): Array<any> => {
        return [
          {"title" : "Comments Basic", "theme"  : "layout1"},
          {"title" : "Comments With Timeline", "theme"  : "layout2"},
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
            'onItemClick': function(item: any) {
                that.toastCtrl.presentToast(item.title);
            },
        };
    };

    getDataForLayout1 = (): any => {
        return {
          "headerTitle": "Commnets Basic",
          "allComments": "2121 Comments",
          "items": [
              {
                  "id": 1,
                  "image": "assets/images/avatar/16.jpg",
                  "title": "Erica Romaguera",
                  "time": "18 August 2018 at 12:20pm",
                  "description": "If you could have any kind of pet, what would you choose?"
              },
              {
                  "id": 2,
                  "image": "assets/images/avatar/15.jpg",
                  "title": "Caleigh Jerde",
                  "time": "18 August 2018 at 8:13pm",
                  "description": "If you could learn any language, what would you choose?"
              },
              {
                  "id": 3,
                  "image": "assets/images/avatar/14.jpg",
                  "title": "Lucas Schultz",
                  "time": "18 August 2018 at 5:22pm",
                  "description": "Life is about making an impact, not making an income."
              },
              {
                  "id": 4,
                  "image": "assets/images/avatar/13.jpg",
                  "title": "Carole Marvin",
                  "time": "18 August 2018 at 7:36am",
                  "description": "I’ve learned that people will forget what you said, people will forget what you did, but people will never forget how you made them feel"
              },
              {
                  "id": 5,
                  "image": "assets/images/avatar/12.jpg",
                  "title": "Doriana Feeney",
                  "time": "18 August 2018 at 5:28am",
                  "description": "Definiteness of purpose is the starting point of all achievement."
              },
              {
                  "id": 6,
                  "image": "assets/images/avatar/11.jpg",
                  "title": "Nia Gutkowski",
                  "time": "18 August 2018 at 11:27pm",
                  "description": "Life is what happens to you while you’re busy making other plans"
              }
          ]
      };
    };

    getDataForLayout2 = (): any => {
        return {
          "headerTitle": "Comment With Timeline",
          "allComments": "2121 Comments",
          "items": [
              {
                  "id": 1,
                  "image": "assets/images/avatar/16.jpg",
                  "title": "Erica Romaguera",
                  "time": "18 August 2018 at 12:20pm",
                  "description": "If you could have any kind of pet, what would you choose?"
              },
              {
                  "id": 2,
                  "image": "assets/images/avatar/15.jpg",
                  "title": "Caleigh Jerde",
                  "time": "18 August 2018 at 8:13pm",
                  "description": "If you could learn any language, what would you choose?"
              },
              {
                  "id": 3,
                  "image": "assets/images/avatar/14.jpg",
                  "title": "Lucas Schultz",
                  "time": "18 August 2018 at 5:22pm",
                  "description": "Life is about making an impact, not making an income."
              },
              {
                  "id": 4,
                  "image": "assets/images/avatar/13.jpg",
                  "title": "Carole Marvin",
                  "time": "18 August 2018 at 7:36am",
                  "description": "I’ve learned that people will forget what you said, people will forget what you did, but people will never forget how you made them feel"
              },
              {
                  "id": 5,
                  "image": "assets/images/avatar/12.jpg",
                  "title": "Doriana Feeney",
                  "time": "18 August 2018 at 5:28am",
                  "description": "Definiteness of purpose is the starting point of all achievement."
              },
              {
                  "id": 6,
                  "image": "assets/images/avatar/11.jpg",
                  "title": "Nia Gutkowski",
                  "time": "18 August 2018 at 11:27pm",
                  "description": "Life is what happens to you while you’re busy making other plans"
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
                    .object('comment/' + item.theme)
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
