import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ErrorDialogComponent } from '../dialogs/error-dialog/error-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Injectable({
    providedIn: 'root'
})
export class ErrorHandlerService {
    public errorMessage: string = '';
    dialogConfig = {
        disableClose: true,
        data: {}
    }

    constructor(private router: Router, private dialog: MatDialog) { }

    public handleError = (error: HttpErrorResponse) => {
        this.displayError(error);
    }

    public handleErrorMessage = (error: string) => {
        this.displayErrorMessage(error);
    }

    private displayError = (error: HttpErrorResponse) => {
        this.createErrorMessage(error);
        this.dialogConfig.data = { 'errorMessage': "An Error Occured. Please check your network." };
        this.dialog.open(ErrorDialogComponent, this.dialogConfig);
    }

    private displayErrorMessage = (error: string) => {
        this.dialogConfig.data = { 'errorMessage': error };
        this.dialog.open(ErrorDialogComponent, this.dialogConfig);
    }

    private createErrorMessage(error: HttpErrorResponse) {
        this.errorMessage = error.error ? error.error : error.statusText;
    }
}