import { AdminService } from '../../services/index';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { AdminUserView } from '../Dto/AdminUserView';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator'
import { ErrorHandlerService } from '../../services/index';
import { Router } from '@angular/router';
import { debug } from 'util';
import { MatDialog } from '@angular/material/dialog';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogConfig } from "@angular/material";
import { NotifyUserComponent } from '../user-notification/user-notification.component';
import { NotifyUserDto } from '../Dto/NotifyUserDto';
import { SuccessDialogComponent } from '../../dialogs/success-dialog/success-dialog.component';

@Component({
    selector: 'admin-user-list',
    templateUrl: './admin-user-list.component.html',
    styleUrls: ['./admin-user-list.component.scss']
})
export class AdminUserListComponent implements OnInit, AfterViewInit {

    public displayedColumns = ['name', 'dob', "email", "update"];
    public dataSource = new MatTableDataSource<AdminUserView>();
    public preferredFormat = "dd/MM/yyyy";
    ready: boolean = true;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

    dialogConfig = {
        height: '300px',
        width: '400px',
        disableClose: true,
        data: {}
    }

    constructor(private dialog: MatDialog, private userService: AdminService, private errorService: ErrorHandlerService, private router: Router) { }

    ngOnInit() {
        this.getUsers();
    }

    public getUsers = () => {
        this.ready = false;
        this.userService.getAll()
            .subscribe(res => {
                this.dataSource.data = res as AdminUserView[];
                this.ready = true;

            },
                (error) => {
                    this.errorService.handleError(error);
                    this.ready = true;
                })
    }

    ngAfterViewInit(): void {
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
    }

    public customSort = (event) => {
        console.log(event);
    }

    public doFilter = (value: string) => {
        this.dataSource.filter = value.trim().toLocaleLowerCase();
    }

    public notify = (id: number) => {
        this.openNotificationDialog(id);
    }

    openNotificationDialog(id: number) {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;

        dialogConfig.width = "500px";
        dialogConfig.height = "300px";
        dialogConfig.data = {};

        dialogConfig.data = {
            email: (this.dataSource.data.filter(u => u.id == id)[0]['email'])
        };

        const dialogRef = this.dialog.open(NotifyUserComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(result => {
            let notification: NotifyUserDto = { email: result.email, notification: result.notification };
            this.userService.notify(notification)
                .subscribe(res => {

                    if (res.success) {
                        (this.dialogConfig.data as any).message = res.message;
                        let dialogRef = this.dialog.open(SuccessDialogComponent, this.dialogConfig);
                        dialogRef.afterClosed()
                            .subscribe(result => {
                                setTimeout(() =>
                                    this.getUsers(), 0)
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

        });
    }
}