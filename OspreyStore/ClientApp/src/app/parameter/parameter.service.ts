import { ActivatedRoute, ParamMap, Router, NavigationEnd, UrlTree, NavigationStart, RoutesRecognized } from "@angular/router";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { MessageService } from "../message/message.service";
import { filter } from "rxjs/operators";
import { unescape } from "querystring";

@Injectable({
  providedIn: 'root'
})
export class ParameterService {
  protected params: any = {};
  public oldParams: any = {};
  public oldPath: string;
  private history = [];
  protected paramsSource = new Subject<any>();
  public params$ = this.paramsSource.asObservable();

  constructor(
    protected messageService: MessageService,
    protected route: ActivatedRoute,
    protected router: Router) {
    this.router.events.subscribe(val => {
      if (val instanceof NavigationStart) {
        this.oldParams = this.params;
        this.oldPath = this.getCurrentUrlWithoutParams();
      }
      if (val instanceof NavigationEnd) {
        this.params = this.paramMapToParams(this.route.snapshot.queryParamMap);
        Object.entries(this.params).forEach((pair) =>
          console.log(pair[0] + ": " + pair[1])
        );
        this.paramsSource.next(this.params);
        //let p = this.getUrlWithoutParams(this.router.parseUrl(this.getPreviousUrl()));
        //if (this.getCurrentUrlWithoutParams() !== this.getUrlWithoutParams(this.router.parseUrl(this.getPreviousUrl()))) {
        //  window.scrollTo(0, 0);
        //}
      }
      //if (val instanceof RoutesRecognized) {
      //   = val.urlAfterRedirects;
      //}
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
  
  //public getUpdatedParams(pairs: any): any {
  //  return this.assignParams(this.params, pairs);
  //}

  public isParamChanged(name: string): boolean {
    let oldExists = this.oldParams[name] !== undefined;
    let currentExists = this.params[name] !== undefined;
    return (oldExists !== currentExists) || (this.oldParams[name] != this.params[name]);
  }

  public getUpdatedParamsCopy(pairs: any): any {
    return this.assignParams(this.getParamsCopy(), pairs);
  }

  public loadRouting(): void {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(({ urlAfterRedirects }: NavigationEnd) => {
        this.history = [...this.history, urlAfterRedirects];
      });
  }

  public getHistory(): string[] {
    return this.history;
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
    let params = this.getUpdatedParamsCopy(pairs);
    this.router.navigate([this.getCurrentUrlWithoutParams()], { queryParams: params });
  }

  public navigateWithParams(params: any, url: string = null) {
    url = url ? url : this.getCurrentUrlWithoutParams();
    this.router.navigate([url], { queryParams: params });
  }

  public getCurrentUrlWithoutParams(): string {
    let urlTree = this.router.parseUrl(this.router.url);
    return "/" + this.getUrlWithoutParams(urlTree);
  }

  public getUrlWithoutParams(urlTree: UrlTree): string {
    let urlWithoutParams = this.router.url;
    let children = urlTree.root.children['primary'];
    if (children && children.segments) {
      urlWithoutParams = children.segments.map(it => it.path).join('/');
    }
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
