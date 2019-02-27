import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ParameterService } from '../parameter/parameter.service';
import { ParameterNames } from '../parameter/parameter-names';
import { Entity } from '../models/entity/entity';
import { EntityIndex } from '../models/entity/entity-index';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.css'],
})
export class PaginatorComponent<T extends Entity> implements OnInit, OnChanges {
  @Input() index: EntityIndex<T>;
  pageNumbers: number[];
  pageSizes: number[];
  pageSizeParamName: string = ParameterNames.pageSize;
  pageParamName: string = ParameterNames.page;

  constructor(
    private parameterService: ParameterService)
  {
  }

  initializeComponent() {
    if (this.index != null) {
      let pageRange = 5;
      let l = this.index.page - pageRange;
      l = l < 0 ? l * -1: 0;
      let r = this.index.page + pageRange + l;
      r = r > this.index.totalPages ? r - this.index.totalPages: 0;
      let leftmost = Math.max(1, this.index.page - pageRange - r);
      let rightmost = Math.min(this.index.page + pageRange + l + 1, this.index.totalPages + 1);
      this.pageNumbers = PaginatorComponent.range(leftmost, rightmost);
      this.pageSizes = [1, 4, 5, 10];
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
