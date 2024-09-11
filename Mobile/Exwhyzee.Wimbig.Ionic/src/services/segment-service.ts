import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings'
import { ToastService } from './toast-service'
import { LoadingService } from './loading-service'

@Injectable()
export class SegmentService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'segment';

    getTitle = (): string => 'Segment';

    getAllThemes = (): Array<any> => {
        return [
            { "title": "Segment List", "theme": "layout1" },
            { "title": "Segment Card", "theme": "layout2" },
            { "title": "Segment Post", "theme": "layout3" }
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
            'onButton': function (item: any) {
                that.toastCtrl.presentToast(item.title);
            },
            'onItemClick': function (item: any) {
                that.toastCtrl.presentToast(item.title);
            },
            'onLike': function (item: any) {
                if (item && item.like) {
                    if (item.like.isActive) {
                        item.like.isActive = false;
                        item.like.number--;
                    } else {
                        item.like.isActive = true;
                        item.like.number++;
                    }
                }
            },
            'onComment': function (item: any) {
                if (item && item.comment) {
                    if (item.comment.isActive) {
                        item.comment.isActive = false;
                        item.comment.number--;
                    } else {
                        item.comment.isActive = true;
                        item.comment.number++;
                    }
                }
            }
        };
    };

    getDataForLayout1 = (): any => {
        return {
            "headerTitle": "Segment List",
            "segmentButton1": "New Product",
            "segmentButton2": "Most Sold Products",
            // Data Page 1
            "page1": {
                "background": "assets/images/background/9.jpg",
                "items": [
                    {
                        "id": 1,
                        "title": "Black Shirt",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/17.jpg",
                    },
                    {
                        "id": 2,
                        "title": "Black Sweater",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/18.jpg",
                    },
                    {
                        "id": 3,
                        "title": "White Shirt",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/19.jpg",
                    },
                    {
                        "id": 4,
                        "title": "Shirt",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/20.jpg",
                    },
                    {
                        "id": 5,
                        "title": "T Shirt",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/5.jpg",
                    }
                ]
            },
            // Data Page 2
            "page2": {
                "background": "assets/images/background/6.jpg",
                "items": [
                    {
                        "id": 1,
                        "title": "White T Shirt",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/21.jpg",
                    },
                    {
                        "id": 2,
                        "title": "Hoodies",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/22.jpg",
                    },
                    {
                        "id": 3,
                        "title": "Shirt",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/23.jpg",
                    },
                    {
                        "id": 4,
                        "title": "Sweater",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/17.jpg",
                    },
                    {
                        "id": 5,
                        "title": "White Shirt",
                        "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        "image": "assets/images/avatar/19.jpg",
                    }
                ]
            }
        };
    };

    getDataForLayout2 = (): any => {
        return {
            "headerTitle": "Segment Card",
            "segmentButton1": "New Offer",
            "segmentButton2": "Best Offer",
            // Data Page 1
            "page1": {
                "items": [
                    {
                        "id": 1,
                        "title": "Cape Town",
                        "subtitle": "Category: Travel",
                        "description": "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC",
                        "image": "assets/images/background/3.jpg",
                    },
                    {
                        "id": 2,
                        "title": "Charleston, South Carolina",
                        "subtitle": "Category: Travel",
                        "description": "Here are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form",
                        "image": "assets/images/background/2.jpg",
                    },
                    {
                        "id": 3,
                        "title": "Chiang Mai, Thailand",
                        "subtitle": "Category: Travel",
                        "description": "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC",
                        "image": "assets/images/background/5.jpg",
                    },
                    {
                        "id": 4,
                        "title": "Hoi An, Vietnam",
                        "subtitle": "Category: Travel",
                        "description": "Icero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum",
                        "image": "assets/images/background/6.jpg",
                    },
                ]
            },
            // Data Page 2
            "page2": {
                "items": [
                    {
                        "id": 1,
                        "title": "Luang Prabang",
                        "subtitle": "Category: Travel",
                        "description": "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC",
                        "image": "assets/images/background/8.jpg",
                    },
                    {
                        "id": 2,
                        "title": "Kyoto, Japan",
                        "subtitle": "Category: Travel",
                        "description": "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words",
                        "image": "assets/images/background/4.jpg",
                    },
                    {
                        "id": 3,
                        "title": "Ubud, Indonesia",
                        "subtitle": "Category: Travel",
                        "description": "Icero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum",
                        "image": "assets/images/background/11.jpg",
                    },
                    {
                        "id": 4,
                        "title": "Udaipur, India",
                        "subtitle": "Category: Travel",
                        "description": "Here are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form",
                        "image": "assets/images/background/14.jpg",
                    },
                ]
            }
        };
    };

    getDataForLayout3 = (): any => {
        return {
            "headerTitle": "Segment Post",
            "segmentButton1": "New Post",
            "segmentButton2": "Old Post",
            // Data Page 1
            "page1": {
                "items": [
                    {
                        "id": 1,
                        "image": "assets/images/background/2.jpg",
                        "time": "25 January 2018",
                        "title": "Perth, Australia",
                        "description": "The Western Australian city is home to some of the country's most beautiful beaches...",
                        "like": {
                            "icon":"thumbs-up",
                            "number": "12",
                            "isActive": true
                        },
                        "comment": {
                            "icon":"ios-chatbubbles",
                            "number": "4",
                            "isActive": false
                        }
                    },
                    {
                        "id": 2,
                        "image": "assets/images/background/3.jpg",
                        "time": "03 May 2018",
                        "title": "Hamburg, Germany",
                        "description": "The major port city in northern Germany is the second largest of its kind in the country..",
                        "like": {
                            "icon":"thumbs-up",
                            "number": "12",
                            "isActive": false
                        },
                        "comment": {
                            "icon":"ios-chatbubbles",
                            "number": "4",
                            "isActive": true
                        }
                    },
                    {
                        "id": 3,
                        "image": "assets/images/background/4.jpg",
                        "time": "30 July 2018",
                        "title": "Ottawa, Canada",
                        "description": "This city is considered the most educated in Canada with its wealth of post secondary, research..",
                        "like": {
                            "icon":"thumbs-up",
                            "number": "12",
                            "isActive": true
                        },
                        "comment": {
                            "icon":"ios-chatbubbles",
                            "number": "4",
                            "isActive": false
                        }
                    },
                    {
                        "id": 4,
                        "image": "assets/images/background/5.jpg",
                        "time": "28 April 2018",
                        "title": "Luxembourg City, Luxembourg",
                        "description": "The tiny European country, which borders Belgium, France, and Germany, is incredibly wealthy...",
                        "like": {
                            "icon":"thumbs-up",
                            "number": "12",
                            "isActive": true
                        },
                        "comment": {
                            "icon":"ios-chatbubbles",
                            "number": "4",
                            "isActive": false
                        }
                    }
                ]
            },
            // Data Page 2
            "page2": {
                "items": [
                    {
                        "id": 1,
                        "image": "assets/images/background/6.jpg",
                        "time": "09 May 2018",
                        "title": "Melbourne, Australia",
                        "description": "The coastal city is one of the best places in the world for education and healthcare...",
                        "like": {
                            "icon":"thumbs-up",
                            "number": "12",
                            "isActive": false
                        },
                        "comment": {
                            "icon":"ios-chatbubbles",
                            "number": "4",
                            "isActive": false
                        }
                    },
                    {
                        "id": 2,
                        "image": "assets/images/background/14.jpg",
                        "time": "08 July 2018",
                        "title": "Wellington, New Zealand ",
                        "description": "The second most populous city in New Zealand, and the nation's political centre...",
                        "like": {
                            "icon":"thumbs-up",
                            "number": "12",
                            "isActive": false
                        },
                        "comment": {
                            "icon":"ios-chatbubbles",
                            "number": "4",
                            "isActive": false
                        }
                    },
                    {
                        "id": 3,
                        "image": "assets/images/background/16.jpg",
                        "time": "11 September 2018",
                        "title": "Berlin, Germany",
                        "description": "Germany's capital has an excellent of quality of life — with good employment opportunities...",
                        "like": {
                            "icon":"thumbs-up",
                            "number": "12",
                            "isActive": false
                        },
                        "comment": {
                            "icon":"ios-chatbubbles",
                            "number": "4",
                            "isActive": false
                        }
                    },
                    {
                        "id": 4,
                        "image": "assets/images/background/9.jpg",
                        "time": "23 July 2018",
                        "title": "Amsterdam, Netherlands",
                        "description": "Amsterdam combines modern and urban life with relaxed attitudes toward recreation and leisure...",
                        "like": {
                            "icon":"thumbs-up",
                            "number": "12",
                            "isActive": false
                        },
                        "comment": {
                            "icon":"ios-chatbubbles",
                            "number": "4",
                            "isActive": false
                        }
                    }
                ]
            }
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
                    .object('segment/' + item.theme)
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
