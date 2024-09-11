import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPageSegment } from './item-details-segment';

import { SegmentLayout1Module } from '../../components/segment/layout-1/segment-layout-1.module';
import { SegmentLayout2Module } from '../../components/segment/layout-2/segment-layout-2.module';
import { SegmentLayout3Module } from '../../components/segment/layout-3/segment-layout-3.module';

@NgModule({
  declarations: [
    ItemDetailsPageSegment,
  ],
  imports: [
    IonicPageModule.forChild(ItemDetailsPageSegment),
    SegmentLayout1Module, SegmentLayout2Module, SegmentLayout3Module
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPageTimeLineModule {}
