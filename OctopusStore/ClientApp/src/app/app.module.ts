import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
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
import { ItemVariantCharacteristicValueCreateUpdateComponent } from './item-variant-characteristic-value/item-variant-characteristic-value-create-update/item-variant-characteristic-value-create-update.component';
import { ItemCreateUpdateComponent } from './item/item-create-update/item-create-update.component';
import { ItemThumbnailsComponent } from './item/item-thumbnails/item-thumbnails.component';
import { ItemDetailsComponent } from './item/item-details/item-details.component';
import { ItemVariantCharacteristicValueDetailsComponent } from './item-variant-characteristic-value/item-variant-characteristic-value-details/item-variant-characteristic-value-details.component';
import { ItemVariantDetailsComponent } from './item-variant/item-variant-details/item-variant-details.component';
import { ItemImageGalleryComponent } from './item-image/item-image-gallery/item-image-gallery.component';
import { ItemImageCreateUpdateComponent } from './item-image/item-image-create-update/item-image-create-update.component';
import { ItemImageDisplayComponent } from './item-image/item-image-display/item-image-display.component';
import { ItemsNavigationPaneComponent } from './item/items-navigation-pane/items-navigation-pane.component';
import { HomepageComponent } from './homepage/homepage/homepage.component';
import { StoreCreateUpdateComponent } from './store/store-create-update/store-create-update.component';
import { StoreDetailsComponent } from './store/store-details/store-details.component';
import { StoreIndexComponent } from './store/store-index/store-index.component';


@NgModule({
  declarations: [
    AppComponent,
    ItemDetailsComponent,
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
    ItemVariantDetailsComponent,
    ItemVariantCharacteristicValueCreateUpdateComponent,
    ItemVariantCharacteristicValueDetailsComponent,
    ItemImageCreateUpdateComponent,
    ItemImageDisplayComponent,
    ItemsNavigationPaneComponent,
    HomepageComponent,
    StoreCreateUpdateComponent,
    StoreDetailsComponent,
    StoreIndexComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule.forRoot(), 
    HttpClientModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
