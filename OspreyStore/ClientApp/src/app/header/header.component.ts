import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { IdentityService } from '../identity/identity.service';
import { ParameterService } from '../parameter/parameter.service';
import { ParameterNames } from '../parameter/parameter-names';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  public signedInSubscription: Subscription;
  public signedIn: boolean;
  public email: string;
  public searchValue: string;
  public isContentAdministrator: boolean;
  public storeId: number;

  constructor(
    private identityService: IdentityService,
    private parameterService: ParameterService,
    private router: Router)
  {
    this.signedIn = this.identityService.signedIn;
    this.fillActions();
      
    this.signedInSubscription = this.identityService.signedIn$.subscribe(
      signedIn => {
        this.signedIn = signedIn;
        this.fillActions();
      });
  }

  ngOnInit() {
    //this.tourMessages['search'] = true;
    this.parameterService.params$.subscribe(_ => {
      this.searchValue = this.parameterService.getParam("searchValue");
    });
  }

  search() {
    if (this.searchValue || this.searchValue === "") {
      this.parameterService.navigateWithParams(
        {
          "searchValue": this.searchValue,
          "page": null,
          "categoryId": this.parameterService.getParam(ParameterNames.categoryId)
        }, "/items");
    }
  }

  getStoreParams(): any {
    return { "storeId": this.storeId };
  }
  signOut() {
    this.identityService.signOut();
    this.router.navigate([""]);
  }

  shouldShowSidebar(): boolean {
    let url = this.parameterService.getCurrentUrlWithoutParams();
    return url == "/items" || (url.startsWith("/stores") && url.endsWith("detail"));
  }

  switchSidebar() {
    let currentState: boolean = this.parameterService.getParam(ParameterNames.sidebarHidden);
    this.parameterService.navigateWithUpdatedParams({ "sidebarHidden": !currentState });
  }

  switchActions() {
    let currentState: boolean = this.parameterService.getParam(ParameterNames.actionsShown);
    this.parameterService.navigateWithUpdatedParams({ "actionsShown": !currentState });
  }

  fillActions() {
    this.email = this.identityService.currentUserEmail;
    this.isContentAdministrator = this.identityService.isContentAdministrator();
    let administratedStoreIds = this.identityService.getUserAdministredStoreIds();
    if (administratedStoreIds.length > 0) {
      this.storeId = administratedStoreIds[0];
    } else {
      this.storeId = 0;
    }
  }

  hideTourMessage(name: string) {
    //this.tourMessages[name] = false;
  }
}
