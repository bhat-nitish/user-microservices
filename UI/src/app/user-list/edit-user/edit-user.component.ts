import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { UserModify } from "../dto/UserModify";
import { FormBuilder, Validators, FormGroup, FormControl } from "@angular/forms";
import * as moment from 'moment';

@Component({
    selector: 'edit-user',
    templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit {

    form: FormGroup;
    userName: string;

    constructor(
        private fb: FormBuilder,
        private dialogRef: MatDialogRef<EditUserComponent>,
        @Inject(MAT_DIALOG_DATA) { userName,
        }: UserModify) {

        this.form = fb.group({
            userName: new FormControl(userName, [Validators.required, Validators.maxLength(20)]),
        });

    }

    ngOnInit() {

    }

    public hasError = (controlName: string, errorName: string) => {
        return this.form.controls[controlName].hasError(errorName);
    }


    Update() {
        this.dialogRef.close(this.form.value);
    }

    close() {
        this.dialogRef.close();
    }

}