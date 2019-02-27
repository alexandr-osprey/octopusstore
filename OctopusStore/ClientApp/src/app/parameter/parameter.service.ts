import { ActivatedRoute, ParamMap, Router, NavigationEnd } from "@angular/router";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { MessageService } from "../message/message.service";

@Injectable({
  providedIn: 'root'
})
export class ParameterService {
  protected params: any = {};
  protected paramsSource = new Subject<any>();
  public params$ = this.paramsSource.asObservable();

  constructor(
    protected messageService: MessageService,
    protected route: ActivatedRoute,
    protected router: Router) {
    this.router.events.subscribe(val => {
      if (val instanceof NavigationEnd) {
        this.params = this.paramMapToParams(this.route.snapshot.queryParamMap);
        Object.entries(this.params).forEach((pair) =>
          console.log(pair[0] + ": " + pair[1])
        );
        this.paramsSource.next(this.params);
      }
    });
    //this.route.queryParamMap.subscribe(
    //  (params: ParamMap) => {
    //    this.params = ParameterService.paramMapToParams(params);
    //  });
  }

  public getParamsCopy(): any {
    let params: any = {};
    Object.assign(params, this.params);
    return params;
  }
  public getParam(key: string): any {
    return this.params[key];
  }

  public getUpdatedParams(pairs: any): any {
    return this.assignParams(this.params, pairs);
  }

  public getUpdatedParamsCopy(pairs: any): any {
    return this.assignParams(this.getParamsCopy(), pairs);
  }

  private assignParams(params: any, pairs: any): any {
    for (let key in pairs) {
      if (pairs[key]) {
        params[key] = pairs[key];
      } else {
        delete params[key];
      }
    }
    return params;
  }

  public navigateWithUpdatedParams(pairs: any): void {
    let params = this.getUpdatedParams(pairs);
    this.router.navigate([ParameterService.getUrlWithoutParams(this.router)], { queryParams: params });
  }

  public navigateWithParams(params: any, url: string = null) {
    url = url ? url : ParameterService.getUrlWithoutParams(this.router);
    this.router.navigate([url], { queryParams: params });
  }

  public static getUrlWithoutParams(router: Router): string {
    let urlTree = router.parseUrl(router.url);
    let urlWithoutParams = urlTree.root.children['primary'].segments.map(it => it.path).join('/');
    return urlWithoutParams;
  }
  protected paramMapToParams(params: ParamMap): any {
    let result: any = {};
    for (var _i = 0; _i < params.keys.length; _i++) {
      result[params.keys[_i]] = params.get(params.keys[_i]);
    }
    return result;
  }


}
