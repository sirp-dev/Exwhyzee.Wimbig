import { Component, Input, OnChanges } from '@angular/core';
import { IonicPage, AlertController } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'alert-layout-1',
    templateUrl: 'alert.html'
})
export class AlertLayout1 implements OnChanges{
  @Input('data') data: any;
  @Input('events') events: any;

  constructor(private alertCtrl: AlertController) {
  }

  ngOnChanges(changes: { [propKey: string]: any }) {
    this.data = changes['data'].currentValue;
  }

  presentAlert(item):void {
    let alert = this.alertCtrl.create({
      title:  "DIALOG INFO",
      message: "Lorem Ipsum has been the industry’s standard dummy text ever since the 1500s.",
      subTitle: item.title,
      cssClass : "info-dialog",
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          handler: () => {
            console.log('Cancel clicked');
          }
        },
        {
          text: 'Ok',
          handler: () => {
            console.log('Ok clicked');
          }
        }
      ]
    });
    alert.present();
  }
}
