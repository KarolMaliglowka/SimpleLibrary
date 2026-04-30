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
import {Book} from '../../models/book';
import {BooksService} from '../../service/book.service';
import {NamesListPipe} from '../../../shared/extensions/NamesListPipe';
import {PublishersService} from '../../service/publisher.service';
import {CategoriesService} from '../../service/category.service';

@Component({
    selector: 'book-component',
    templateUrl: 'book.component.html',
    styleUrl: 'book.component.scss',
    standalone: true,
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, InputNumber, IconFieldModule, InputIconModule,
        ButtonModule, PaginatorModule, TooltipModule, NamesListPipe
    ],
    providers: [
        MessageService, ConfirmationService, BooksService, PublishersService, CategoriesService
    ]
})
export class BookComponent implements OnInit {
    bookDialog: boolean = false;
    books!: Book[];
    book!: Book;
    selectedBooks!: Book[] | null;
    submitted: boolean = false;
    statuses!: any[];
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;

    countries: any[] | undefined;
    selectedCountry: string | undefined;

    publishers: any[] | undefined;
    selectedPublisher: string | undefined;

    categories: any[] | undefined;
    selectedCategory: string | undefined;

    constructor(
        private bookService: BooksService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
        private publisherService: PublishersService,
        private categoriesService: CategoriesService
    ) {
    }

    ngOnInit() {
        this.loadData();
        console.log(this.selectedPublisher);
    }

    loadData() {
        this.loading = true;
        this.bookService.GetAllBooks()
            .then((data: any) => {
                console.log(data);
                this.books = data;
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
        this.countries = [
            { name: 'Australia', code: 'AU' },
            { name: 'Brazil', code: 'BR' },
            { name: 'China', code: 'CN' },
            { name: 'Egypt', code: 'EG' },
            { name: 'France', code: 'FR' },
            { name: 'Germany', code: 'DE' },
            { name: 'India', code: 'IN' },
            { name: 'Japan', code: 'JP' },
            { name: 'Spain', code: 'ES' },
            { name: 'United States', code: 'US' }
        ];

        this.publisherService.GetAllPublishersDictionary()
            .then((data: any) => {
                this.publishers = data;
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });

        this.categoriesService.GetAllCategoriesDictionary()
            .then((data: any) => {
                this.categories = data;
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
    }

    openNew() {
        this.selectedPublisher = undefined;
        this.book = {};
        this.submitted = false;
        this.bookDialog = true;
    }

    toolt(book: Book){
        return book.description;
    }

    editBook(book: Book) {
        console.log(book);
        this.book = {...book};

        this.selectedCategory = book.category;
        console.log(this.selectedCategory);
        this.selectedPublisher = book.publisher as string;
        console.log(this.selectedPublisher);

        this.bookDialog = true;
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
                //przesłac do servisu http i wykasować

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

    saveBook() {
        this.submitted = true;

        console.log(this.book);
        this.book.publisher = this.selectedPublisher;

        if (this.book.name?.trim()) {
            if (this.book.id) {
                this.books[this.findIndexById(this.book.id)] = this.book;
                this.bookService.UpdateBook(this.book);
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Book Updated',
                    life: 3000
                });
            } else {
                this.books.push(this.book);
                this.bookService.CreateBook(this.book);
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

    protected readonly HTMLInputElement = HTMLInputElement;
}
