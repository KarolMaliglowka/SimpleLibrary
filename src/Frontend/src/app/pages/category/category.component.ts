import { Component } from '@angular/core';
import {ButtonModule} from 'primeng/button';
import {ToolbarModule} from 'primeng/toolbar';
import {TableModule} from 'primeng/table';
import {IconFieldModule} from 'primeng/iconfield';
import {InputIconModule} from 'primeng/inputicon';
import {InputTextModule} from 'primeng/inputtext';

import {Category} from './category';

@Component({
  selector: 'app-category',
  imports: [ButtonModule, ToolbarModule, TableModule, IconFieldModule, InputIconModule, InputTextModule],
  templateUrl: './category.component.html',
  styleUrl: './category.component.scss'
})
export class CategoryComponent {

    categories!: Category[];
    category!: Category;

    openNew() {

    }

    editCategory(category: Category) {}

    deleteCategory(category: Category) {}
}
