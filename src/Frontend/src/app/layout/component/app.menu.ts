import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from '@angular/router';
import {MenuItem} from 'primeng/api';
import {AppMenuitem} from './app.menuitem';

@Component({
    selector: 'app-menu',
    standalone: true,
    imports: [CommonModule, AppMenuitem, RouterModule],
    template: `
        <ul class="layout-menu">
            <ng-container *ngFor="let item of model; let i = index">
                <li app-menuitem *ngIf="!item.separator" [item]="item" [index]="i" [root]="true"></li>
                <li *ngIf="item.separator" class="menu-separator"></li>
            </ng-container>
        </ul>
    `
})
export class AppMenu {
    model: MenuItem[] = [];

    ngOnInit() {
        this.model = [
            {
                label: 'Home',
                items: [{label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/']}]
            },
            {
                label: 'Library',
                items: [
                    {
                        label: 'Available books',
                        icon: 'pi pi-fw pi-book',
                        routerLink: ['/pages/books-to-borrow']
                    },
                    {
                        label: 'Borrowed books',
                        icon: 'pi pi-fw pi-arrow-right-arrow-left',
                        routerLink: ['/pages/borrow']
                    }
                ]
            },
            {
                label: 'Manage',
                items: [
                    {
                        label: 'Books',
                        icon: 'pi pi-fw pi-book',
                        routerLink: ['/pages/book']
                    },
                    {
                        label: 'Users',
                        icon: 'pi pi-fw pi-user',
                        routerLink: ['/pages/user']
                    },
                    {
                        label: 'Categories',
                        icon: 'pi pi-fw pi-arrow-right-arrow-left',
                        routerLink: ['/pages/category']
                    },
                    {
                        label: 'Authors',
                        icon: 'pi pi-fw pi-arrow-right-arrow-left',
                        routerLink: ['/pages/author']
                    },
                    {
                        label: 'Publishers',
                        icon: 'pi pi-fw pi-arrow-right-arrow-left',
                        routerLink: ['/pages/publisher']
                    }
                ]
            }
            // ,
            // {
            //     label: 'Settings',
            //     items: [
            //         {
            //             label: 'Settings',
            //             icon: 'pi pi-fw pi-cog',
            //             routerLink: ['/pages/setting']
            //         }
            //     ]
            // }
        ];
    }
}
