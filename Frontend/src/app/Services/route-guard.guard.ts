import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { GlobalStoreService } from './global-store.service';

@Injectable({
  providedIn: 'root',
})
export class RouteGuardGuard implements CanActivate {
  constructor(private store: GlobalStoreService, private router: Router) {}
  canActivate() {
    // console.log('in guard');
    const user: any = this.store.getUser();
    // console.log(user);
    if (user.username != 'null' && user.username != null) return true;
    this.router.navigate(['/login']);
    return false;
  }
}
