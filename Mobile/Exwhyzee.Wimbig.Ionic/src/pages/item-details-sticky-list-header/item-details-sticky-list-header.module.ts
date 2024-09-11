import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPageStickyListHeader } from './item-details-sticky-list-header';

import { StickyListHeaderLayout1Module } from '../../components/list-view/sticky-list-header/layout-1/sticky-list-header-layout-1.module';
import { StickyListHeaderLayout2Module } from '../../components/list-view/sticky-list-header/layout-2/sticky-list-header-layout-2.module';
import { StickyListHeaderLayout3Module } from '../../components/list-view/sticky-list-header/layout-3/sticky-list-header-layout-3.module';
import { StickyListHeaderLayout4Module } from '../../components/list-view/sticky-list-header/layout-4/sticky-list-header-layout-4.module';

@NgModule({
  declarations: [
    ItemDetailsPageStickyListHeader,
  ],
  imports: [
    IonicPageModule.forChild(ItemDetailsPageStickyListHeader),
    StickyListHeaderLayout1Module, StickyListHeaderLayout2Module,
    StickyListHeaderLayout3Module, StickyListHeaderLayout4Module
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPageStickyListHeaderModule {}
