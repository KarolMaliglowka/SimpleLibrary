import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import {ConfirmDialog} from "primeng/confirmdialog";
import {Dialog} from "primeng/dialog";
import { Toast } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { ToolbarModule } from 'primeng/toolbar';
import { Table, TableModule } from 'primeng/table';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Category } from '../../models/category';
import { CategoriesService } from '../../service/category.service';
import { HttpErrorResponse } from '@angular/common/http';
import {TooltipModule} from 'primeng/tooltip'

@Component({
    selector: 'app-category',
    standalone: true,
    templateUrl: './category.component.html',
    styleUrls: ['./category.component.scss'],
    imports: [
        ButtonModule,
        Dialog,
        ToolbarModule,
        TableModule,
        IconFieldModule,
        InputIconModule,
        InputTextModule,
        ReactiveFormsModule,
        Toast,
        CommonModule,
        ConfirmDialog,
        TooltipModule
    ],
    providers: [
        MessageService,
        ConfirmationService,
        CategoriesService
    ]
})
export class CategoryComponent implements OnInit {
    categories!: Category[];
    editingCategory?: Category;
    category!: Category;
    @ViewChild('dt') dt!: Table;
    editMode = false;
    categoryDialog = false;
    categoryForm!: FormGroup;
    loading: boolean = false;

    constructor(
        private categoriesService: CategoriesService,
        private fb: FormBuilder,
        private cd: ChangeDetectorRef,
        private messageService: MessageService,
        private confirmationService: ConfirmationService
    ) {}

    async ngOnInit() {
        await this.loadData();
        this.categoryForm = this.fb.group({
            id: [null],
            name: ['', Validators.required]
        });
    }

    async loadData() {
        this.loading = true;
        this.categoriesService
            .GetAllCategories()
            .then((data) => {
                this.categories = data.filter(x => !x.isDelete);
                this.cd.markForCheck();
            })
            .catch(() => {});
        this.loading = false;
    }

    openNew() {
        this.editingCategory = undefined;
        this.categoryForm.reset();
        this.categoryDialog = true;
        this.editMode = false;
    }

    editCategory(category: Category) {
        this.editingCategory = category;
        this.categoryForm.patchValue(category);
        this.categoryDialog = true;
        this.editMode = true;
    }

    async saveCategory() {
        if (this.categoryForm.invalid) return;
        const newCategory:Category = this.categoryForm.value;
        try {
            if (this.editMode) {
                await this.categoriesService.UpdateCategory(newCategory);
                this.messageInfo('Update category', 'success');
            } else {
                await this.categoriesService.CreateCategory(newCategory);
                this.messageInfo('Created new category', 'success');
            }
            this.categoryDialog = false;
        } catch (err) {
            if (err instanceof HttpErrorResponse) {
                this.messageInfo(err.error.message, 'error');
            } else {
                this.messageInfo('Unexpected error', 'error');
            }
        }
        await this.loadData();
    }
    deleteCategory(category: Category) {
        this.categoriesService.DeleteCategory(category.id as string);
    }

    messageInfo(message: string, kind: string) {
        this.messageService.add({ severity: kind, summary: kind.toUpperCase(), detail: message, life: 3000 });
    }

    confirm(category: Category) {
        this.confirmationService.confirm({
            header: 'Are you confirm delete: ' + category.name + '?',
            message: 'Please confirm to \n\b proceed.',
            icon: 'pi pi-exclamation-triangle',
            accept: async () => {
                try {
                    await this.categoriesService.DeleteCategory(category.id!);
                    this.categories = this.categories.filter(x => x.id !== category.id);
                    this.messageService.add({
                        severity: 'success',
                        summary: 'Success',
                        detail: 'The category has been removed.'
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
    protected readonly HTMLInputElement = HTMLInputElement;
}
