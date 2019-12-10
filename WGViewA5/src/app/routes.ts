import { Routes} from '@angular/router'
import { AuthGuard } from './_guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { SuggestionListComponent } from './suggestion/suggestion-list/suggestion-list.component';
import { SuggestionCreateComponent } from './suggestion/suggestion-create/suggestion-create.component';
import { SuggestionDetailComponent } from './suggestion/suggestion-detail/suggestion-detail.component';
import { AboutComponent } from './about/about.component';
import { ModerationComponent } from './moderation/moderation.component';
import { RegisterComponent } from './user/register/register.component';
import { ChangePasswordComponent } from './user/change-password/change-password.component';
import { ResetPasswordComponent } from './user/reset-password/reset-password.component';
import { AccountValidationComponent } from './user/account-validation/account-validation.component';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'suggestion-list', pathMatch: 'full' },
  { path: 'suggestion-list', component: SuggestionListComponent },
  { path: 'suggestion-detail/:id', component: SuggestionDetailComponent },
  { path: 'home', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'account-validation', component: AccountValidationComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'suggestion-create', component: SuggestionCreateComponent },
      { path: 'change-password', component: ChangePasswordComponent },
      { path: 'moderation', component: ModerationComponent },
      { path: 'about', component: AboutComponent }
    ]
  },
  { path: '**', redirectTo: 'suggestion-list', pathMatch:'full' }
]
