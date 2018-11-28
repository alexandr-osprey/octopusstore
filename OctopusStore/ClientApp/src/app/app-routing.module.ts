import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { ItemCreateUpdateComponent } from './item/item-create-update/item-create-update.component';
import { ItemThumbnailsComponent } from './item/item-thumbnails/item-thumbnails.component';
import { ItemDetailComponent } from './item/item-detail/item-detail.component';
import { HomepageComponent } from './homepage/homepage/homepage.component';
import { StoreCreateUpdateComponent } from './store/store-create-update/store-create-update.component';
import { StoreDetailComponent } from './store/store-detail/store-detail.component';
import { StoreIndexComponent } from './store/store-index/store-index.component';
import { IdentitySignInComponent } from './identity/identity-sign-in/identity-sign-in.component';
import { CreateUpdateAuthorizationGuard } from './guards/create-update-authorization-guard';
import { IdentitySignUpComponent } from './identity/identity-sing-up/identity-sign-up.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { StoreAdministratorsComponent } from './store/store-administrators/store-administrators.component';
import { CartComponent } from './cart/cart/cart.component';
import { AdministratingHomepageComponent } from './administrating/administrating-homepage/administrating-homepage.component';
import { AdministratorAuthorizationGuard } from './guards/administrator-authorization-guard';


const routes: Routes = [
  {
    path: '',
    component: HomepageComponent,
    pathMatch: 'full',
  },
  {
    path: 'cart',
    component: CartComponent,
    pathMatch: 'full',
  },
  {
    path: 'administration',
    component: AdministratingHomepageComponent,
    canActivate: [AdministratorAuthorizationGuard],
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
    path: 'items/:id/detail',
    component: ItemDetailComponent,
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
    path: 'stores/:id/detail',
    component: StoreDetailComponent,
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
