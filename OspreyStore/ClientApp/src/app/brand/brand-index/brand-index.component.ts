import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { BrandService } from '../brand.service';
import { Brand } from '../brand';

@Component({
  selector: 'app-brand-index',
  templateUrl: './brand-index.component.html',
  styleUrls: ['./brand-index.component.css']
})
export class BrandIndexComponent implements OnInit {
  public brands: Brand[] = [];
  @Input() administrating: boolean;

  constructor(
    private brandService: BrandService,
    private router: Router) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
      this.brandService.index().subscribe(data => {
        if (data) {
          this.brands = [];
          data.entities.forEach(b => {
            this.brands.push(new Brand(b));
          });
        }
      });
  }

  create() {
    this.router.navigate(['/brands/create']);
  }
}
