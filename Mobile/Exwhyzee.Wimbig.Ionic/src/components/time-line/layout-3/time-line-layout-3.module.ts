import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { TimeLineLayout3 } from './time-line-layout-3';

@NgModule({
    declarations: [
        TimeLineLayout3,
    ],
    imports: [
        IonicPageModule.forChild(TimeLineLayout3),
    ],
    exports: [
        TimeLineLayout3
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class TimeLineLayout3Module { }
