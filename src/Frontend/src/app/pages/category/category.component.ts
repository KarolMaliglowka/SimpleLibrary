import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';
import { Table, TableModule } from 'primeng/table';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { CategoriesService } from '../../service/category.service';
import { Category } from '../../models/category';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-category',
    standalone: true,
    templateUrl: './category.component.html',
    styleUrls: ['./category.component.scss'],
    imports: [
        ButtonModule,
        ToolbarModule,
        TableModule,
        IconFieldModule,
        InputIconModule,
        InputTextModule,
        ReactiveFormsModule,
        DialogModule,
        CommonModule
    ],
    providers: [MessageService, ConfirmationService, CategoriesService]
})
export class CategoryComponent implements OnInit {
    categories!: Category[];
    editingCategory?: Category;
    category!: Category;
    @ViewChild('dt') dt!: Table;
    editMode = false;
    categoryDialog = false;
    categoryForm!: FormGroup;

    constructor(
        private categoriesService: CategoriesService,
        private fb: FormBuilder,
        private cd: ChangeDetectorRef
    ) {}

    async ngOnInit() {
        await this.loadData();
        this.categoryForm = this.fb.group({
            id: [null],
            name: ['', Validators.required]
        });
    }

    async loadData() {
        this.categoriesService
            .GetAllCategories()
            .then((data) => {
                this.categories = data;
                this.cd.markForCheck();
            })
            .catch(() => {});
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
            } else {
                await this.categoriesService.CreateCategory(newCategory);
            }
            this.categoryDialog = false;
        } catch (err) {
            console.error('Błąd zapisu kategorii:', err);
        }
        await this.loadData();
    }
    deleteCategory(category: Category) {
        console.log('Delete', category);
        //await this.categoriesService.DeleteCategory(newCategory);
    }
}
