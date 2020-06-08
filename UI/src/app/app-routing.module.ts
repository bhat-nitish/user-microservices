import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddUserComponent } from './add-user/add-user.component';
import { UserListComponent } from './user-list/user-list.component';
import { AdminUserListComponent } from './admin/user-list/admin-user-list.component';

const routes: Routes = [
  { path: 'users', component: AddUserComponent },
  { path: 'users/list', component: UserListComponent },
  { path: 'admin/users/list', component: AdminUserListComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
