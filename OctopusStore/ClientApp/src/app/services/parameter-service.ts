import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import { MessageService } from "./message.service";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ParameterService {
  private params: any = {};
  private paramsSource = new Subject<any>();
  public params$ = this.paramsSource.asObservable();

  constructor(
    protected messageService: MessageService,
    protected route: ActivatedRoute) {
    this.route.queryParamMap.subscribe(
      (params: ParamMap) => {
        this.params = ParameterService.paramMapToParams(params);
      });
  }

  public getParams(): any {
    let params: any = {};
    Object.assign(params, this.params);
    return params;
  }
  public getParam(key: string): any {
    return this.params[key];
  }
  public setParam(key: string, value: any): void {
    this.params[key] = value;
    this.paramsSource.next(this.params);
  }
  public getUpdatedParams(paramName: string, param: any): any {
    let params = this.getParams();
    params[paramName] = param;
    return params;
  }
  public clearParams() {
    this.params = {};
    this.paramsSource.next(this.params);
  }

  public static getUrlWithoutParams(router: Router): string {
    let urlTree = router.parseUrl(router.url);
    let urlWithoutParams = urlTree.root.children['primary'].segments.map(it => it.path).join('/');
    return urlWithoutParams;
  }
  protected static paramMapToParams(params: ParamMap): any {
    let result: any = {};
    for (var _i = 0; _i < params.keys.length; _i++) {
      result[params.keys[_i]] = params.get(params.keys[_i]);
    }
    return result;
  }
}
