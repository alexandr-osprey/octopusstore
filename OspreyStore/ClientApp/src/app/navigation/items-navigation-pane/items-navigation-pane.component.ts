import { Component, OnInit } from '@angular/core';
import { ParameterService } from '../../parameter/parameter.service';
import { ParameterNames } from '../../parameter/parameter-names';
import { Characteristic } from 'src/app/characteristic/characteristic';
import { CategoryService } from 'src/app/category/category.service';
import { CharacteristicService } from 'src/app/characteristic/characteristic.service';
import { CharacteristicValue } from 'src/app/characteristic-value/characteristic-value';
import { CategoryWithCharacteristicsDisplayed } from '../characteristic-pane/category-with-characteristics-displayed';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { Category } from 'src/app/category/category';
import { trigger, state, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-items-navigation-pane',
  templateUrl: './items-navigation-pane.component.html',
  styleUrls: ['./items-navigation-pane.component.css'],
  animations: [
    trigger('fadeInFadeOut', [
      state('fadeIn', style({ visibility: 'visible', opacity: 1, display: 'block' })),
      state('fadeOut', style({ visibility: 'hidden', opacity: 0, display: 'none' })),
      transition('fadeOut => fadeIn', [animate('350ms linear')]),
      transition('fadeIn => fadeOut', [animate('0ms linear')]),
    ])
  ],
})
export class ItemsNavigationPaneComponent implements OnInit {
  protected currentCategory: CategoryWithCharacteristicsDisplayed;
  protected allCategories: CategoryWithCharacteristicsDisplayed[] = [];
  protected selectedChacarteristicValueIds: number[] = [];

  constructor(private categoryService: CategoryService,
    private characteristicService: CharacteristicService,
    private parameterService: ParameterService) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.categoryService.index().subscribe((data: EntityIndex<Category>) => {
      if (data && data.entities) {
        this.allCategories = data.entities
          .map(c => {
            let categoryDisplayed = new CategoryWithCharacteristicsDisplayed(c);
            this.characteristicService.index({ "categoryId": categoryDisplayed.id }).subscribe((data2: EntityIndex<Characteristic>) => {
              if (data2 && data2.entities) {
                categoryDisplayed.characteristics = data2.entities.map(cs => new Characteristic(cs));
              }
            });
            return categoryDisplayed;
          });
      }
      this.setCurrentCategory(this.allCategories.find(c => c.id == +this.parameterService.getParam(ParameterNames.categoryId)));
      let filters: string = this.parameterService.getParam(ParameterNames.characteristicsFilter);
      if (filters) {
        this.selectedChacarteristicValueIds = filters.split(';').map(v => +v);
      }
      this.applyFilter(false);
    });
  }

  setCurrentCategory(category: Category) {
    let newCategoryId: number = 0;
    if (category) {
      newCategoryId = category.id;
    }
    else {
      newCategoryId = this.categoryService.rootCategory.id;
    }
    if (!this.currentCategory || newCategoryId !== this.currentCategory.id) {
      this.currentCategory = this.allCategories.find(c => c.id == newCategoryId);
      this.selectedChacarteristicValueIds = [];
    }
  }

  characteristicValueSelected(characteristicValue: CharacteristicValue) {
    let existing = this.selectedChacarteristicValueIds.find(i => i == characteristicValue.id);
    if (!existing) {
      this.selectedChacarteristicValueIds.push(characteristicValue.id);
      this.applyFilter();
    }
  }

  characteristicValueUnselected(characteristicValue: CharacteristicValue) {
    let existing = this.selectedChacarteristicValueIds.find(i => i == characteristicValue.id);
    if (existing) {
      this.selectedChacarteristicValueIds = this.selectedChacarteristicValueIds.filter(i => i != characteristicValue.id);
      this.applyFilter();
    }
  }

  applyFilter(resetPage: boolean = true) {
    let parameters: any = { "characteristicsFilter": this.selectedChacarteristicValueIds.join(';') };
    if (resetPage) {
      parameters["page"] = null;
    }
    this.parameterService.navigateWithUpdatedParams(parameters);
  }
}
