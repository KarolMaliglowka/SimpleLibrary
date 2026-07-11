import {ChangeDetectorRef, Component, OnInit, ViewChild} from '@angular/core';
import {ConfirmationService, MessageService} from 'primeng/api';
import {Dialog} from 'primeng/dialog';
import {ToastModule} from 'primeng/toast';
import {ToolbarModule} from 'primeng/toolbar';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {InputTextModule} from 'primeng/inputtext';
import {TextareaModule} from 'primeng/textarea';
import {CommonModule} from '@angular/common';
import {SelectModule} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {InputNumber} from 'primeng/inputnumber';
import {IconFieldModule} from 'primeng/iconfield';
import {InputIconModule} from 'primeng/inputicon';
import {TableModule} from 'primeng/table';
import {Table} from 'primeng/table';
import {ButtonModule} from 'primeng/button';
import {PaginatorModule} from "primeng/paginator";
import {TooltipModule} from 'primeng/tooltip'
import {User} from '../../models/user';
import {UsersService} from '../../service/user.service';
import {NamesListPipe,} from '../../../shared/extensions/NamesListPipe';

@Component({
    selector: 'user-component',
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, InputNumber, IconFieldModule, InputIconModule,
        ButtonModule, PaginatorModule, TooltipModule, NamesListPipe
    ],
    providers: [
        MessageService, ConfirmationService, UsersService
    ],
    templateUrl: './user.component.html',
    styleUrl: './user.component.scss'
})

export class UserComponent implements OnInit {
    userDialog: boolean = false;
    users!: User[];
    user!: User;
    selectedUsers!: User[] | null;
    submitted: boolean = false;
    statuses!: any[];
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;

    constructor(
        private userService: UsersService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
    ) {
    }

    async ngOnInit() {
        await this.loadData();
    }

    openNew() {
        this.user = {};
        this.submitted = false;
        this.userDialog = true;
    }

    editUser(user: User) {
        this.user = {...user};
        this.userDialog = true;
    }

    hideDialog() {
        this.userDialog = false;
        this.submitted = false;
    }

    deleteUser(user: User) {
        this.userService.DeleteUser(user.id as string);
    }

    findIndexById(id: string): number {
        let index = -1;
        for (let i = 0; i < this.users.length; i++) {
            if (this.users[i].id === id) {
                index = i;
                break;
            }
        }
        return index;
    }

    saveUser() {
        this.submitted = true;
        if (this.user.name?.trim()) {
            if (this.user.id) {
                this.users[this.findIndexById(this.user.id)] = this.user;
                this.userService.UpdateUser(this.user);
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'User updated',
                    life: 3000
                });
            } else {
                this.users.push(this.user);
                this.userService.CreateUser(this.user);
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'User created',
                    life: 3000
                });
            }

            this.users = [...this.users];
            this.userDialog = false;
            this.user;
        }
    }

    async loadData() {
        this.loading = true;
        try {
            const data = await this.userService.GetAllUsers();
            this.users = [...data].filter(x => !x.isDelete);
            console.log(this.users);
        } catch (err) {
            console.error(err);
        } finally {
            this.loading = false;
            this.cd.detectChanges();
        }
    }

    async setNotActive(user: User){
        this.confirmationService.confirm({
            message: `Are you sure you want to set user ${user.fullName} not active?`,
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            acceptButtonProps: { severity: 'danger', label: 'Yes' },
            rejectButtonProps: { label: 'No', severity: 'secondary', variant: 'text' },
            accept: async () => {
                await this.userService.SetNotActive(user);
                this.messageInfo(`User ${user.fullName} is deactivate`, 'warn');
                await this.loadData();
            }
        });
    }

    async setActive(user: User){
        this.confirmationService.confirm({
            message: `Are you sure you want to set user ${user.fullName} active?`,
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            acceptButtonProps: { severity: 'danger', label: 'Yes' },
            rejectButtonProps: { label: 'No', severity: 'secondary', variant: 'text' },
            accept: async () => {
                await this.userService.SetActive(user);
                this.messageInfo(`User ${user.fullName} is activate`, 'success');
                await this.loadData();
            }
        });
    }

    messageInfo(message: string, kind: string) {
        this.messageService.add({ severity: kind, summary: kind.toUpperCase(), detail: message, life: 3000 });
    }

    confirm(user: User) {
        this.confirmationService.confirm({
            header: 'Are you confirm delete: ' + user.name + '?',
            message: 'Please confirm to \n\b proceed.',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                var tst = this.deleteUser(user);
                this.messageService.add({ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' });
                this.users = this.users.filter((val) => val.id !== user.id);
            },
            reject: () => {
                this.messageService.add({ severity: 'info', summary: 'Rejected', detail: 'You have rejected' });
            },
        });
    }
    protected readonly HTMLInputElement = HTMLInputElement;
}
