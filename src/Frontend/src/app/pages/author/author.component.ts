import {ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation} from '@angular/core';
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
import {Author} from '../../models/author';
import {AuthorsService} from '../../service/author.service';
import {Dialog} from "primeng/dialog";
import {ToastModule} from "primeng/toast";
import {ToolbarModule} from "primeng/toolbar";
import {ConfirmDialog} from "primeng/confirmdialog";
import {InputTextModule} from "primeng/inputtext";
import {TextareaModule} from "primeng/textarea";
import {CommonModule} from "@angular/common";
import {ConfirmationService, MessageService} from "primeng/api";

@Component({
  selector: 'app-author',
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, InputNumber, IconFieldModule, InputIconModule,
        ButtonModule, PaginatorModule, TooltipModule
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
    selectedAuthors!: Author[] | null;
    submitted: boolean = false;
    statuses!: any[];
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;

    constructor(
        private authorService: AuthorsService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
    ) {}

    ngOnInit() {
        this.loadData();
    }

    loadData() {
        this.loading = true;
        this.authorService.GetAllAuthors()
            .then((data: any) => {
                console.log('authors: ', data);
                this.authors = data;
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
    }

    openNew() {
        this.author = {};
        this.submitted = false;
        this.authorDialog = true;
    }

    editAuthor(author: Author) {
        this.author = {...author};
        this.authorDialog = true;
    }

    hideDialog() {
        this.authorDialog = false;
        this.submitted = false;
    }

    deleteAuthor(author: Author) {
        this.confirmationService.confirm({
            message: 'Are you sure you want to delete ' + author.name + '?',
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            rejectButtonProps: {
                label: 'No',
                severity: 'secondary',
                variant: 'text'
            },
            acceptButtonProps: {
                severity: 'danger',
                label: 'Yes'
            },
            accept: () => {
                this.authors = this.authors.filter((val) => val.id !== author.id);
                //przesłac do servisu http i wykasować

                this.author;
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Author Deleted',
                    life: 3000
                });
            }
        });
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

    saveAuthor() {
        this.submitted = true;
        if (this.author.name?.trim()) {
            if (this.author.id) {
                this.authors[this.findIndexById(this.author.id)] = this.author;
                this.authorService.UpdateAuthor(this.author);
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Author Updated',
                    life: 3000
                });
            } else {
                this.authors.push(this.author);
                this.authorService.CreateAuthor(this.author);
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Author Created',
                    life: 3000
                });
            }
            this.authors = [...this.authors];
            this.authorDialog = false;
            this.author;
        }
    }
}
