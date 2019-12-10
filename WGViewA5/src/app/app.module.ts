import { FormsModule, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { appRoutes } from './routes';
import { AuthModule } from './auth/auth.module';
import { AuthGuard } from './_guards/auth.guard';
import { AuthService } from './_services/auth.service';
import { TenantService } from './_services/tenant.service';
import { UserService } from './_services/user.service';
import { SuggestionService } from './_services/suggestion.service';
import { ReplyService } from './_services/reply.service';
import { httpLoaderFactory } from './_services/http-loader-factory.service';
import { AppComponent } from './app.component';
import { MaterialModule } from './material/material.component';
import { DialogDelComponent } from './material/dialog-del/dialog-del.component';
import { DialogCreateComponent } from './material/dialog-create/dialog-create.component';
import { DialogStatusComponent } from './material/dialog-status/dialog-status.component';
import { DialogModerationComponent } from './material/dialog-moderation/dialog-moderation.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { SuggestionCreateComponent } from './suggestion/suggestion-create/suggestion-create.component';
import { SuggestionListComponent } from './suggestion/suggestion-list/suggestion-list.component';
import { SuggestionDetailComponent } from './suggestion/suggestion-detail/suggestion-detail.component';
import { FooterComponent } from './footer/footer.component';
import { ModerationComponent } from './moderation/moderation.component';
import { AboutComponent } from './about/about.component';
import { RegisterComponent } from './user/register/register.component';
import { ChangePasswordComponent } from './user/change-password/change-password.component';
import { ResetPasswordComponent } from './user/reset-password/reset-password.component';
import { MatPaginatorIntl } from '@angular/material';
import { PaginatorIntlService } from './material/paginator-intl';
import { TranslateService } from '@ngx-translate/core';
import { StatusService } from './_services/status.service';
import { CaptchaComponent } from './user/captcha/captcha.component';
import { RecaptchaModule } from 'ng-recaptcha';
import { RecaptchaFormsModule } from 'ng-recaptcha/forms';
import { AccountValidationComponent } from './user/account-validation/account-validation.component';
import { DialogChangePasswordComponent } from './material/dialog-change-password/dialog-change-password.component';
import { DialogLogoutComponent } from './material/dialog-logout/dialog-logout.component';
import { registerLocaleData } from '@angular/common';
import localeEs from '@angular/common/locales/es';
import localeEsExtra from '@angular/common/locales/extra/es';
import 'hammerjs';

registerLocaleData(localeEs, localeEsExtra);

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    SuggestionCreateComponent,
    SuggestionListComponent,
    SuggestionDetailComponent,
    FooterComponent,
    DialogDelComponent,
    DialogCreateComponent,
    AboutComponent,
    ModerationComponent,
    DialogStatusComponent,
    DialogModerationComponent,
    RegisterComponent,
    ChangePasswordComponent,
    ResetPasswordComponent,
    CaptchaComponent,
    AccountValidationComponent,
    DialogChangePasswordComponent,
    DialogLogoutComponent
],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule,
    BrowserAnimationsModule,
    MaterialModule,
    RouterModule.forRoot(appRoutes, { useHash: true }),
    AuthModule,
    RecaptchaFormsModule,
    RecaptchaModule.forRoot(),
    ReactiveFormsModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpLoaderFactory,
        deps: [HttpClient]
      }
    }),
  ],
  entryComponents: [
    DialogDelComponent,
    DialogCreateComponent,
    DialogStatusComponent,
    DialogModerationComponent,
    DialogChangePasswordComponent,
    DialogLogoutComponent
  ],
  providers: [
    AuthService,
    SuggestionService,
    AuthGuard,
    ReplyService,
    UserService,
    TenantService,
    StatusService,
    {
      provide: MatPaginatorIntl,
      useFactory: (translate) => {
        const service = new PaginatorIntlService();
        service.injectTranslateService(translate);
        return service;
      },
      deps: [TranslateService]
    },
    { provide: LOCALE_ID, useValue: 'es' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
