import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppHeaderComponent } from './app-header/app-header.component';
import { LeftMenuComponent } from './left-menu/left-menu.component';
import { MaterialModule } from './material.module'
import { SidenavService } from './sidenav.service';
import { AddUserComponent } from './add-user/add-user.component';
import { UserListComponent } from './user-list/user-list.component';
import { SuccessDialogComponent } from './dialogs/success-dialog/success-dialog.component';
import { ErrorDialogComponent } from './dialogs/error-dialog/error-dialog.component';
import { UserServices } from './services/services.module';
import { HttpClientModule } from '@angular/common/http';
import { EditUserComponent } from './user-list/edit-user/edit-user.component';
import { AdminUserListComponent } from './admin/user-list/admin-user-list.component';
import { NotifyUserComponent } from './admin/user-notification/user-notification.component';

@NgModule({
  declarations: [
    AppComponent,
    AppHeaderComponent,
    LeftMenuComponent,
    AddUserComponent,
    UserListComponent,
    SuccessDialogComponent,
    ErrorDialogComponent,
    AdminUserListComponent,
    EditUserComponent,
    NotifyUserComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MaterialModule,
    HttpClientModule
  ],
  providers: [SidenavService, UserServices],
  entryComponents: [
    SuccessDialogComponent,
    ErrorDialogComponent,
    EditUserComponent,
    NotifyUserComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
