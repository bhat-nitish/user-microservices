import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder, FormGroupDirective } from '@angular/forms';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import {
  MAT_MOMENT_DATE_FORMATS,
  MomentDateAdapter,
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
} from '@angular/material-moment-adapter';
import { CreateUser, UserServiceResponseDto } from './dto/index';
import { debug } from 'util';
import { SuccessDialogComponent } from '../dialogs/success-dialog/success-dialog.component';
import { ErrorHandlerService, UserService } from '../services/index';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss'],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-Gb' },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
  ]
})
export class AddUserComponent implements OnInit {

  public userForm: FormGroup;
  public emailPattern = "[a-zA-Z0-9.-_]{1,}@[a-zA-Z.-]{2,}[.]{1}[a-zA-Z]{2,}";

  minDate: Date;
  maxDate: Date;

  @ViewChild(FormGroupDirective, { static: true }) formGroupDirective: FormGroupDirective;

  dialogConfig = {
    height: '300px',
    width: '400px',
    disableClose: true,
    data: {}
  }

  constructor(private router: Router, private dialog: MatDialog, private errorService: ErrorHandlerService, private userService: UserService) {

    const currentYear = new Date().getFullYear();
    this.minDate = new Date(currentYear - 100, 0, 1);
    this.maxDate = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());

    this.userForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.maxLength(20)]),
      dateOfBirth: new FormControl(new Date()),
      email: new FormControl('', [Validators.required])
    });
  }

  public onCancel = () => {
    this.userForm.reset();
  }

  public hasError = (controlName: string, errorName: string) => {
    return this.userForm.controls[controlName].hasError(errorName);
  }

  public createUser = (userFormValue) => {
    debug;
    if (this.userForm.valid) {
      let user: CreateUser = this.mapUser(userFormValue);
      this.userService.create(user)
        .subscribe(res => {

          if (res.success) {
            (this.dialogConfig.data as any).message = res.message;
            let dialogRef = this.dialog.open(SuccessDialogComponent, this.dialogConfig);
            dialogRef.afterClosed()
              .subscribe(result => {
                setTimeout(() =>
                  this.formGroupDirective.resetForm(), 0)
              });
          } else {
            (this.dialogConfig.data as any).message = res.error.message;
            this.errorService.dialogConfig = { ...this.dialogConfig };
            this.errorService.handleErrorMessage(res.error.message);
          }
        },
          (error => {
            this.errorService.dialogConfig = { ...this.dialogConfig };
            this.errorService.handleError(error);
          })
        )
    }
  }

  private mapUser = (userFormValue): CreateUser => {
    let user: CreateUser = {
      userName: userFormValue.name,
      dateOfBirth: userFormValue.dateOfBirth,
      email: userFormValue.email
    }
    return user;
  }

  ngOnInit() {
  }

}
