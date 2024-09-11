import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { SegmentLayout1 } from './segment-layout-1';

@NgModule({
    declarations: [
        SegmentLayout1,
    ],
    imports: [
        IonicPageModule.forChild(SegmentLayout1),
    ],
    exports: [
        SegmentLayout1
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]

})

export class SegmentLayout1Module { }
