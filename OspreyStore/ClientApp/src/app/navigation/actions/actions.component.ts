import { Component, OnInit } from '@angular/core';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';
import { trigger, state, transition, style, animate } from '@angular/animations';
import { IdentityService } from 'src/app/identity/identity.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-actions',
  templateUrl: './actions.component.html',
  styleUrls: ['./actions.component.css'],
  animations: [
    trigger('fadeInFadeOut', [
      state('fadeIn', style({ visibility: 'visible', opacity: 1, display: 'block' })),
      state('fadeOut', style({ visibility: 'hidden', opacity: 0, display: 'none' })),
      transition('fadeOut => fadeIn', [animate('350ms linear')]),
      transition('fadeIn => fadeOut', [animate('0ms linear')]),
    ])
  ],
})
export class ActionsComponent implements OnInit {
  isActive: boolean;
  constructor(
    private parameterService: ParameterService,
    private identityService: IdentityService,
  private router: Router) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.parameterService.params$.subscribe(_ => {
      if (this.parameterService.isParamChanged(ParameterNames.actionsShown)) {
        this.isActive = this.parameterService.getParam(ParameterNames.actionsShown) as boolean;
      }
    });
  }

  disable() {
    this.parameterService.navigateWithUpdatedParams({ "actionsShown": false });
  }

  signOut() {
    this.identityService.signOut();
    this.router.navigate([""]);
  }
}
