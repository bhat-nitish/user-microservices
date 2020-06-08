import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { NotifyUserDto } from "../dto/NotifyUserDto";
import { FormBuilder, Validators, FormGroup, FormControl } from "@angular/forms";

@Component({
    selector: 'notify-user',
    templateUrl: './user-notification.component.html',
    styleUrls: ['./user-notification.component.scss']
})
export class NotifyUserComponent implements OnInit {

    form: FormGroup;
    email: string;
    notification: string;

    constructor(
        private fb: FormBuilder,
        private dialogRef: MatDialogRef<NotifyUserComponent>,
        @Inject(MAT_DIALOG_DATA) { email, notification
        }: NotifyUserDto) {

        this.email = email;
        this.notification = notification;

        this.form = fb.group({
            notification: new FormControl(notification, [Validators.required, Validators.maxLength(200)]),
            email: new FormControl(email, [])
        });

    }

    ngOnInit() {

    }

    public hasError = (controlName: string, errorName: string) => {
        return this.form.controls[controlName].hasError(errorName);
    }


    notify() {
        this.dialogRef.close(this.form.value);
    }

    close() {
        this.dialogRef.close();
    }

}