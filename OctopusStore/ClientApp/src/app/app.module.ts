import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms'
import { AppComponent } from './app.component';
import { AppRoutingModule } from './/app-routing.module';
import { HeaderComponent } from './header/header.component';
import { ItemThumbnailComponent } from './item/item-thumbnail/item-thumbnail.component';
import { CategoryIndexComponent } from './category/category-index/category-index.component';
import { MessagesComponent } from './message/messages.component';
import { PaginatorComponent } from './paginator/paginator.component';
import { ItemVariantCreateUpdateComponent } from './item-variant/item-variant-create-update/item-variant-create-update.component';
import { ItemPropertyCreateUpdateComponent } from './item-property/item-property-create-update/item-property-create-update.component';
import { ItemCreateUpdateComponent } from './item/item-create-update/item-create-update.component';
import { ItemThumbnailIndexComponent } from './item/item-thumbnail-index/item-thumbnail-index.component';
import { ItemDetailComponent } from './item/item-detail/item-detail.component';
import { ItemPropertyDetailComponent } from './item-property/item-property-detail/item-property-detail.component';
import { ItemVariantDetailComponent } from './item-variant/item-variant-detail/item-variant-detail.component';
import { ItemImageGalleryComponent } from './item-image/item-image-gallery/item-image-gallery.component';
import { ItemImageCreateUpdateComponent } from './item-image/item-image-create-update/item-image-create-update.component';
import { ItemImageDisplayComponent } from './item-image/item-image-display/item-image-display.component';
import { ItemsNavigationPaneComponent } from './navigation/items-navigation-pane/items-navigation-pane.component';
import { HomepageComponent } from './homepage/homepage/homepage.component';
import { StoreCreateUpdateComponent } from './store/store-create-update/store-create-update.component';
import { StoreDetailComponent } from './store/store-detail/store-detail.component';
import { StoreIndexComponent } from './store/store-index/store-index.component';
import { IdentitySignInComponent } from './identity/identity-sign-in/identity-sign-in.component';
import { DefaultErrorHandler } from './error-handlers/default-error-handler';
import { IdentitySignOutComponent } from './identity/identity-sign-out/identity-sign-out.component';
import { IdentitySignUpComponent } from './identity/identity-sing-up/identity-sign-up.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { CharacteristicPaneComponent } from './navigation/characteristic-pane/characteristic-pane.component';
import { AdministratingHomepageComponent } from './administrating/administrating-homepage/administrating-homepage.component';
import { CategoryCreateUpdateComponent } from './category/category-create-update/category-create-update.component';
import { AppLoadModule } from './app-load';
import { CharacteristicIndexComponent } from './characteristic/characteristic-index/characteristic-index.component';
import { CharacteristicCreateUpdateComponent } from './characteristic/characteristic-create-update/characteristic-create-update.component';
import { CharacteristicValueIndexComponent } from './characteristic-value/characteristic-value-index/characteristic-value-index.component';
import { CharacteristicValueCreateUpdateComponent } from './characteristic-value/characteristic-value-create-update/characteristic-value-create-update.component';
import { BrandIndexComponent } from './brand/brand-index/brand-index.component';
import { BrandCreateUpdateComponent } from './brand/brand-create-update/brand-create-update.component';
import { MeasurementUnitIndexComponent } from './measurement-unit/measurement-unit-index/measurement-unit-index.component';
import { MeasurementUnitCreateUpdateComponent } from './measurement-unit/measurement-unit-create-update/measurement-unit-create-update.component';
import { OrderCreateComponent } from './order/order-create/order-create.component';
import { CartItemThumbnailIndexComponent } from './cart-item/cart-item-thumbnail-index/cart-item-thumbnail-index.component';
import { OrderThumbnailIndexComponent } from './order/order-thumbnail-index/order-thumbnail-index.component';
import { SidebarComponent } from './navigation/sidebar/sidebar.component';

@NgModule({
  declarations: [
    AppComponent,
    ItemDetailComponent,
    ItemThumbnailComponent,
    HeaderComponent,
    ItemThumbnailIndexComponent,
    ItemThumbnailComponent,
    CategoryIndexComponent,
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
    CartItemThumbnailIndexComponent,
    CharacteristicPaneComponent,
    AdministratingHomepageComponent,
    CategoryCreateUpdateComponent,
    CharacteristicIndexComponent,
    CharacteristicCreateUpdateComponent,
    CharacteristicValueIndexComponent,
    CharacteristicValueCreateUpdateComponent,
    BrandIndexComponent,
    BrandCreateUpdateComponent,
    MeasurementUnitIndexComponent,
    MeasurementUnitCreateUpdateComponent,
    OrderCreateComponent,
    OrderThumbnailIndexComponent,
    SidebarComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule.forRoot(), 
    HttpClientModule,
    FormsModule,
    AppLoadModule
  ],
  providers: [{
    provide: ErrorHandler, 
    useClass: DefaultErrorHandler
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
