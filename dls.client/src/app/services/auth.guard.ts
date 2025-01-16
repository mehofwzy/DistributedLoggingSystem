import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from './auth.service';  // Import AuthService

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  // This method will be called to check if the route can be activated
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // Check if the user is authenticated
    if (this.authService.isAuthenticated()) {
      return true; // If authenticated, allow the route
    }

    // If not authenticated, redirect to the login page
    this.router.navigate(['/login']);
    return false; // Prevent route activation
  }
}
