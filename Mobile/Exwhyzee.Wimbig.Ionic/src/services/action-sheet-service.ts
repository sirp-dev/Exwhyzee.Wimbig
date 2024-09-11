import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings';
import { ToastService } from './toast-service';
import { LoadingService } from './loading-service';

@Injectable()
export class ActionSheetService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'actionSheet';

    getTitle = (): string => 'Action Sheet';

    getAllThemes = (): Array<any> => {
        return [
            { "title": "Basic", "theme": "layout1" },
            { "title": "News", "theme": "layout2" },
            { "title": "With Text Header", "theme": "layout3" }
        ];
    };

    getDataForTheme = (menuItem: any): any => {
        return this[
            'getDataFor' +
            menuItem.theme.charAt(0).toUpperCase() +
            menuItem.theme.slice(1)
        ]();
    };

    //Basic data
    getDataForLayout1 = (): any => {
        return {
            "headerImage": "assets/images/background/0.jpg",
            "shareIcon": "more",
            "actionSheet": {
                "buttons": [
                    {
                        "text": "Add to Cart",
                        "role": "destructive"
                    },
                    {
                        "text": "Add to Favorites"
                    },
                    {
                        "text": "Read more info"
                    },
                    {
                        "text": "Delete Item"
                    },
                    {
                        "text": "Cancel",
                        "role": "cancel"
                    }
                ]
            },
            "items": [
                {
                    "id": 1,
                    "title": "Australia clade in Anglia Pulvis Blundstone",
                    "category": "Celebrity vitae",
                    "productDescriptions": [
                        {
                            "id": 1,
                            "description": "Cum autem producturus tribus cursibus turpis eget nisi cinxit saeculi altum traditione Maxwell tenues satis gratanter polum noctis vincula purgare subest."
                        },
                        {
                            "id": 2,
                            "description": "Cujus rei manifestum est compositus award victor hominis, Maxwell Mark locutus est ad eum de ludo Nicolaum post perficientur in captura quaestionemque quae ipsius causa et de tali tumultu proficisci imperat. Nice quod nest accipere in unum funem, et consummare officium et pro nobis ut hanc seriem incipere possumus habere in via ob facultatem"
                        },
                        {
                            "id": 3,
                            "description": "Nabu portare potuimus protector meus et in Sacra Pagina formam Magna pars aestiva. Quod fuit placitum est in re."
                        },
                        {
                            "id": 4,
                            "description": "Passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum"
                        }
                    ]
                }
            ]
        }
    };

    //News data
    getDataForLayout2 = (): any => {
        return {
            "headerImage": "assets/images/background/8.jpg",
            "title": "Infinit pontem in Sinis. Quod locus non videre finem pontis. VII deambulatio inter homines pereunt.",
            "subtitle": "by Guerrero Woodard",
            "category": "SEASONAL ITINERIBUS",
            "avatar": "assets/images/avatar/3.jpg",
            "shareIcon": "more",
            "actionSheet": {
                "buttons": [
                    {
                        "text": "Add to Cart",
                        "role": "destructive"
                    },
                    {
                        "text": "Add to Favorites"
                    },
                    {
                        "text": "Read more info"
                    },
                    {
                        "text": "Delete Item"
                    },
                    {
                        "text": "Cancel",
                        "role": "cancel"
                    }
                ]
            },
            "items": [
                {
                    "id": 1,
                    "subtitle": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur."
                },
                {
                    "id": 2,
                    "subtitle": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur."
                },
                {
                    "id": 3,
                    "subtitle": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip."
                },
                {
                    "id": 4,
                    "subtitle": "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum is simply dummy text of the printing and typesetting industry ut labore et dolore magna aliqua. Ut enim ad minim veniam."
                }
            ]
        };
    };

    //With Text Header data
    getDataForLayout3 = (): any => {
        return {
            "shareIcon": "more",
            "actionSheet": {
                "title": "Choose what to do with this card?",
                "buttons": [
                    {
                        "text": "Read more",
                        "role": "destructive"
                    },
                    {
                        "text": "Add to Favorites"
                    },
                    {
                        "text": "Delete Card"
                    },
                    {
                        "text": "Disable Card"
                    },
                    {
                        "text": "Cancel",
                        "role": "cancel"
                    }
                ]
            },
            "items": [
                {
                    "id": 1,
                    "category": "offer optimus",
                    "image": "assets/images/background/2.jpg",
                    "title": "Aliquam libero pontem",
                    "subtitle": "Tiffany Place, Rehrersburg",
                    "button": "$35.99"
                },
                {
                    "id": 2,
                    "category": "res pelagus",
                    "image": "assets/images/background/1.jpg",
                    "title": "Aeris aperta cinematographico",
                    "subtitle": "Rutherford Place, Norvelt",
                    "button": "$12.99"
                },
                {
                    "id": 3,
                    "category": "optimus unius tortae",
                    "image": "assets/images/background/0.jpg",
                    "title": "hiems sit naturæ debitus",
                    "subtitle": "Lewis Avenue, Caspar",
                    "button": "$13.45"
                },
                {
                    "id": 4,
                    "category": "mons",
                    "image": "assets/images/background/3.jpg",
                    "title": "mons piscium",
                    "subtitle": "SMountain Trout Camp",
                    "button": "$38.60"
                },
                {
                    "id": 5,
                    "category": "Aliquam pontem",
                    "image": "assets/images/background/4.jpg",
                    "title": "Aliquam pontem",
                    "subtitle": "Stryker Court, Evergreen",
                    "button": "$40.85"
                },
                {
                    "id": 6,
                    "category": "certe optimus",
                    "image": "assets/images/background/5.jpg",
                    "title": "De musica",
                    "subtitle": "Joval Court, Holtville",
                    "button": "$56.55"
                }
            ]
        };
    }

    getEventsForTheme = (menuItem: any): any => {
        var that = this;
        return {
            'onItemClick': function (item: any) {
                that.toastCtrl.presentToast(item.title);
            },
            'onItemClickActionSheet': function (item: any) {
                that.toastCtrl.presentToast(item.button.text);
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
                    .object('actionSheet/' + item.theme)
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
