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
import {InputNumber} from 'primeng/inputnumber';
import {IconFieldModule} from 'primeng/iconfield';
import {InputIconModule} from 'primeng/inputicon';
import {TableModule} from 'primeng/table';
import {Table} from 'primeng/table';
import {ButtonModule} from 'primeng/button';
import {PaginatorModule} from "primeng/paginator";
import {TooltipModule} from 'primeng/tooltip'
import {Book} from '../../models/book';
import {Author} from '../../models/author';
import {BooksService} from '../../service/book.service';
import {NamesListPipe} from '../../../shared/extensions/NamesListPipe';
import {PublishersService} from '../../service/publisher.service';
import {CategoriesService} from '../../service/category.service';
import {AuthorsService} from '../../service/author.service';
import { MultiSelectModule } from 'primeng/multiselect';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {HttpErrorResponse} from '@angular/common/http';

@Component({
    selector: 'book-component',
    templateUrl: 'book.component.html',
    styleUrl: 'book.component.scss',
    standalone: true,
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, InputNumber, IconFieldModule, InputIconModule,
        ButtonModule, PaginatorModule, TooltipModule, NamesListPipe,ReactiveFormsModule,MultiSelectModule


    ],
    providers: [
        MessageService,
        ConfirmationService,
        BooksService,
        PublishersService,
        CategoriesService,
        AuthorsService
    ]
})
export class BookComponent implements OnInit {
    bookDialog: boolean = false;
    books: Book[] = [];
    book!: Book;
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;

    publishers: any[] | undefined;
    publisher: string | undefined;

    categories: any[] | undefined;
    category: string | undefined;

    authors!: Author[] | undefined;
    author: Author[] | undefined;

    editMode = false;
    bookForm!: FormGroup;
    editingBook?: Book;


    constructor(
        private bookService: BooksService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
        private publisherService: PublishersService,
        private categoriesService: CategoriesService,
        private authorsService: AuthorsService,
        private fb: FormBuilder
    ) {
    }

     ngOnInit() {
         this.loadData();
        console.log(this.publisher);
        this.bookForm = this.fb.group({
            name: ['', Validators.required],
            description: [''],

            category: [null, Validators.required],
            publisher: [null, Validators.required],
            authors: [[]],

            pagesCount: [0],
            yearOfRelease: [''],
            isbn: ['']
        });
    }

     loadData() {
        this.loading = true;
        this.bookService.GetAllBooks()
            .then((data: any) => {
                this.books = data;
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });

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

        this.authorsService.GetAllAuthorsDictionary()
            .then((data: any) => {
                this.authors = data;
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
    }

    openNew() {
        this.editingBook = undefined;
        this.bookForm.reset();
        this.bookDialog = true;
        this.editMode = false;
    }

    toolt(book: Book){
        return book.description;
    }

    editBook(book: Book) {
        console.log(book);
        this.book = {...book};

        this.category = book.category;
        console.log(book.category);
        this.publisher = book.publisher as string;
        console.log(this.publisher);
        //this.selectedAuthors = book.authors?.filter(x => x.name);
        //console.log(this.selectedAuthors);
        this.bookDialog = true;
    }

    hideDialog() {
        this.bookDialog = false;
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

        // console.log("przy zapisie", this.book);
        //
        // this.book.publisher = this.selectedPublisher;
        // this.book.category = this.selectedCategory;
        // this.book.authors = this.selectedAuthors;
        // console.log(JSON.stringify(this.book, null, 2));
        // if (this.book.name?.trim()) {
        //     if (this.book.id) {
        //         this.books[this.findIndexById(this.book.id)] = this.book;
        //         this.bookService.UpdateBook(this.book);
        //         this.messageService.add({
        //             severity: 'success',
        //             summary: 'Successful',
        //             detail: 'Book Updated',
        //             life: 3000
        //         });
        //     } else {
        //         this.books.push(this.book);
        //         this.bookService.CreateBook(this.book);
        //         this.messageService.add({
        //             severity: 'success',
        //             summary: 'Successful',
        //             detail: 'Book Created',
        //             life: 3000
        //         });
        //     }
        //
        //     this.books = [...this.books];
        //     this.bookDialog = false;
        //     this.book;
        // }
        if (this.bookForm.invalid) return;
        const newBook: Book = this.bookForm.value;
         console.log('newbook: ', newBook);
        try {
            if (this.editMode) {
                this.bookService.UpdateBook(newBook);
                this.messageInfo('Updated book', 'success');
            } else {
                this.bookService.CreateBook(newBook);
                this.messageInfo('Created new book', 'success');
            }
            this.bookDialog = false;
        } catch (err) {
            if (err instanceof HttpErrorResponse) {
                this.messageInfo(err.error.message, 'error');
            } else {
                this.messageInfo('Unexpected error', 'error');
            }
        }
        this.loadData();
    }

    messageInfo(message: string, kind: string) {
        this.messageService.add({severity: kind, summary: kind.toUpperCase(), detail: message, life: 3000});
    }

    protected readonly HTMLInputElement = HTMLInputElement;
}
