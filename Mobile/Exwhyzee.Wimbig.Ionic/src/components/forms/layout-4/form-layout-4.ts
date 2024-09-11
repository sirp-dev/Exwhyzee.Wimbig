import { Component, Input } from '@angular/core';
import { IonicPage } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { Camera, CameraOptions } from '@ionic-native/camera';

declare let window: any; 

@IonicPage()
@Component({
    selector: 'form-layout-4',
    templateUrl: 'form.html'
})
export class FormLayout4 {
    @Input() data: any;
    @Input() events: any;

    options: CameraOptions = {
        quality: 100,
        destinationType: this.camera.DestinationType.FILE_URI,
        encodingType: this.camera.EncodingType.JPEG,
        mediaType: this.camera.MediaType.PICTURE,
        correctOrientation: true,
        saveToPhotoAlbum: true,
    }

    item = {
        'description': '',
        'stars':1,
        'imageUrl':''
    };

    constructor(
        private camera: Camera, 
        public alertCtrl: AlertController
    ) { }

    onAddVideoPhoto() {
        this.camera.getPicture(this.options).then((imageData) => {
            if (window.Ionic) {
                this.item.imageUrl = window.Ionic.WebView.convertFileSrc(imageData);
            }
        }, (err) => {
            this.displayErrorAlert(err);
        });
           
    };

    displayErrorAlert(err){
        console.log(err);
        let alert = this.alertCtrl.create({
           title: 'Error',
           subTitle: 'Error while trying to capture picture',
           buttons: ['OK']
         });
         alert.present();
      }
    
    onEvent(event: string, e: any, index:any) {
        if (e) {
            e.stopPropagation();
        }
        if (this.events[event]) {
            this.events[event](this.item);
        }
    }

    onStarClass(items: any, index: number, e: any) {
        this.item.stars = index;
        for (var i = 0; i < items.length; i++) {
          items[i].isActive = i <= index;
        }
        if (this.events['onRates']) {
            this.events['onRates'](index);
        }
    };
    

}
