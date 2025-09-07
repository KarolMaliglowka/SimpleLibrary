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
import {FormsModule} from '@angular/forms';
import {InputNumber} from 'primeng/inputnumber';
import {IconFieldModule} from 'primeng/iconfield';
import {InputIconModule} from 'primeng/inputicon';
import {TableModule} from 'primeng/table';
import {Table} from 'primeng/table';
import {ButtonModule} from 'primeng/button';
import {PaginatorModule} from "primeng/paginator";
import {TooltipModule} from 'primeng/tooltip'
import {Publisher} from '../../models/publisher';
import {PublishersService} from '../../service/publisher.service';

@Component({
    selector: 'app-publisher',
    imports: [
        TableModule, Dialog, SelectModule, ToastModule, ToolbarModule,
        ConfirmDialog, InputTextModule, TextareaModule, CommonModule,
        FormsModule, InputNumber, IconFieldModule, InputIconModule,
        ButtonModule, PaginatorModule, TooltipModule
    ],
    providers: [
        MessageService, ConfirmationService, PublishersService
    ],
    templateUrl: './publisher.component.html',
    styleUrl: './publisher.component.scss'
})
export class PublisherComponent implements OnInit {
    publisherDialog: boolean = false;
    publishers!: Publisher[];
    publisher!: Publisher;
    selectedPublishers!: Publisher[] | null;
    submitted: boolean = false;
    statuses!: any[];
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;

    constructor(
        private publisherService: PublishersService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
    ) {}

    ngOnInit() {
        this.loadData();
    }

    loadData() {
        this.loading = true;
        this.publisherService.GetAllPublishers()
            .then((data: any) => {
                this.publishers = data;
                this.loading = false;
                this.cd.markForCheck();
            }).catch(() => {
            this.loading = false;
        });
    }

    openNew() {
        this.publisher = {};
        this.submitted = false;
        this.publisherDialog = true;
    }

    editPublisher(publisher: Publisher) {
        this.publisher = {...publisher};
        this.publisherDialog = true;
    }

    hideDialog() {
        this.publisherDialog = false;
        this.submitted = false;
    }

    deletePublisher(publisher: Publisher) {
        this.confirmationService.confirm({
            message: 'Are you sure you want to delete ' + publisher.name + '?',
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
                this.publishers = this.publishers.filter((val) => val.id !== publisher.id);
                //przesłac do servisu http i wykasować

                this.publisher;
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Publisher Deleted',
                    life: 3000
                });
            }
        });
    }

    findIndexById(id: string): number {
        let index = -1;
        for (let i = 0; i < this.publishers.length; i++) {
            if (this.publishers[i].id === id) {
                index = i;
                break;
            }
        }
        return index;
    }

    savePublisher() {
        this.submitted = true;
        if (this.publisher.name?.trim()) {
            if (this.publisher.id) {
                this.publishers[this.findIndexById(this.publisher.id)] = this.publisher;
                this.publisherService.UpdatePublisher(this.publisher);
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Publisher Updated',
                    life: 3000
                });
            } else {
                this.publishers.push(this.publisher);
                this.publisherService.CreatePublisher(this.publisher);
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'Publisher Created',
                    life: 3000
                });
            }
            this.publishers = [...this.publishers];
            this.publisherDialog = false;
            this.publisher;
        }
    }
    //protected readonly HTMLInputElement = HTMLInputElement;
}
