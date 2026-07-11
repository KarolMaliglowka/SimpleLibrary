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
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
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
        ReactiveFormsModule, ButtonModule, PaginatorModule, TooltipModule
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
    @ViewChild('dt') dt!: Table;
    loading: boolean = false;
    editMode = false;
    publisherForm!: FormGroup;
    editingPublisher?: Publisher;

    constructor(
        private publisherService: PublishersService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private cd: ChangeDetectorRef,
        private fb: FormBuilder
    ) {}

    async ngOnInit() {
        await this.loadData();
        this.publisherForm = this.fb.group({
            id: [null],
            name: ['', Validators.required]
        });
    }

    async loadData() {
        this.loading = true;
        this.publisherService
            .GetAllPublishers()
            .then((data) => {
                this.publishers = data.filter(x => !x.isDelete);
                this.loading = false;
                this.cd.markForCheck();
            })
            .catch(() => {
            this.loading = false;
        });
    }

    openNew() {
        this.editingPublisher = undefined;
        this.publisherForm.reset();
        this.publisherDialog = true;
        this.editMode = false;
    }

    editPublisher(publisher: Publisher) {
        this.editingPublisher = publisher;
        this.publisherForm.patchValue(publisher);
        this.publisherDialog = true;
        this.editMode = true;
    }

    // deletePublisher(publisher: Publisher) {
    //     this.confirmationService.confirm({
    //         message: 'Are you sure you want to delete ' + publisher.name + '?',
    //         header: 'Confirm',
    //         icon: 'pi pi-exclamation-triangle',
    //         rejectButtonProps: {
    //             label: 'No',
    //             severity: 'secondary',
    //             variant: 'text'
    //         },
    //         acceptButtonProps: {
    //             severity: 'danger',
    //             label: 'Yes'
    //         },
    //         accept: () => {
    //             this.publishers = this.publishers.filter((val) => val.id !== publisher.id);
    //             //przesłac do servisu http i wykasować
    //
    //             this.publisher;
    //             this.messageService.add({
    //                 severity: 'success',
    //                 summary: 'Successful',
    //                 detail: 'Publisher Deleted',
    //                 life: 3000
    //             });
    //         }
    //     });
    // }

    deletePublisher(publisher: Publisher) {
        this.publisherService.DeletePublisher(publisher.id as string);
    }

    async savePublisher() {
        if (this.publisherForm.invalid) return;
        const newPublisher:Publisher = this.publisherForm.value;
        try {
            if (this.editMode) {
                await this.publisherService.UpdatePublisher(newPublisher);
                this.messageInfo('Updated publisher', 'success');
            } else {
                await this.publisherService.CreatePublisher(newPublisher);
                this.messageInfo('Created new publisher', 'success');
            }
            this.publisherDialog = false;
        } catch (err) {
            console.error(err);
            this.messageInfo('Some error: ' + err, 'error');
        }
        await this.loadData();
    }

    messageInfo(message: string, kind: string) {
        this.messageService.add({ severity: kind, summary: kind.toUpperCase(), detail: message, life: 3000 });
    }

    confirm(publisher: Publisher) {
        console.log("środek potwierdzenia")
        this.confirmationService.confirm({
            header: 'Are you confirm delete: ' + publisher.name + '?',
            message: 'Please confirm to \n\b proceed.',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                var tst = this.deletePublisher(publisher);
                console.log("potwierdzenie: ", tst)
                this.messageService.add({ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' });
                this.publishers = this.publishers.filter((val) => val.id !== publisher.id);
            },
            reject: () => {
                this.messageService.add({ severity: 'info', summary: 'Rejected', detail: 'You have rejected' });
            },
        });
    }
    protected readonly HTMLInputElement = HTMLInputElement;
}
