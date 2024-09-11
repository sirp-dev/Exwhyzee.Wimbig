import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { TimeLineLayout1 } from './time-line-layout-1';

@NgModule({
    declarations: [
        TimeLineLayout1,
    ],
    imports: [
        IonicPageModule.forChild(TimeLineLayout1),
    ],
    exports: [
        TimeLineLayout1
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]

})

export class TimeLineLayout1Module { }
