import {ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation} from '@angular/core';
import {ConfirmationService, MessageService} from 'primeng/api';
import {Dialog} from 'primeng/dialog';
import {ToastModule} from 'primeng/toast';
import {ToolbarModule} from 'primeng/toolbar';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {InputTextModule} from 'primeng/inputtext';
import {TextareaModule} from 'primeng/textarea';
import {CommonModule} from '@angular/common';
import {SelectModule} from 'primeng/select';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {InputNumber} from 'primeng/inputnumber';
import {IconFieldModule} from 'primeng/iconfield';
import {InputIconModule} from 'primeng/inputicon';
import {TableModule} from 'primeng/table';
import {Table} from 'primeng/table';
import {ButtonModule} from 'primeng/button';
import {PaginatorModule} from "primeng/paginator";
import {TooltipModule} from 'primeng/tooltip'
import {Author} from '../../models/author';
import {AuthorsService} from '../../service/author.service';
import {HttpErrorResponse} from '@angular/common/http';

@Component({
    selector: 'app-author',
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, InputNumber, IconFieldModule, InputIconModule,
        ReactiveFormsModule, ButtonModule, PaginatorModule, TooltipModule
    ],
    providers: [
        MessageService, ConfirmationService, AuthorsService
    ],
    templateUrl: './author.component.html',
    styleUrl: './author.component.scss'
})
export class AuthorComponent {
    authorDialog: boolean = false;
    authors!: Author[];
    author!: Author;
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;
    editMode = false;
    authorForm!: FormGroup;
    editingAuthor?: Author;

    constructor(
        private authorService: AuthorsService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
        private fb: FormBuilder
    ) {
    }

    async ngOnInit() {
        await this.loadData();
        this.authorForm = this.fb.group({
            id: [null],
            name: [
                '',
                [
                    Validators.required,
                    Validators.minLength(3),
                    Validators.maxLength(50)
                ]
            ],
            surname: [
                '',
                [
                    Validators.maxLength(50)
                ]
            ]
        });
    }

    async loadData() {
        this.loading = true;
        this.authorService.GetAllAuthors()
            .then((data) => {
                this.authors = data.filter(x => x.isDelete);
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
    }

    openNew() {
        this.editingAuthor = undefined;
        this.authorForm.reset();
        this.authorDialog = true;
        this.editMode = false;
    }

    editAuthor(author: Author) {
        this.editingAuthor = author;
        this.authorForm.patchValue(author);
        this.authorDialog = true;
        this.editMode = true;
    }

    deleteAuthor(author: Author) {
        this.authorService.DeleteAuthor(author.id as string);
    }

    findIndexById(id: string): number {
        let index = -1;
        for (let i = 0; i < this.authors.length; i++) {
            if (this.authors[i].id === id) {
                index = i;
                break;
            }
        }
        return index;
    }

    async saveAuthor() {
        if (this.authorForm.invalid) return;
        const newAuthor: Author = this.authorForm.value;
        try {
            if (this.editMode) {
                await this.authorService.UpdateAuthor(newAuthor);
                this.messageInfo('Updated author', 'success');
            } else {
                await this.authorService.CreateAuthor(newAuthor);
                this.messageInfo('Created new author', 'success');
            }
            this.authorDialog = false;
        } catch (err) {
            if (err instanceof HttpErrorResponse) {
                this.messageInfo(err.error.message, 'error');
            } else {
                this.messageInfo('Unexpected error', 'error');
            }
        }
        await this.loadData();
    }

    messageInfo(message: string, kind: string) {
        this.messageService.add({severity: kind, summary: kind.toUpperCase(), detail: message, life: 3000});
    }

    confirm(author: Author) {
        this.confirmationService.confirm({
            header: 'Are you confirm delete: ' + author.name + '?',
            message: 'Please confirm to \n\b proceed.',
            icon: 'pi pi-exclamation-triangle',
            accept: async () => {
                try {
                    await this.authorService.DeleteAuthor(author.id!);
                    this.authors = this.authors.filter(x => x.id !== author.id);
                    this.messageService.add({
                        severity: 'success',
                        summary: 'Sukces',
                        detail: 'Author deleted.'
                    });

                } catch (err: any) {
                    this.messageService.add({
                        severity: 'error',
                        summary: 'Error',
                        detail: err?.error?.message ?? 'Some unexpected error.'
                    });
                }
            },
        });
    }

    protected readonly HTMLInputElement = HTMLInputElement;
}
