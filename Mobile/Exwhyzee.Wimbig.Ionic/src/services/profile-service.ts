import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings'
import { ToastService } from './toast-service'
import { LoadingService } from './loading-service'

@Injectable()
export class ProfileService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'profile';

    getTitle = (): string => 'Profile';

    getAllThemes = (): Array<any> => {
        return [
            { "title": "Profile With Avatar", "theme": "layout1" },
            { "title": "Profile with Slider + Comments", "theme": "layout2" },
            { "title": "Profile Basic", "theme": "layout3" },
            { "title": "Profile With Slider", "theme": "layout4" },
            { "title": "Profile With Big Image", "theme": "layout5" }
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
            'onInstagram': function (item: any) {
                that.toastCtrl.presentToast("onInstagram");
            },
            'onFacebook': function (item: any) {
                that.toastCtrl.presentToast("onFacebook");
            },
            'onTwitter': function (item: any) {
                that.toastCtrl.presentToast("onTwitter");
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
            "image": "assets/images/avatar/20.jpg",
            "title": "Samantha Kennedy",
            "subtitle": "Extreme coffee lover. Twitter maven. Internet practitioner. Beeraholic.",
            "category": "populary",
            "items": [
                {
                    "id": 1,
                    "category": "Engineering News",
                    "title": "Small flying robots haul heavy loads",
                    "like": {
                        "icon": "thumbs-up",
                        "number": "4",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                },
                {
                    "id": 2,
                    "category": "Bioengineering News",
                    "title": "Creating custom brains from the ground up",
                    "like": {
                        "icon": "thumbs-up",
                        "number": "4",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                },
                {
                    "id": 3,
                    "category": "Energy and Resources News",
                    "title": "3D-printed lithium-ion batteries",
                    "like": {
                        "icon": "thumbs-up",
                        "number": "4",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                }
            ]
        };
    };

    getDataForLayout2 = (): any => {
        return {
            "image": "assets/images/avatar/22.jpg",
            "title": "Carolyn Guerrero",
            "subtitle": "Extreme coffee lover. Twitter maven. Internet practitioner. Beeraholic.",
            "category": "populary",
            "followers": "Followers",
            "valueFollowers": "439",
            "following": "Following",
            "valueFollowing": "297",
            "posts": "Posts",
            "valuePosts": "43",
            "items": [
                {
                    "id": 1,
                    "category": "Engineering News",
                    "title": "New definition returns meaning to information",
                    "like": {
                        "icon": "thumbs-up",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                },
                {
                    "id": 2,
                    "category": "Science News",
                    "title": "Investigating glaciers in depth",
                    "like": {
                        "icon": "thumbs-up",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                },
                {
                    "id": 3,
                    "category": "Science News",
                    "title": "Nanodiamonds as photocatalysts",
                    "like": {
                        "icon": "thumbs-up",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                }
            ]
        };
    };

    getDataForLayout3 = (): any => {
        return {
            "image": "assets/images/avatar/10.jpg",
            "title": "Carolyn Guerrero",
            "subtitle": "Extreme coffee lover. Twitter maven. Internet practitioner. Beeraholic.",
            "category": "populary",
            "followers": "Followers",
            "valueFollowers": "439",
            "following": "Following",
            "valueFollowing": "297",
            "posts": "Posts",
            "valuePosts": "43",
            "items": [
                {
                    "id": 1,
                    "category": "dinner",
                    "backgroundCard": "assets/images/background/19.jpg",
                    "title": "Vegetable Salad with greens",
                    "like": {
                        "icon": "thumbs-up",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                },
                {
                    "id": 2,
                    "category": "BREAKFAST",
                    "backgroundCard": "assets/images/background/21.jpg",
                    "title": "Gigantes with paprika and greens",
                    "like": {
                        "icon": "thumbs-up",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                },
                {
                    "id": 3,
                    "category": "BREAKFAST",
                    "backgroundCard": "assets/images/background/20.jpg",
                    "title": "Penne with Chicken and Asparagus",
                    "like": {
                        "icon": "thumbs-up",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                }
            ]
        };
    };

    getDataForLayout4 = (): any => {
        return {
            "image": "assets/images/avatar/16.jpg",
            "title": "Katie Murray",
            "subtitle": "Extreme coffee lover. Twitter maven. Internet practitioner. Beeraholic.",
            "category": "populary",
            "followers": "Followers",
            "valueFollowers": "439",
            "following": "Following",
            "valueFollowing": "297",
            "posts": "Posts",
            "valuePosts": "43",
            "items": [
                {
                    "id": 1,
                    "category": "BREAKFAST",
                    "backgroundCard": "assets/images/background/21.jpg",
                    "title": "Penne with Chicken and Asparagus",
                    "like": {
                        "icon": "thumbs-up",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                },
                {
                    "id": 2,
                    "category": "BREAKFAST",
                    "backgroundCard": "assets/images/background/19.jpg",
                    "title": "Gigantes with paprika and greens",
                    "like": {
                        "icon": "thumbs-up",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                },
                {
                    "id": 3,
                    "category": "dinner",
                    "backgroundCard": "assets/images/background/20.jpg",
                    "title": "Vegetable Salad with greens",
                    "like": {
                        "icon": "thumbs-up",
                        "text": "Like",
                        "isActive": true
                    },
                    "comment": {
                        "icon": "ios-chatbubbles",
                        "number": "4",
                        "text": "Comments",
                        "isActive": false
                    }
                }
            ]
        };
    };

    getDataForLayout5 = (): any => {
        return {
            "headerImage": "assets/images/background/29.jpg",
            "image": "assets/images/avatar/12.jpg",
            "title": "Jennifer Reid",
            "subtitle": "Extreme coffee lover. Twitter maven. Internet practitioner. Beeraholic.",
            "category": "populary",
            "followers": "Followers",
            "valueFollowers": "439",
            "following": "Following",
            "valueFollowing": "297",
            "posts": "Posts",
            "valuePosts": "43",
            "logoFacebook": "logo-facebook",
            "logoTwitter": "logo-twitter",
            "logoInstagram": "logo-instagram",
            "items": [
                {
                    "id": 1,
                    "iconPhone": "ios-phone-portrait",
                    "iconMail": "mail-open",
                    "iconGlobe": "globe",
                    "phone": "i598-968-5698987",
                    "mail": "dev@csform.com",
                    "globe": "csform.com",
                    "content": "Content",
                    "subtitle": "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    "title": "About",
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
                    .object('profile/' + item.theme)
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
