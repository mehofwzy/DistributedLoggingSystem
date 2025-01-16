import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';  // FormsModule

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule], // FormsModule 

})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  login() {
    this.authService.login(this.username, this.password).subscribe(
      (response) => {
        // Save token after login
        this.authService.saveToken(response.token);

        // Get returnUrl from query parameter or default to '/'
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

        // Navigate to the original requested URL or home page
        this.router.navigate(['/loglist']);
      },
      (error) => {
        this.errorMessage = 'Invalid credentials. Please try again.';
      }
    );
  }
}
