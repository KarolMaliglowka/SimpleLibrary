import {ChangeDetectorRef, Component, OnInit, ViewChild} from '@angular/core';
import {Table, TableModule} from "primeng/table";
import {Dialog} from "primeng/dialog";
import {SelectModule} from "primeng/select";
import {ToastModule} from "primeng/toast";
import {ToolbarModule} from "primeng/toolbar";
import {ConfirmDialog} from "primeng/confirmdialog";
import {InputTextModule} from "primeng/inputtext";
import {TextareaModule} from "primeng/textarea";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {InputNumber} from "primeng/inputnumber";
import {IconFieldModule} from "primeng/iconfield";
import {InputIconModule} from "primeng/inputicon";
import {ButtonModule} from "primeng/button";
import {PaginatorModule} from "primeng/paginator";
import {TooltipModule} from "primeng/tooltip";
import {ConfirmationService, MessageService} from "primeng/api";
import {Borrow} from '../../models/borrow';
import {BorrowsService} from '../../service/borrow.service';
import {NamesListPipe} from '../../../shared/extensions/NamesListPipe';
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-borrow',
  imports: [
      TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
      ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
      FormsModule, InputNumber, IconFieldModule, InputIconModule,
      ButtonModule, PaginatorModule, TooltipModule, NamesListPipe
  ],
    providers: [
        MessageService, ConfirmationService, BorrowsService
    ],
  templateUrl: './borrow.component.html',
  styleUrl: './borrow.component.scss'
})
export class BorrowComponent implements OnInit {
    borrowDialog: boolean = false;
    borrows!: Borrow[];
    borrow!: Borrow;
    selectedBorrows!: Borrow[] | null;
    submitted: boolean = false;
    statuses!: any[];
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;

    constructor(
        private borrowService: BorrowsService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
    ) {}

    ngOnInit() {
        this.loadData();
    }

    loadData() {
        this.loading = true;
        this.borrowService.GetAllBorrows()
            .then((data: any) => {
                this.borrows = data;
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
    }

    openNew() {
        this.borrow = {};
        this.submitted = false;
        this.borrowDialog = true;
    }

    returnBook(borrow: Borrow) {
        try {
            this.borrowService.DeleteBorrow(borrow.id);
            this.messageInfo('Book was returned', 'success');
            this.borrowDialog = false;
        } catch (err) {
            if (err instanceof HttpErrorResponse) {
                this.messageInfo(err.error.message, 'error');
            } else {
                this.messageInfo('Unexpected error', 'error');
            }
        }
        this.loadData();
    }

    hideDialog() {
        this.borrowDialog = false;
        this.submitted = false;
    }

    // findIndexById(id: string): number {
    //     let index = -1;
    //     for (let i = 0; i < this.borrows.length; i++) {
    //         if (this.borrows[i].id === id) {
    //             index = i;
    //             break;
    //         }
    //     }
    //
    //     return index;
    // }

    confirm(borrow: Borrow) {
        this.confirmationService.confirm({
            header: 'Are you confirm return: ' + borrow.bookName + '?',
            message: 'Please confirm to \n\b proceed.',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.borrows = this.borrows.filter((val) => val.id !== borrow.id);
                this.returnBook(borrow);
            },
            reject: () => {
                this.messageService.add({ severity: 'info', summary: 'Rejected', detail: 'You have rejected' });
            },
        });
    }

    messageInfo(message: string, kind: string) {
        this.messageService.add({ severity: kind, summary: kind.toUpperCase(), detail: message, life: 3000 });
    }

    protected readonly HTMLInputElement = HTMLInputElement;
}
