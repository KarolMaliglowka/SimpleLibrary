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
import {TooltipModule} from 'primeng/tooltip';
import {Book} from '../../models/book';
import {Author} from '../../models/author';
import {BooksService} from '../../service/book.service';
import {NamesListPipe} from '../../../shared/extensions/NamesListPipe';
import {PublishersService} from '../../service/publisher.service';
import {CategoriesService} from '../../service/category.service';
import {AuthorsService} from '../../service/author.service';
import {MultiSelectModule} from 'primeng/multiselect';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {HttpErrorResponse} from '@angular/common/http';
import JsBarcode from 'jsbarcode';

@Component({
    selector: 'book-component',
    templateUrl: 'book.component.html',
    styleUrl: 'book.component.scss',
    standalone: true,
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, InputNumber, IconFieldModule, InputIconModule,
        ButtonModule, PaginatorModule, TooltipModule, NamesListPipe,
        ReactiveFormsModule, MultiSelectModule
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

    async ngOnInit() {
        await this.loadData();
        this.bookForm = this.fb.group({
            id: [null],
            name: ['', Validators.required],
            description: [''],

            category: [null, Validators.required],
            publisher: [null, Validators.required],
            authors: [[]],

            pagesCount: [0],
            yearOfRelease: [''],
            isbn: [''],
            code: ['']
        });
    }

    async loadData() {
        this.loading = true;
        try {
            const [books, publishers, categories, authors] = await Promise.all([
                this.bookService.GetAllBooks(),
                this.publisherService.GetAllPublishersDictionary(),
                this.categoriesService.GetAllCategoriesDictionary(),
                this.authorsService.GetAllAuthorsDictionary()
            ]);
            this.books = books.filter(x => !x.isDeleted);
            this.publishers = publishers;
            this.categories = categories;
            this.authors = authors;
            this.cd.markForCheck();
        } catch (error) {
            console.error(error);
        } finally {
            this.loading = false;
        }
    }

    openNew() {
        this.editingBook = undefined;
        this.bookForm.reset();
        this.bookDialog = true;
        this.editMode = false;
    }

    toolt(book: Book) {
        return book.description;
    }

    editBook(book: Book) {
        this.editingBook = book;
        const category = this.categories?.find(
            x => x.id === book.category?.id
        );
        const publisher = this.publishers?.find(
            x => x.id === book.publisher?.id
        );
        const authors = this.authors?.filter(
            x => book.authors?.some(a => a.id === x.id)
        );
        this.bookForm.patchValue({
            id: book.id,
            name: book.name,
            description: book.description,
            category: category,
            publisher: publisher,
            authors: authors,
            pagesCount: book.pagesCount,
            yearOfRelease: book.yearOfRelease,
            isbn: book.isbn,
            code: book.code
        });
        this.bookDialog = true;
        this.editMode = true;
    }

    hideDialog() {
        this.bookDialog = false;
    }

    async saveBook() {
        if (this.bookForm.invalid) return;
        const newBook: Book = this.bookForm.value;
        try {
            if (this.editMode) {
                await this.bookService.UpdateBook(newBook);
                this.messageInfo('Updated book', 'success');
            } else {
                await this.bookService.CreateBook(newBook);
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
        await this.loadData();
    }

    messageInfo(message: string, kind: string) {
        this.messageService.add({severity: kind, summary: kind.toUpperCase(), detail: message, life: 3000});
    }

    printBookCode(code: string, name: string): void {
        const printWindow = window.open('', '_blank', 'width=400,height=300');

        if (!printWindow) {
            return;
        }

        printWindow.document.write(`
    <!DOCTYPE html>
    <html>
      <head>
        <title>Kod książki</title>

        <style>
          @page {
            size: 50mm 30mm;
            margin: 0;
          }

          html, body {
            margin: 0;
            padding: 0;
            width: 50mm;
            height: 30mm;
          }

          body {
            display: flex;
            justify-content: center;
            align-items: center;
          }

          .label {
            width: 48mm;
            height: 28mm;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
          }

          svg {
            width: 42mm;
            height: 15mm;
          }

          .code {
            margin-top: 2mm;
            font-family: Arial, sans-serif;
            font-size: 12pt;
            letter-spacing: 1px;
          }
        </style>
      </head>

      <body>
        <div class="label">
          <svg id="barcode"></svg>
          <div class="code">${code}</div>
          <div>${name}</div>
        </div>
      </body>
    </html>
  `);

        printWindow.document.close();

        printWindow.onload = () => {
            const barcode = printWindow.document.getElementById('barcode');

            if (!barcode) {
                return;
            }

            JsBarcode(barcode, code, {
                format: 'CODE128',
                displayValue: false,
                width: 1.5,
                height: 50,
                margin: 0
            });

            printWindow.focus();
            printWindow.print();
        };
    }

    confirm(book: Book) {
        this.confirmationService.confirm({
            header: 'Are you confirm delete: ' + book.name + '?',
            message: 'Please confirm to \n\b proceed.',
            icon: 'pi pi-exclamation-triangle',
            accept: async () => {
                try {
                    await this.bookService.DeleteBook(book.id!);
                    this.books = this.books.filter(x => x.id !== book.id);
                    this.messageService.add({
                        severity: 'success',
                        summary: 'Success',
                        detail: 'The book has been removed.'
                    });

                } catch (err: any) {
                    this.messageService.add({
                        severity: 'error',
                        summary: 'Error',
                        detail: err?.error?.message ?? 'An unexpected error occurred.'
                    });
                }
            },
        });
    }

    deleteBook(book: Book) {
        this.bookService.DeleteBook(book.id as string);
    }

    protected readonly HTMLInputElement = HTMLInputElement;
}
