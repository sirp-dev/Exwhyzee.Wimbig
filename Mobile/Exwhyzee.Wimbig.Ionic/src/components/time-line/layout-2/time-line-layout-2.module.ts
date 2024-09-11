import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { TimeLineLayout2 } from './time-line-layout-2';

@NgModule({
    declarations: [
        TimeLineLayout2,
    ],
    imports: [
        IonicPageModule.forChild(TimeLineLayout2),
    ],
    exports: [
        TimeLineLayout2
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class TimeLineLayout2Module { }
