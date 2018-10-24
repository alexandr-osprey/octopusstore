import { ActivatedRoute, ParamMap, Router, NavigationEnd } from "@angular/router";
import { MessageService } from "./message.service";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

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

  public getParams(): any {
    let params: any = {};
    Object.assign(params, this.params);
    return params;
  }
  public getParam(key: string): any {
    return this.params[key];
  }
  public getUpdatedParams(...args: [string, any][]): [string, any][] {
    let params = this.getParams();
    args.forEach(pair => params[pair[0]] = pair[1]);
    return params;
  }
  // navigate to the same URL, but with updated parameters
  public navigateWithUpdatedParam(...args: [string, any][]): void {
    let params = this.getUpdatedParams(...args);
    this.router.navigate([ParameterService.getUrlWithoutParams(this.router)], { queryParams: params });
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
