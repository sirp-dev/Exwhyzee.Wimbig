import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPageComment } from './item-details-comment';

import { CommentLayout1Module } from '../../components/comment/layout-1/comment-layout-1.module';
import { CommentLayout2Module } from '../../components/comment/layout-2/comment-layout-2.module';

@NgModule({
  declarations: [
    ItemDetailsPageComment,
  ],
  imports: [
    IonicPageModule.forChild(ItemDetailsPageComment),
    CommentLayout1Module, CommentLayout2Module
  ],
  exports: [
    ItemDetailsPageComment
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPageCommentModule {}
