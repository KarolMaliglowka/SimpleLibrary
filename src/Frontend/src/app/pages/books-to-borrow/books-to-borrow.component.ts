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
//import {InputNumber} from 'primeng/inputnumber';
import {IconFieldModule} from 'primeng/iconfield';
import {InputIconModule} from 'primeng/inputicon';
import {TableModule} from 'primeng/table';
import {Table} from 'primeng/table';
import {ButtonModule} from 'primeng/button';
import {PaginatorModule} from "primeng/paginator";
import {TooltipModule} from 'primeng/tooltip'
import {ToggleSwitch} from 'primeng/toggleswitch';
import {Book} from '../../models/book';
import {BooksService} from '../../service/book.service';
import {NamesListPipe} from '../../../shared/extensions/NamesListPipe';
import {Borrow, CreateBorrowRequest} from '../../models/borrow';
import {UsersService} from "../../service/user.service";
import {BorrowsService} from "../../service/borrow.service";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {HttpErrorResponse} from "@angular/common/http";

@Component({
    selector: 'app-books-to-borrow',
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, IconFieldModule, InputIconModule,
        ButtonModule, PaginatorModule, TooltipModule, NamesListPipe,
        ReactiveFormsModule, ToggleSwitch
    ],
    providers: [
        MessageService, ConfirmationService, BooksService, UsersService, BorrowsService
    ],
    templateUrl: './books-to-borrow.component.html',
    styleUrl: './books-to-borrow.component.scss'
})
export class BooksToBorrowComponent implements OnInit {
    bookToBorrowDialog: boolean = false;
    books!: Book[];
    book!: Book;
    selectedBooks!: Book[] | null;
    submitted: boolean = false;
    statuses!: any[];
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;
    checked: boolean = false;
    borrow!: CreateBorrowRequest;

    editMode = false;
    borrowForm!: FormGroup;
    editingBorrow?: CreateBorrowRequest;

    users!: any[] | undefined;
    user: string | undefined;

    constructor(
        private bookService: BooksService,
        private borrowService: BorrowsService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
        private usersService: UsersService,
        private fb: FormBuilder
    ) {
    }
    ngOnInit() {
        this.loadData(this.checked);
        this.borrowForm = this.fb.group({
            user: [null, Validators.required],
        });
    }

    loadData(value: boolean) {
        this.loading = true;
        this.bookService.GetAllBooks()
            .then((data: Book[]) => {
                if (!value) {
                    this.books = data.filter(x => x.isAvailable && !x.isDeleted);
                } else {
                    this.books = data.filter(x => !x.isDeleted);
                }
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
        this.usersService.GetAllUsersDictionary()
            .then((data: any) => {
                this.users = data;
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
    }

    toolt(book: Book) {
        return book.description;
    }

    hideDialog() {
        this.bookToBorrowDialog = false;
        this.submitted = false;
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

    bookToBorrow(book: Book) {
        this.borrowForm.reset();
        this.bookToBorrowDialog = true;
        this.book = {...book};
    }

    getAuthorsNames(book?: Book): string {
        return (book?.authors ?? [])
            .map(a => `${a.surname} ${a.name}`)
            .join(', ');
    }

    saveBorrow() {
        if (this.borrowForm.invalid) {
            this.borrowForm.markAllAsTouched();
            return;
        }

        const borrowRequest = {
            bookId: this.book.id as string,
            userId: this.borrowForm.value.user as string,
        };

        console.log('borrowRequest:', borrowRequest);
        try {
            console.log('srodek try');
            this.borrowService.CreateBorrow(borrowRequest);
            this.messageInfo('Book borrowed', 'success');
            this.bookToBorrowDialog = false;
        } catch (err) {
            if (err instanceof HttpErrorResponse) {
                this.messageInfo(err.error.message, 'error');
            } else {
                this.messageInfo('Unexpected error', 'error');
            }
        }
        this.loadData(this.checked);
    }
    messageInfo(message: string, kind: string) {
        this.messageService.add({ severity: kind, summary: kind.toUpperCase(), detail: message, life: 3000 });
    }
    protected readonly HTMLInputElement = HTMLInputElement;
}

