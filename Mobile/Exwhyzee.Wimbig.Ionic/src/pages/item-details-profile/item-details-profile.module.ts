import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ItemDetailsPageProfile } from './item-details-profile';

import { ProfileLayout1Module } from '../../components/profile/layout-1/profile-layout-1.module';
import { ProfileLayout2Module } from '../../components/profile/layout-2/profile-layout-2.module';
import { ProfileLayout3Module } from '../../components/profile/layout-3/profile-layout-3.module';
import { ProfileLayout4Module } from '../../components/profile/layout-4/profile-layout-4.module';
import { ProfileLayout5Module } from '../../components/profile/layout-5/profile-layout-5.module';

@NgModule({
  declarations: [
    ItemDetailsPageProfile,
  ],
  imports: [
    IonicPageModule.forChild(  ItemDetailsPageProfile),
    ProfileLayout1Module, ProfileLayout2Module, ProfileLayout3Module,
    ProfileLayout4Module, ProfileLayout5Module
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ItemDetailsPageProfileModule {}
