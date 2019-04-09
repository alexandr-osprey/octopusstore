import { Component, OnInit, OnDestroy, AfterViewInit, HostBinding, HostListener  } from '@angular/core';
import { ParameterService } from '../../parameter/parameter.service';
import { debounceTime } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { ParameterNames } from '../../parameter/parameter-names';
import { ItemService } from '../item.service';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { ItemThumbnail } from '../item-thumbnail';
import { DisplayedItemThumbnail } from '../displayed-item-thumbnail';
import { trigger, style, state, animate, transition } from '@angular/animations';

@Component({
  selector: 'app-item-thumbnail-index',
  templateUrl: './item-thumbnail-index.component.html',
  styleUrls: ['./item-thumbnail-index.component.css'],
  providers: [ItemService],
  animations: [
    trigger('expandCollapse', [
      state('expanded', style({ width: '*', opacity: 1, display: 'block' })),
      state('collapsed', style({ width: 0, opacity: 0, display: 'none' })),
      transition('expanded => collapsed', [animate('350ms linear')]),
      transition('collapsed => expanded', [animate('350ms linear')]),
      //transition('collapsed => expanded', [animate('350ms linear')]),
    ]),
  ],
})

export class ItemThumbnailIndexComponent implements OnInit, OnDestroy, AfterViewInit {
  @HostBinding('class') classes = 'flex-grow-1';
  index: EntityIndex<ItemThumbnail>;
  shownItems: DisplayedItemThumbnail[] = [];
  parametersSubsription: Subscription;
  loadedPages: number[] = [];
  //nextNavigationOperation = Operation.Initial;
  sidebarHidden: boolean = true;
  fabHidden: boolean = true;

  @HostListener('window:scroll', ['$event']) // for window scroll events
  onScroll(event) {
    this.fabHidden = window.scrollY < 500;
    //console.log("ScrollY: " + window.scrollY);
  }

  constructor(
    private itemService: ItemService,
    private parameterService: ParameterService)
  {
  }
  error: string;
  //navigationSubscription;

  ngAfterViewInit() {
    let page = +this.parameterService.getParam("page");
    page = page ? page : 1;
    //this.bindWaypoints(page);
  }

  isSortingActive(paramName: string) {

  }

  ngOnInit() {
    //window.addEventListener('scroll', this.scroll, true);
    //this.nextNavigationOperation = Operation.Initial;
    this.parametersSubsription = this.parameterService.params$.pipe(
      debounceTime(10),
      //distinctUntilChanged(),
    ).subscribe(
      params => {
        let page = +this.parameterService.getParam(ParameterNames.page);
        if (this.parameterService.isParamChanged(ParameterNames.characteristicsFilter)
          || this.parameterService.isParamChanged(ParameterNames.categoryId)
          || this.parameterService.isParamChanged(ParameterNames.orderBy)
          || this.parameterService.isParamChanged(ParameterNames.orderByDescending)
          || this.parameterService.isParamChanged(ParameterNames.orderByDescending)
          || this.parameterService.isParamChanged(ParameterNames.searchValue)
          || this.parameterService.isParamChanged(ParameterNames.pageSize)
          || (page && !this.loadedPages.some(p => p == page))) {
          if (this.parameterService.isParamChanged(ParameterNames.page)
            || this.parameterService.isParamChanged(ParameterNames.pageSize)) {
            this.scrollToTopFast();
          }
          this.getItems();
        };
        if (this.parameterService.isParamChanged(ParameterNames.sidebarHidden)) {
          this.sidebarHidden = this.parameterService.getParam(ParameterNames.sidebarHidden);
        }
      }
    );
    //this.parameterService.clearParams();
    this.getItems();
    this.sidebarHidden = this.parameterService.getParam(ParameterNames.sidebarHidden);
  }

  getOrderByPriceQueryParams(): any {
    return this.getOrderByQueryParams('price');
  }

  getOrderByTitleQueryParams(): any {
    return this.getOrderByQueryParams('title');
  }

  getOrderByQueryParams(paramName: string): any {
    let updatedParams = this.parameterService.getUpdatedParamsCopy({"orderBy": paramName});
    updatedParams[ParameterNames.page] = 1;
    updatedParams[ParameterNames.orderByDescending] = this.getOrderByDescending(paramName);
    return updatedParams;
  }



  getOrderByDescending(paramName: string): boolean {
    let param = this.parameterService.getParam(ParameterNames.orderBy) == paramName;
    if (!param)
      return false;
    // desc is "false"
    //let desc: boolean = this.parameterService.getParam(ParameterNames.orderByDescending);
    // but orderByDesc is false! wtf
    //let orderByDesc: boolean = !desc;
    let desc: boolean = this.parameterService.getParam(ParameterNames.orderByDescending) == "true";
    let orderByDesc: boolean = !desc;
    return orderByDesc;
  }

  getPageParams(page: number): any {
    let params = this.parameterService.getUpdatedParamsCopy({ "page": page });
    return params;
  }


  //when page scrolled down to bottom or up, url updated to point to a new page, and this function called to fetch a new data from server
  getItems(): void {
    this.itemService.indexThumbnails().subscribe((data: EntityIndex<ItemThumbnail>) => {
      this.index = data;
      this.index.entities = data.entities.map(e => new ItemThumbnail(e));
      //if (this.nextNavigationOperation == Operation.Prepend) {
      //  this.appendItems(this.index);
      //} else if (this.nextNavigationOperation == Operation.Append) {
      //  this.prependItems(this.index);
      //} else {
        this.replaceItems(this.index);
      //}
      this.loadedPages.push(data.page);
      //this.nextNavigationOperation = Operation.Initial;
      //this.bindWaypoints(this.index.page);
    });
  }
  //bindWaypoints(page: number) {
  //  let insertedElements = document.getElementsByClassName("item-thumbnail-of-page-" + page);
  //  for (let i = 0; i < insertedElements.length; i++) {
  //    let e: HTMLElement = insertedElements[i] as HTMLElement;
  //    if (e) {
  //      let waypoint = new Waypoint({
  //        element: e,
  //        handler: function (direction) {
  //          console.log("IT WORKS!!1 " + direction + "PAGE: " + page);
  //        }
  //      });
  //    }
  //  }
  //}

  scrollToTopFast() {
    this.scrollToTop(200);
  }

  scrollToTop(speed: number) {
    let scrollToTop = window.setInterval(() => {
      let pos = window.pageYOffset;
      if (pos > 0) {
        window.scrollTo(0, pos - speed); // how far to scroll on each step
      } else {
        window.clearInterval(scrollToTop);
      }
    }, 16);
  }

  appendItems(index: EntityIndex<ItemThumbnail>) {
    this.addItems(index, 'push');
  }

  replaceItems(index: EntityIndex<ItemThumbnail>) {
    this.loadedPages = [];
    this.addItems(index, 'push', true);
  }

  prependItems(index: EntityIndex<ItemThumbnail>) {
    this.addItems(index, 'unshift');
  }

  addItems(index: EntityIndex<ItemThumbnail>, _method: string, clear: boolean = false) {
    if (clear)
      this.shownItems = [];
    for (let i = 0; i < index.entities.length; ++i) {
      let d = new DisplayedItemThumbnail(index.entities[i]);
      d.page = index.page;
      this.shownItems[_method](d);
    }
  }

  

  ngOnDestroy() {
    // prevent memory leak when component destroyed
    this.parametersSubsription.unsubscribe();
  }

  //onScrollDown() {
  //  this.nextNavigationOperation = Operation.Append;
  //  let nextPage = this.index.page + 1;
  //  if (this.index.totalPages >= nextPage) {
  //    this.parameterService.navigateWithParams(this.getPageParams(nextPage));
  //  }
  //}
  //onScrollUp() {
  //  this.nextNavigationOperation = Operation.Prepend;
  //  let nextPage = this.index.page - 1;
  //  if (nextPage > 0) {
  //    this.parameterService.navigateWithParams(this.getPageParams(nextPage));
  //  }
  //}
}
//export enum Operation {
//  Initial,
//  Append,
//  Prepend,
//}
