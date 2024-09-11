import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPageForm } from './item-details-form';

import { FormLayout1Module } from '../../components/forms/layout-1/form-layout-1.module';
import { FormLayout2Module } from '../../components/forms/layout-2/form-layout-2.module';
import { FormLayout3Module } from '../../components/forms/layout-3/form-layout-3.module';
import { FormLayout4Module } from '../../components/forms/layout-4/form-layout-4.module';

@NgModule({
  declarations: [
    ItemDetailsPageForm,
  ],
  imports: [
    IonicPageModule.forChild(  ItemDetailsPageForm),
    FormLayout1Module, FormLayout2Module, FormLayout3Module,
    FormLayout4Module
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPageFormeModule {}
