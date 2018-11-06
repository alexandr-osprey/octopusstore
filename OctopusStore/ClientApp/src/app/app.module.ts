import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms'
import { AppComponent } from './app.component';
import { AppRoutingModule } from './/app-routing.module';
import { HeaderComponent } from './header/header.component';
import { ItemThumbnailComponent } from './item/item-thumbnail/item-thumbnail.component';
import { CategoriesComponent } from './categories/categories.component';
import { MessagesComponent } from './messages/messages.component';
import { PaginatorComponent } from './paginator/paginator.component';
import { ItemVariantCreateUpdateComponent } from './item-variant/item-variant-create-update/item-variant-create-update.component';
import { ItemPropertyCreateUpdateComponent } from './item-property/item-property-create-update/item-property-create-update.component';
import { ItemCreateUpdateComponent } from './item/item-create-update/item-create-update.component';
import { ItemThumbnailsComponent } from './item/item-thumbnails/item-thumbnails.component';
import { ItemDetailComponent } from './item/item-detail/item-detail.component';
import { ItemPropertyDetailComponent } from './item-property/item-property-detail/item-property-detail.component';
import { ItemVariantDetailComponent } from './item-variant/item-variant-detail/item-variant-detail.component';
import { ItemImageGalleryComponent } from './item-image/item-image-gallery/item-image-gallery.component';
import { ItemImageCreateUpdateComponent } from './item-image/item-image-create-update/item-image-create-update.component';
import { ItemImageDisplayComponent } from './item-image/item-image-display/item-image-display.component';
import { ItemsNavigationPaneComponent } from './item/items-navigation-pane/items-navigation-pane.component';
import { HomepageComponent } from './homepage/homepage/homepage.component';
import { StoreCreateUpdateComponent } from './store/store-create-update/store-create-update.component';
import { StoreDetailComponent } from './store/store-detail/store-detail.component';
import { StoreIndexComponent } from './store/store-index/store-index.component';
import { IdentitySignInComponent } from './identity/identity-sign-in/identity-sign-in.component';
import { DefaultErrorHandler } from './error-handlers/default-error-handler';
import { IdentitySignOutComponent } from './identity/identity-sign-out/identity-sign-out.component';
import { IdentitySignUpComponent } from './identity/identity-sing-up/identity-sign-up.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { StoreAdministratorsComponent } from './store/store-administrators/store-administrators.component';
import { CartItemComponent } from './cart/cart-item/cart-item.component';
import { CartComponent } from './cart/cart/cart.component';
import { CartThumbnailComponent } from './cart/cart-thumbnail/cart-thumbnail.component';


@NgModule({
  declarations: [
    AppComponent,
    ItemDetailComponent,
    ItemThumbnailComponent,
    HeaderComponent,
    ItemThumbnailsComponent,
    ItemThumbnailComponent,
    CategoriesComponent,
    MessagesComponent,
    PaginatorComponent,
    ItemImageGalleryComponent,
    ItemCreateUpdateComponent,
    ItemVariantCreateUpdateComponent,
    ItemVariantDetailComponent,
    ItemPropertyCreateUpdateComponent,
    ItemPropertyDetailComponent,
    ItemImageCreateUpdateComponent,
    ItemImageDisplayComponent,
    ItemsNavigationPaneComponent,
    HomepageComponent,
    StoreCreateUpdateComponent,
    StoreDetailComponent,
    StoreIndexComponent,
    IdentitySignUpComponent,
    IdentitySignInComponent,
    IdentitySignOutComponent,
    PageNotFoundComponent,
    StoreAdministratorsComponent,
    CartItemComponent,
    CartComponent,
    CartThumbnailComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule.forRoot(), 
    HttpClientModule,
    FormsModule,
  ],
  providers: [{
    provide: ErrorHandler,
    useClass: DefaultErrorHandler
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
