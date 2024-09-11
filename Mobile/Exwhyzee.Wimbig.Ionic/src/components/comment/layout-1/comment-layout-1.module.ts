import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CommentLayout1 } from './comment-layout-1';

@NgModule({
    declarations: [
        CommentLayout1,
    ],
    imports: [
        IonicPageModule.forChild(CommentLayout1),
    ],
    exports: [
        CommentLayout1
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class CommentLayout1Module { }
