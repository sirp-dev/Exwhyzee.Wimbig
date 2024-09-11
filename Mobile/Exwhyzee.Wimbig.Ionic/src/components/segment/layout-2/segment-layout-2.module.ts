import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { SegmentLayout2 } from './segment-layout-2';

@NgModule({
    declarations: [
        SegmentLayout2,
    ],
    imports: [
        IonicPageModule.forChild(SegmentLayout2),
    ],
    exports: [
        SegmentLayout2
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class SegmentLayout2Module { }
