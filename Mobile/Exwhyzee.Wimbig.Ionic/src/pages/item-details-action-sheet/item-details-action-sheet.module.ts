import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPageActionSheet } from './item-details-action-sheet';

import { ActionSheetLayout1Module } from '../../components/action-sheet/layout-1/action-sheet-layout-1.module';
import { ActionSheetLayout2Module } from '../../components/action-sheet/layout-2/action-sheet-layout-2.module';
import { ActionSheetLayout3Module } from '../../components/action-sheet/layout-3/action-sheet-layout-3.module';

@NgModule({
  declarations: [
    ItemDetailsPageActionSheet,
  ],
  imports: [
    IonicPageModule.forChild(ItemDetailsPageActionSheet),
    ActionSheetLayout1Module, ActionSheetLayout2Module, ActionSheetLayout3Module
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPageActionSheetModule {}
