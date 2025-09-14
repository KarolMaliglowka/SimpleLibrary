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
import { ToggleSwitch } from 'primeng/toggleswitch';
import {Book} from '../../models/book';
import {BooksService} from '../../service/book.service';
import {NamesListPipe} from '../../../shared/extensions/NamesListPipe';

@Component({
  selector: 'app-books-to-borrow',
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, InputNumber, IconFieldModule, InputIconModule,
        ButtonModule, PaginatorModule, TooltipModule, ToggleSwitch,
        NamesListPipe
    ],
    providers: [
        MessageService, ConfirmationService, BooksService
    ],
  templateUrl: './books-to-borrow.component.html',
  styleUrl: './books-to-borrow.component.scss'
})
export class BooksToBorrowComponent implements OnInit {
    bookDialog: boolean = false;
    books!: Book[];
    book!: Book;
    selectedBooks!: Book[] | null;
    submitted: boolean = false;
    statuses!: any[];
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;
    checked: boolean = false;

    constructor(
        private bookService: BooksService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
    ) {
    }

    ngOnInit() {
        this.loadData(this.checked);
    }

    loadData(value: boolean) {
        this.loading = true;
        this.bookService.GetAllBooks()
            .then((data: Book[]) => {
                // filtruj w zależności od stanu przełącznika
                if (!value) {
                    this.books = data.filter(x => x.isAvailable);
                } else {
                    this.books = data;
                }
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
    }

    toolt(book: Book){
        return book.description;
    }

    hideDialog() {
        this.bookDialog = false;
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



        protected readonly HTMLInputElement = HTMLInputElement;
}
