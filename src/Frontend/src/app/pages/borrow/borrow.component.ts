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
    ) {
    }

    ngOnInit() {
        this.loadData();
        if(this.loadData() == null) {
            console.log("Loading...");
            //return "brak ksiązek";
        }
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
        const borrow1: Borrow = {
            id: borrow.id,
            bookId: borrow.bookId,
            bookName: borrow.bookName,
            bookAuthors: borrow.bookAuthors,
            userId: borrow.userId,
            userFullName: borrow.userFullName,
            borrowDate: new Date().toISOString()
        };
        this.borrowService.DeleteBorrow(borrow1);
    }

    hideDialog() {
        this.borrowDialog = false;
        this.submitted = false;
    }

    findIndexById(id: string): number {
        let index = -1;
        for (let i = 0; i < this.borrows.length; i++) {
            if (this.borrows[i].id === id) {
                index = i;
                break;
            }
        }

        return index;
    }

    confirm(borrow: Borrow) {
        this.confirmationService.confirm({
            header: 'Are you confirm return: ' + borrow.bookName + '?',
            message: 'Please confirm to \n\b proceed.',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.messageService.add({ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' });
                this.borrows = this.borrows.filter((val) => val.id !== borrow.id);
                this.returnBook(borrow);
            },
            reject: () => {
                this.messageService.add({ severity: 'info', summary: 'Rejected', detail: 'You have rejected' });
            },
        });
    }

    protected readonly HTMLInputElement = HTMLInputElement;
}
