import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings';
import { ToastService } from './toast-service';
import { LoadingService } from './loading-service';

@Injectable()
export class HomeService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService) { }

  getData = () => {
   
        return {
            "toolbarTitle": "WIMBIG",
            //"title": "PLAY SMALL, WIN BIG",
            //"subtitle": "No 1 Nigerian Lottery APP",
            //"subtitle2": "Trust Worthy",
            //"link":"http://csform.com/documentation-for-ionic-2-ui-template-app/",
            //"description": "For better understanding how our template works please read documentation.",
            "background": "assets/images/background/29.jpg"
        };
  };

 
    load(): Observable<any> {
        var that = this;
        that.loadingService.show();
        if (AppSettings.IS_FIREBASE_ENABLED) {
            return new Observable(observer => {
                this.af
                    .object('home')
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
              observer.next(this.getData());
              console.log(this.getData());
                observer.complete();
            });
        }
    }
}
