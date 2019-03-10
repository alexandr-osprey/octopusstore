import { Component, OnInit, Input, OnChanges, SimpleChanges, EventEmitter } from '@angular/core';
import { ParameterService } from '../parameter/parameter.service';
import { ParameterNames } from '../parameter/parameter-names';
import { Entity } from '../models/entity/entity';
import { EntityIndex } from '../models/entity/entity-index';
import { Subject, Observable } from 'rxjs';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.css'],
})
export class PaginatorComponent<T extends Entity> implements OnInit, OnChanges {
  @Input() delegate: Pageable<T>;
  pageNumbers: number[];
  pageSizes: number[];

  constructor(
    private parameterService: ParameterService)
  {
  }

  initializeComponent() {
    if (this.delegate.index != null) {
      let pageRange = 5;
      let l = this.delegate.index.page - pageRange;
      l = l < 0 ? l * -1: 0;
      let r = this.delegate.index.page + pageRange + l;
      r = r > this.delegate.index.totalPages ? r - this.delegate.index.totalPages: 0;
      let leftmost = Math.max(1, this.delegate.index.page - pageRange - r);
      let rightmost = Math.min(this.delegate.index.page + pageRange + l + 1, this.delegate.index.totalPages + 1);
      this.pageNumbers = PaginatorComponent.range(leftmost, rightmost);
      this.pageSizes = [1, 4, 5, 10];
    }
    if (this.delegate.nextPageNavigated$) {
      this.delegate.nextPageNavigated$.subscribe((data: any) => {
        this.parameterService.navigateWithParams(this.getPageParams(this.delegate.index.page + 1));
      });
    }
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    this.initializeComponent();
  }

  getPageSizeParams(pageSize: number): any {
    let params = this.parameterService.getUpdatedParamsCopy(
      { "pageSize": pageSize, "page": 1 });
    return params;
  }

  getPageParams(page: number): any {
    let params = this.parameterService.getUpdatedParamsCopy({ "page": page });
    return params;
  }

  static range(start: number, end: number): number[] {
    return Array.from({ length: (end - start) }, (v, k) => k + start);
  }
}

export interface Pageable<T extends Entity> {
  index: EntityIndex<T>;
  nextPageNavigated$: Observable<any>;
}
