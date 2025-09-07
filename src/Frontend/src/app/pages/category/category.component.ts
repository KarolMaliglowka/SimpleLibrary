import {ChangeDetectorRef, Component, OnInit, ViewChild} from '@angular/core';
import {ButtonModule} from 'primeng/button';
import {ToolbarModule} from 'primeng/toolbar';
import {Table, TableModule} from 'primeng/table';
import {IconFieldModule} from 'primeng/iconfield';
import {InputIconModule} from 'primeng/inputicon';
import {InputTextModule} from 'primeng/inputtext';
import {CategoriesService} from '../../service/category.service';
import {Category} from '../../models/category';
import {ConfirmationService, MessageService} from "primeng/api";

@Component({
    selector: 'app-category',
    imports: [ButtonModule, ToolbarModule, TableModule, IconFieldModule, InputIconModule, InputTextModule],
    providers: [MessageService, ConfirmationService, CategoriesService],
    templateUrl: './category.component.html',
    standalone: true,
    styleUrl: './category.component.scss'
})
export class CategoryComponent implements OnInit {

    categories!: Category[];
    category!: Category;
    @ViewChild('dt') dt!: Table;

    constructor(
        private categoriesService: CategoriesService,
        private cd: ChangeDetectorRef
    ) {}

    ngOnInit() {
        this.loadData();
    }

    loadData() {
        this.categoriesService.GetAllCategories()
            .then((data) => {
                console.log(data);
                this.categories = data;
                this.cd.markForCheck();
            }).catch(() => {
        });
    }

    openNew() {
    }

    editCategory(category: Category) {
    }

    deleteCategory(category: Category) {
    }

    //protected readonly name = name;
}
