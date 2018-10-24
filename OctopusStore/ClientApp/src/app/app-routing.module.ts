import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { ItemCreateUpdateComponent } from './item/item-create-update/item-create-update.component';
import { ItemThumbnailsComponent } from './item/item-thumbnails/item-thumbnails.component';
import { ItemDetailsComponent } from './item/item-details/item-details.component';
import { HomepageComponent } from './homepage/homepage/homepage.component';
import { StoreCreateUpdateComponent } from './store/store-create-update/store-create-update.component';
import { StoreDetailsComponent } from './store/store-details/store-details.component';
import { StoreIndexComponent } from './store/store-index/store-index.component';
import { IdentitySignInComponent } from './identity/identity-sign-in/identity-sign-in.component';
import { CreateUpdateAuthorizationGuard } from './guards/create-update-authorization-guard';
import { IdentitySignUpComponent } from './identity/identity-sing-up/identity-sign-up.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { StoreAdministratorsComponent } from './store/store-administrators/store-administrators.component';


const routes: Routes = [
  {
    path: '',
    component: HomepageComponent,
    pathMatch: 'full',
  },

  {
    path: 'signUp',
    component: IdentitySignUpComponent,
    pathMatch: 'full'
  },
  {
    path: 'signIn',
    component: IdentitySignInComponent,
    pathMatch: 'full'
  },
  {
    path: 'items/create',
    pathMatch: 'full',
    canActivate: [CreateUpdateAuthorizationGuard],
    component: ItemCreateUpdateComponent,
  },
  {
    path: 'items/:id/update',
    component: ItemCreateUpdateComponent,
    pathMatch: 'full',
    canActivate: [CreateUpdateAuthorizationGuard],
  },
  {
    path: 'items/:id/details',
    component: ItemDetailsComponent,
    pathMatch: 'full'
  },
  {
    path: 'items',
    component: ItemThumbnailsComponent,
  },
  {
    path: 'stores',
    component: StoreIndexComponent,
  },
  {
    path: 'stores/create',
    component: StoreCreateUpdateComponent,
    pathMatch: 'full',
    canActivate: [CreateUpdateAuthorizationGuard],
  },
  {
    path: 'stores/:id/update',
    component: StoreCreateUpdateComponent,
    pathMatch: 'full',
    canActivate: [CreateUpdateAuthorizationGuard],
  },
  {
    path: 'stores/:id/administrators',
    component: StoreAdministratorsComponent,
    pathMatch: 'full',
    canActivate: [CreateUpdateAuthorizationGuard],
  },
  {
    path: 'stores/:id/details',
    component: StoreDetailsComponent,
  },

  { path: 'pageNotFound', component: PageNotFoundComponent },
  { path: '**', component: PageNotFoundComponent }
]
@NgModule({
  imports: [RouterModule.forRoot(routes, {
    onSameUrlNavigation: 'reload' })],
  exports: [RouterModule],
  providers: [CreateUpdateAuthorizationGuard]
})
export class AppRoutingModule { }
