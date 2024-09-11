import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPageToggle } from './item-details-toggle';

import { ToggleLayout1Module } from '../../components/toggle/layout-1/toggle-layout-1.module';
import { ToggleLayout2Module } from '../../components/toggle/layout-2/toggle-layout-2.module';
import { ToggleLayout3Module } from '../../components/toggle/layout-3/toggle-layout-3.module';

@NgModule({
    declarations: [
        ItemDetailsPageToggle,
    ],
    imports: [
        IonicPageModule.forChild(ItemDetailsPageToggle),
        ToggleLayout1Module, ToggleLayout2Module, ToggleLayout3Module
    ],
    exports: [
        ItemDetailsPageToggle
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPageToggleModule { }
