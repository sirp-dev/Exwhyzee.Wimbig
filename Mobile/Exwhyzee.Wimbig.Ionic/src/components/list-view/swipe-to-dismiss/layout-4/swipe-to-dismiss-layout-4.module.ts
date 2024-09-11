import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { SwipeToDismissLayout4 } from './swipe-to-dismiss-layout-4';

@NgModule({
    declarations: [
        SwipeToDismissLayout4,
    ],
    imports: [
        IonicPageModule.forChild(SwipeToDismissLayout4),
    ],
    exports: [
        SwipeToDismissLayout4
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class SwipeToDismissLayout4Module { }
