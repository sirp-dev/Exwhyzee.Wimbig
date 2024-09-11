import { IService } from './IService';
import { AngularFireDatabase } from 'angularfire2/database';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from './app-settings';
import { ToastService } from './toast-service';
import { LoadingService } from './loading-service';

@Injectable()
export class ListViewStickyListHeaderService implements IService {

    constructor(public af: AngularFireDatabase, private loadingService: LoadingService, private toastCtrl: ToastService) { }

    getId = (): string => 'stickyListHeader';

    getTitle = (): string => 'Sticky List header';

    getAllThemes = (): Array<any> => {
        return [
            { "title": "Basic", "theme": "layout1" },
            { "title": "Product", "theme": "layout2" },
            { "title": "Post", "theme": "layout3" },
            { "title": "Small header + large item", "theme": "layout4" }
        ];
    };

//Basic data
    getDataForLayout1 = (): any => {
        return {
            "items": [
                {
                    "groupName": "Jazz",
                    "items": [
                        {
                            "id": 1,
                            "title": "Dejesus Norris",
                            "image": "assets/images/avatar/0.jpg",
                            "price": "$58.99",
                            "subtitle": "58 songs"
                        },
                        {
                            "id": 2,
                            "title": "Gayle Gaines",
                            "image": "assets/images/avatar/1.jpg",
                            "price": "$16.99",
                            "subtitle": "16 songs"
                        },
                        {
                            "id": 3,
                            "title": "Prince Phelps",
                            "image": "assets/images/avatar/7.jpg",
                            "price": "$8.29",
                            "subtitle": "92 songs"
                        }
                    ]
                },
                {
                    "groupName": "Blues",
                    "items": [
                        {
                            "id": 1,
                            "title": "Keri Hudson",
                            "image": "assets/images/avatar/0.jpg",
                            "price": "$13.95",
                            "subtitle": "93 songs"
                        },
                        {
                            "id": 2,
                            "title": "Duran Clayton",
                            "image": "assets/images/avatar/1.jpg",
                            "price": "$3.89",
                            "subtitle": "43 songs"
                        },
                        {
                            "id": 3,
                            "title": "Schmidt English",
                            "image": "assets/images/avatar/2.jpg",
                            "price": "$45.06",
                            "subtitle": "62 songs"
                        },
                        {
                            "id": 4,
                            "title": "Lara Lynn",
                            "image": "assets/images/avatar/3.jpg",
                            "price": "$56.66",
                            "subtitle": "81 songs"
                        },
                        {
                            "id": 5,
                            "title": "Perry Bradley",
                            "image": "assets/images/avatar/7.jpg",
                            "price": "$6.78",
                            "subtitle": "2 songs"
                        }
                    ]
                },
                {
                    "groupName": "Rock and roll",
                    "items": [
                        {
                            "id": 1,
                            "title": "Randall Hurley",
                            "image": "assets/images/avatar/0.jpg",
                            "price": "$3.99",
                            "subtitle": "4 songs"
                        },
                        {
                            "id": 2,
                            "title": "Josefa Gardner",
                            "image": "assets/images/avatar/1.jpg",
                            "price": "$9.99",
                            "subtitle": "51 songs"
                        },
                        {
                            "id": 3,
                            "title": "Vega Hayes",
                            "image": "assets/images/avatar/2.jpg",
                            "price": "$7.99",
                            "subtitle": "40 songs"
                        },
                        {
                            "id": 4,
                            "title": "Burns Marks",
                            "image": "assets/images/avatar/3.jpg",
                            "price": "$45.99",
                            "subtitle": "58 songs"
                        },
                        {
                            "id": 5,
                            "title": "Byrd Hewitt",
                            "image": "assets/images/avatar/7.jpg",
                            "price": "$39.75",
                            "subtitle": "55 songs"
                        }
                    ]
                },
                {
                    "groupName": "R&B",
                    "items": [
                        {
                            "id": 1,
                            "title": "Solis Mullen",
                            "image": "assets/images/avatar/0.jpg",
                            "price": "$34.59",
                            "subtitle": "41 songs"
                        },
                        {
                            "id": 2,
                            "title": "Lottie Stone",
                            "image": "assets/images/avatar/1.jpg",
                            "price": "$44.99",
                            "subtitle": "48 songs"
                        },
                        {
                            "id": 3,
                            "title": "Bates Dunn",
                            "image": "assets/images/avatar/2.jpg",
                            "price": "$89.45",
                            "subtitle": "70 songs"
                        },
                        {
                            "id": 4,
                            "title": "Benton Willis",
                            "image": "assets/images/avatar/3.jpg",
                            "price": "$3.10",
                            "subtitle": "6 songs"
                        }
                    ]
                },
                {
                    "groupName": "Country Song",
                    "items": [
                        {
                            "id": 1,
                            "title": "Barbara Bernard",
                            "image": "assets/images/avatar/4.jpg",
                            "price": "$7.99",
                            "subtitle": "85 songs"
                        },
                        {
                            "id": 2,
                            "title": "Megan Singleton",
                            "image": "assets/images/avatar/5.jpg",
                            "price": "$7.39",
                            "subtitle": "59 songs"
                        },
                        {
                            "id": 3,
                            "title": "Guy Mccoy",
                            "image": "assets/images/avatar/6.jpg",
                            "price": "$13.99",
                            "subtitle": "24 songs"
                        },
                        {
                            "id": 4,
                            "title": "Cline Lindsay",
                            "image": "assets/images/avatar/7.jpg",
                            "price": "$75.95",
                            "subtitle": "40 songs"
                        }
                    ]
                }
            ]
        };
    };

//  Product data
    getDataForLayout2 = (): any => {
        return {
            "items": [
                {
                    "groupName": "Shirts",
                    "items": [
                        {
                            "id": 1,
                            "title": "Trendy White Shirt",
                            "image1": "assets/images/background/19.jpg",
                            "image2": "assets/images/background/15.jpg",
                            "image3": "assets/images/background/16.jpg",
                            "image4": "assets/images/background/17.jpg",
                            "image5": "assets/images/background/18.jpg",
                            "price": "$33.99",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        },
                        {
                            "id": 2,
                            "title": "Trendy Black Shirt",
                            "image1": "assets/images/background/18.jpg",
                            "image2": "assets/images/background/17.jpg",
                            "image3": "assets/images/background/16.jpg",
                            "image4": "assets/images/background/15.jpg",
                            "image5": "assets/images/background/19.jpg",
                            "price": "$28.17",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        }
                    ]
                },
                {
                    "groupName": "Shoes",
                    "items": [
                        {
                            "id": 1,
                            "title": "Trendy White Shoes",
                            "image1": "assets/images/background/16.jpg",
                            "image2": "assets/images/background/18.jpg",
                            "image3": "assets/images/background/19.jpg",
                            "image4": "assets/images/background/15.jpg",
                            "image5": "assets/images/background/17.jpg",
                            "price": "$31.99",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        },
                        {
                            "id": 2,
                            "title": "Black Shoes",
                            "image1": "assets/images/background/17.jpg",
                            "image2": "assets/images/background/18.jpg",
                            "image3": "assets/images/background/16.jpg",
                            "image4": "assets/images/background/15.jpg",
                            "image5": "assets/images/background/19.jpg",
                            "price": "$25.69",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        },
                        {
                            "id": 3,
                            "title": "Trendy Shoes",
                            "image1": "assets/images/background/16.jpg",
                            "image2": "assets/images/background/17.jpg",
                            "image3": "assets/images/background/18.jpg",
                            "image4": "assets/images/background/19.jpg",
                            "image5": "assets/images/background/15.jpg",
                            "price": "$13.66",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        }
                    ]
                },
                {
                    "groupName": "Jacket",
                    "items": [
                        {
                            "id": 1,
                            "title": "Trendy White Jacket",
                            "image1": "assets/images/background/19.jpg",
                            "image2": "assets/images/background/18.jpg",
                            "image3": "assets/images/background/17.jpg",
                            "image4": "assets/images/background/16.jpg",
                            "image5": "assets/images/background/15.jpg",
                            "price": "$17.89",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        },
                        {
                            "id": 2,
                            "title": "Trendy Black Jacket",
                            "image1": "assets/images/background/15.jpg",
                            "image2": "assets/images/background/16.jpg",
                            "image3": "assets/images/background/17.jpg",
                            "image4": "assets/images/background/18.jpg",
                            "image5": "assets/images/background/19.jpg",
                            "price": "$55.01",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        },
                        {
                            "id": 3,
                            "title": "Brown Jacket",
                            "image1": "assets/images/background/16.jpg",
                            "image2": "assets/images/background/17.jpg",
                            "image3": "assets/images/background/18.jpg",
                            "image4": "assets/images/background/19.jpg",
                            "image5": "assets/images/background/15.jpg",
                            "price": "$14.45",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        }
                    ]
                },
                {
                    "groupName": "T-Shirt",
                    "items": [
                        {
                            "id": 1,
                            "title": "Trendy White T-Shirt",
                            "image1": "assets/images/background/19.jpg",
                            "image2": "assets/images/background/15.jpg",
                            "image3": "assets/images/background/16.jpg",
                            "image4": "assets/images/background/17.jpg",
                            "image5": "assets/images/background/18.jpg",
                            "price": "$17.99",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        },
                        {
                            "id": 2,
                            "title": "Black T-Shirt",
                            "image1": "assets/images/background/18.jpg",
                            "image2": "assets/images/background/17.jpg",
                            "image3": "assets/images/background/16.jpg",
                            "image4": "assets/images/background/15.jpg",
                            "image5": "assets/images/background/19.jpg",
                            "price": "$44.99",
                            "buy": "buy",
                            "subtitle": "Free shipping"
                        }
                    ]
                }
            ]
        };
    };

    //Post data
    getDataForLayout3 = (): any => {
        return {
            "items": [
                {
                    "groupName": "Yesterday",
                    "dateTime": "January 19, 2017.",
                    "items": [
                        {
                            "id": 1,
                            "name": "Julia Petersen",
                            "title": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                            "shareIcon": "more",
                            "image": "assets/images/avatar/1.jpg",
                            "subtitle": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehe nderit in voluptate velit esse cillum dolore eu fugiat nulla pariatu"
                        },
                        {
                            "id": 2,
                            "name": "Holman Valencia",
                            "title": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor.",
                            "shareIcon": "more",
                            "image": "assets/images/avatar/2.jpg",
                            "subtitle": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehe nderit in voluptate velit esse cillum dolore eu fugiat nulla pariatu"
                        }
                    ]
                },
                {
                    "groupName": "Today",
                    "dateTime": "June 26, 2017.",
                    "items": [
                        {
                            "id": 1,
                            "name": "Marisa Cain",
                            "title": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                            "shareIcon": "more",
                            "image": "assets/images/avatar/3.jpg",
                            "subtitle": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehe nderit in voluptate velit esse cillum dolore eu fugiat nulla pariatu"
                        },
                        {
                            "id": 2,
                            "name": "Dejesus Norris",
                            "title": "Lorem ipsum dolor sit amet.",
                            "shareIcon": "more",
                            "image": "assets/images/avatar/4.jpg",
                            "subtitle": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehe nderit in voluptate velit esse cillum dolore eu fugiat nulla pariatu"
                        }
                    ]
                },
                {
                    "groupName": "tomorrow",
                    "dateTime": "August 07, 2017.",
                    "items": [
                        {
                            "id": 1,
                            "name": "Gayle Gaines",
                            "title": "Lorem ipsum dolor sit amet, consectetur.",
                            "shareIcon": "more",
                            "image": "assets/images/avatar/5.jpg",
                            "subtitle": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehe nderit in voluptate velit esse cillum dolore eu fugiat nulla pariatu"
                        },
                        {
                            "id": 2,
                            "name": "Prince Phelps",
                            "title": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt.",
                            "shareIcon": "more",
                            "image": "assets/images/avatar/5.jpg",
                            "subtitle": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehe nderit in voluptate velit esse cillum dolore eu fugiat nulla pariatu"
                        }
                    ]
                }
            ]
        };
    };

// Small header + large item data
    getDataForLayout4 = (): any => {
        return {
            "items": [
              {
              title: 'Monday',
              description: '14.02.2018',
              icon: 'icon-calendar-today',
              items: [
                  {
                      id: '',
                      title: 'Duran Clayton ',
                      image: 'assets/images/avatar/0.jpg',
                      iconLike: 'icon-thumb-up',
                      iconBookmark: 'icon-bookmark',
                      iconShare: 'icon-share-variant',
                      description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                  },
                  {
                      id: '',
                      title: 'Schmidt English ',
                      image: 'assets/images/avatar/1.jpg',
                      iconLike: 'icon-thumb-up',
                      iconBookmark: 'icon-bookmark',
                      iconShare: 'icon-share-variant',
                      description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                  },
                  {
                      id: '',
                      title: 'Lara Lynn ',
                      image: 'assets/images/avatar/2.jpg',
                      iconLike: 'icon-thumb-up',
                      iconBookmark: 'icon-bookmark',
                      iconShare: 'icon-share-variant',
                      description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                  },
                  {
                      id: '',
                      title: 'Perry Bradley ',
                      image: 'assets/images/avatar/3.jpg',
                      iconLike: 'icon-thumb-up',
                      iconBookmark: 'icon-bookmark',
                      iconShare: 'icon-share-variant',
                      description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                  },
                  {
                      id: '',
                      title: 'Randall Hurle ',
                      image: 'assets/images/avatar/4.jpg',
                      iconLike: 'icon-thumb-up',
                      iconBookmark: 'icon-bookmark',
                      iconShare: 'icon-share-variant',
                      description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                  },
                  {
                      id: '',
                      title: 'John Smit ',
                      image: 'assets/images/avatar/5.jpg',
                      iconLike: 'icon-thumb-up',
                      iconBookmark: 'icon-bookmark',
                      iconShare: 'icon-share-variant',
                      description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                  }
              ]
          },
          {
                title: 'Tuesday',
                description:'25.01.2018',
                icon: 'icon-calendar-today',
                items: [
                    {
                        id: '',
                        title: 'Josefa Gardner ',
                        image: 'assets/images/avatar/0.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Vega Hayes ',
                        image: 'assets/images/avatar/1.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Fitzgerald Stantont ',
                        image: 'assets/images/avatar/2.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Burns Marks ',
                        image: 'assets/images/avatar/3.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Byrd Hewitt ',
                        image: 'assets/images/avatar/4.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Bates Dunnt ',
                        image: 'assets/images/avatar/5.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    }
                ]
            },
            {
                title: 'Wednesday',
                description: '04.02.2018',
                icon: 'icon-calendar-today',
                items: [
                    {
                        id: '',
                        title: 'Barbara Bernard ',
                        image: 'assets/images/avatar/0.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Guy Mccoy ',
                        image: 'assets/images/avatar/1.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Cline Lindsay ',
                        image: 'assets/images/avatar/2.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Juliette Medina ',
                        image: 'assets/images/avatar/3.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Mildred Clar ',
                        image: 'assets/images/avatar/4.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    },
                    {
                        id: '',
                        title: 'Valdez Bruce ',
                        image: 'assets/images/avatar/5.jpg',
                        iconLike: 'icon-thumb-up',
                        iconBookmark: 'icon-bookmark',
                        iconShare: 'icon-share-variant',
                        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also'
                    }
                ]
            }
            ]
        };
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
            'onProceed': function (item: any) {
                that.toastCtrl.presentToast("onProceed");
            },
            'onBuy': function (item: any) {
                that.toastCtrl.presentToast("Buy");
            },
            'onShare': function (item: any) {
                that.toastCtrl.presentToast("Share");
            },
            'onBookmark': function (item: any) {
                that.toastCtrl.presentToast("Bookmark");
            },
            'onLike': function (item: any) {
                that.toastCtrl.presentToast("Like");
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
                    .object('listView/stickyListHeader/' + item.theme)
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
