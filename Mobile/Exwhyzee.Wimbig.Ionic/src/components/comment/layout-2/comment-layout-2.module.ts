import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CommentLayout2 } from './comment-layout-2';

@NgModule({
    declarations: [
        CommentLayout2,
    ],
    imports: [
        IonicPageModule.forChild(CommentLayout2),
    ],
    exports: [
        CommentLayout2
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class CommentLayout2Module { }
