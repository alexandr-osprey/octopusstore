import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { ItemCreateUpdateComponent } from './item/item-create-update/item-create-update.component';
import { ItemThumbnailsComponent } from './item/item-thumbnails/item-thumbnails.component';
import { ItemDetailsComponent } from './item/item-details/item-details.component';
import { HomepageComponent } from './homepage/homepage/homepage.component';
import { StoreCreateUpdateComponent } from './store/store-create-update/store-create-update.component';
import { StoreDetailsComponent } from './store/store-details/store-details.component';
import { StoreIndexComponent } from './store/store-index/store-index.component';


const routes: Routes = [
  {
    path: '',
    component: HomepageComponent,
    pathMatch: 'full'
  },
  {
    path: 'items', 
    children: [
      {
        path: 'create',
        component: ItemCreateUpdateComponent,
        pathMatch: 'full'
      },
      {
        path: ':id/update',
        component: ItemCreateUpdateComponent,
        pathMatch: 'full'
      },
      {
        path: ':id/details',
        component: ItemDetailsComponent,
        pathMatch: 'full'
      },
      {
        path: '',
        component: ItemThumbnailsComponent,
      },
    ]
  },
  {
    path: 'stores',
    children: [
      {
        path: 'create',
        component: StoreCreateUpdateComponent,
        pathMatch: 'full'
      },
      {
        path: ':id/update',
        component: StoreCreateUpdateComponent,
        pathMatch: 'full'
      },
      {
        path: ':id/details',
        component: StoreDetailsComponent,
      },
      {
        path: '',
        component: StoreIndexComponent,
      }
    ],
  },
]
@NgModule({
  imports: [RouterModule.forRoot(routes, {
    onSameUrlNavigation: 'reload' })],
  exports: [ RouterModule ]
})
export class AppRoutingModule { }
