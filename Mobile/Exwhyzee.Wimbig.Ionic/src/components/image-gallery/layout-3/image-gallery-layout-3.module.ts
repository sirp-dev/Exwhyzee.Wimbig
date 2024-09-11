import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ImageGalleryLayout3 } from './image-gallery-layout-3';

@NgModule({
    declarations: [
        ImageGalleryLayout3,
    ],
    imports: [
        IonicPageModule.forChild(ImageGalleryLayout3),
    ],
    exports: [
        ImageGalleryLayout3
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ImageGalleryLayout3Module { }
