import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPageRange } from './item-details-range';

import { RangeLayout1Module } from '../../components/range/layout-1/range-layout-1.module';
import { RangeLayout2Module } from '../../components/range/layout-2/range-layout-2.module';
import { RangeLayout3Module } from '../../components/range/layout-3/range-layout-3.module';
import { RangeLayout4Module } from '../../components/range/layout-4/range-layout-4.module';

@NgModule({
  declarations: [
    ItemDetailsPageRange,
  ],
  imports: [
    IonicPageModule.forChild(ItemDetailsPageRange),
    RangeLayout1Module, RangeLayout2Module, RangeLayout3Module, RangeLayout4Module
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPageRangeModule {}
