import { NgModule, APP_INITIALIZER } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { AppLoadService } from "./services/app-load.service";

export function getRootCategory(appLoadService: AppLoadService) {
  return () => appLoadService.getRootCategory();
}

@NgModule({
  imports: [HttpClientModule],
  providers: [
    AppLoadService,
    { provide: APP_INITIALIZER, useFactory: getRootCategory, deps: [AppLoadService], multi: true }
  ]
})
export class AppLoadModule { }
