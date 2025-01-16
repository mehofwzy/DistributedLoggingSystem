import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogListComponent } from './log-list/log-list.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './services/auth.guard'; // Import the AuthGuard

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'loglist',
    component: LogListComponent,
    canActivate: [AuthGuard],  // Protect the loglist route with AuthGuard
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' },  // Default redirect to login page
  { path: '**', redirectTo: '/login' },  // Catch-all route for invalid paths
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
