import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Book } from './book';
import { BooksService } from '../service/book.service';
import { TableModule } from 'primeng/table';
import { Dialog } from 'primeng/dialog';
import { Ripple } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { CommonModule } from '@angular/common';
import { FileUpload } from 'primeng/fileupload';
import { SelectModule } from 'primeng/select';
import { Tag } from 'primeng/tag';
import { RadioButton } from 'primeng/radiobutton';
import { Rating } from 'primeng/rating';
import { FormsModule } from '@angular/forms';
import { InputNumber } from 'primeng/inputnumber';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { Table } from 'primeng/table';

interface Column {
    field: string;
    header: string;
    customExportHeader?: string;
}

interface ExportColumn {
    title: string;
    dataKey: string;
}

@Component({
    selector: 'book-component',
    templateUrl: 'book.component.html',
    standalone: true,
    imports: [TableModule, Dialog, Ripple, SelectModule, ToastModule, ToolbarModule, ConfirmDialog, InputTextModule, TextareaModule, CommonModule, FileUpload, Tag, RadioButton, Rating, InputTextModule, FormsModule, InputNumber, IconFieldModule, InputIconModule],
    providers: [MessageService, ConfirmationService, BooksService],
    styles: [
        `:host ::ng-deep .p-dialog .product-image {
            width: 150px;
            margin: 0 auto 2rem auto;
            display: block;
        }`
    ]
})
export class TableBook implements OnInit{
    bookDialog: boolean = false;

    books!: Book[];

    book!: Book;

    selectedBooks!: Book[] | null;

    submitted: boolean = false;

    statuses!: any[];

    @ViewChild('dt') dt!: Table;

    cols!: Column[];

    exportColumns!: ExportColumn[];

    constructor(
        private bookService: BooksService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef
    ) {}

    exportCSV() {
        this.dt.exportCSV();
    }

    ngOnInit() {
        this.loadDemoData();
    }

    loadDemoData() {
        this.bookService.GetAllBooks().then((data) => {
            this.books = data;
            this.cd.markForCheck();
        });
    }

    openNew() {
        this.book;
        this.submitted = false;
        this.bookDialog = true;
    }

    editBook(book: Book) {
        this.book = { ...book };
        this.bookDialog = true;
    }

    deleteSelectedBooks() {
        this.confirmationService.confirm({
            message: 'Are you sure you want to delete the selected books?',
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
                this.books = this.books.filter((val) => !this.selectedBooks?.includes(val));
                this.selectedBooks = null;
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Books Deleted',
                    life: 3000
                });
            }
        });
    }

    hideDialog() {
        this.bookDialog = false;
        this.submitted = false;
    }

    deleteBook(book: Book) {
        this.confirmationService.confirm({
            message: 'Are you sure you want to delete ' + book.name + '?',
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
                this.books = this.books.filter((val) => val.id !== book.id);
                this.book;
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Book Deleted',
                    life: 3000
                });
            }
        });
    }

    findIndexById(id: string): number {
        let index = -1;
        for (let i = 0; i < this.books.length; i++) {
            if (this.books[i].id === id) {
                index = i;
                break;
            }
        }

        return index;
    }

    createId(): string {
        let id = '';
        var chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        for (var i = 0; i < 5; i++) {
            id += chars.charAt(Math.floor(Math.random() * chars.length));
        }
        return id;
    }

    saveBook() {
        this.submitted = true;

        if (this.book.name?.trim()) {
            if (this.book.id) {
                this.books[this.findIndexById(this.book.id)] = this.book;
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Book Updated',
                    life: 3000
                });
            } else {
                this.book.id = this.createId();

                this.books.push(this.book);
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Book Created',
                    life: 3000
                });
            }

            this.books = [...this.books];
            this.bookDialog = false;
            this.book;
        }
    }
}
