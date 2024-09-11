import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { SegmentLayout3 } from './segment-layout-3';

@NgModule({
    declarations: [
        SegmentLayout3,
    ],
    imports: [
        IonicPageModule.forChild(SegmentLayout3),
    ],
    exports: [
        SegmentLayout3
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class SegmentLayout3Module { }
