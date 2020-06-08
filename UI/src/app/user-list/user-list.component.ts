import { UserService } from '../services/users.service';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { UserView } from './dto/UserView';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator'
import { ErrorHandlerService } from '../services/index';
import { Router } from '@angular/router';
import { debug } from 'util';
import { MatDialog } from '@angular/material/dialog';
import { SuccessDialogComponent } from '../dialogs/success-dialog/success-dialog.component';
import { SatPopoverModule } from '@ncstate/sat-popover';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogConfig } from "@angular/material";
import { EditUserComponent } from "./edit-user/edit-user.component";
import { UserModify } from './dto';
import { MatSnackBar } from '@angular/material';
import { NotifyUserDto } from '../admin/Dto/NotifyUserDto';

@Component({
    selector: 'user-list',
    templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit, AfterViewInit {

    private processingMessage = false;
    private messageNumber = 0;
    private messageQueue: string[] = [];

    public displayedColumns = ['name', 'age', "email", "update", "delete", "notification"];
    public dataSource = new MatTableDataSource<UserView>();
    ready: boolean = true;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

    dialogConfig = {
        height: '300px',
        width: '400px',
        disableClose: true,
        data: {}
    }

    constructor(private snackBar: MatSnackBar, private dialog: MatDialog, private userService: UserService, private errorService: ErrorHandlerService, private router: Router) { }

    ngOnInit() {
        this.getUsers();
    }

    private getNextMessage(): string | undefined {
        return this.messageQueue.length ? this.messageQueue.shift() : undefined;
    }

    addMessage(): void {
        this.messageQueue.push(`Message #${++this.messageNumber}`);
        if (!this.processingMessage) {
            this.displaySnackbar();
        }
    }

    private displaySnackbar(): void {
        const nextMessage = this.getNextMessage();

        if (!nextMessage) {
            this.processingMessage = false;
            return;
        }

        this.processingMessage = true;

        this.snackBar.open(nextMessage, undefined, { duration: 1000, verticalPosition: 'top' })
            .afterDismissed()
            .subscribe(() => {
                this.displaySnackbar();
            });
    }

    showNotifications = (notifications: Array<NotifyUserDto>) => {
        this.messageQueue = [];
        this.messageNumber = 0;
        debugger;
        if (notifications.length > 0) {
            this.messageQueue.push(...notifications.map(n => n.notification));
            this.displaySnackbar();
        }
    }

    public getUsers = () => {
        this.ready = false;
        this.userService.getAll()
            .subscribe(res => {
                this.dataSource.data = res as UserView[];
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

    public redirectToUpdate = (id: number) => {
        this.openEditDialog(id);
    }

    openEditDialog(id: number) {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;

        dialogConfig.width = "400px";
        dialogConfig.height = "300px";
        dialogConfig.data = {};

        dialogConfig.data = {
            userName: (this.dataSource.data.filter(u => u.id == id)[0]['userName'])
        };

        const dialogRef = this.dialog.open(EditUserComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(result => {
            let user: UserModify = { userName: result.userName };
            this.userService.update(id, user)
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

    public redirectToDelete = (id: number) => {
        this.userService.delete(id)
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
    }

}