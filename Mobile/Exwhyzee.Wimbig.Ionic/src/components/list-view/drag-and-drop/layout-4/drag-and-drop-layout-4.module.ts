import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { DragAndDropLayout4 } from './drag-and-drop-layout-4';

@NgModule({
    declarations: [
        DragAndDropLayout4,
    ],
    imports: [
        IonicPageModule.forChild(DragAndDropLayout4),
    ],
    exports: [
        DragAndDropLayout4
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class DragAndDropLayout4Module { }
