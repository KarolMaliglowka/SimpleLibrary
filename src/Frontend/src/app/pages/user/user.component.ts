import {ChangeDetectorRef, Component, OnInit, ViewChild} from '@angular/core';
import {ConfirmationService, MessageService} from 'primeng/api';
import {Dialog} from 'primeng/dialog';
import {ToastModule} from 'primeng/toast';
import {ToolbarModule} from 'primeng/toolbar';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {InputTextModule} from 'primeng/inputtext';
import {TextareaModule} from 'primeng/textarea';
import {CommonModule} from '@angular/common';
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
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {SelectModule} from "primeng/select";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
    selector: 'user-component',
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, InputNumber, IconFieldModule, InputIconModule,
        ReactiveFormsModule, ButtonModule, PaginatorModule, TooltipModule, NamesListPipe
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
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;
    editMode = false;
    userForm!: FormGroup;
    editingUser?: User;

    constructor(
        private userService: UsersService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
        private fb: FormBuilder
    ) {
    }

    async ngOnInit() {
        await this.loadData();
        this.userForm = this.fb.group({
            id: [null],
            name: ['', Validators.required],
            surname: ['', Validators.required],
            email: ['', Validators.required],
            address: ['', Validators.required],
            phoneNumber: ['', Validators.required],
            city: ['', Validators.required],
            country: ['', Validators.required],
            postalCode: ['', Validators.required]
        });
    }

    openNew() {
        this.editingUser = undefined;
        this.userForm.reset();
        this.userDialog = true;
        this.editMode = false;
    }

    editUser(user: User) {
        console.log(user);
        this.editingUser = user;
        this.userForm.patchValue(user);
        this.userDialog = true;
        this.editMode = true;
    }

    deleteUser(user: User) {
        this.userService.DeleteUser(user.id as string);
    }

    async saveUser() {
        if (this.userForm.invalid) return;
        const newUser: User = {
            ...this.userForm.value,
            isActive: this.user?.isActive ?? true
        };

        try {
            if (this.editMode) {
                await this.userService.UpdateUser(newUser);
                this.messageInfo('Updated user', 'success');
            } else {
                await this.userService.CreateUser(newUser);
                this.messageInfo('Created new user', 'success');
            }
            this.userDialog = false;
        } catch (err) {
            if (err instanceof HttpErrorResponse) {
                this.messageInfo(err.error.message, 'error');
            } else {
                this.messageInfo('Unexpected error', 'error');
            }
        }
        await this.loadData();
    }

    async loadData() {
        this.loading = true;
        try {
            const data = await this.userService.GetAllUsers();
            this.users = [...data].filter(x => !x.isDeleted);
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
