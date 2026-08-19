import {Component, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CarouselModule} from 'primeng/carousel';
import {Dashboard} from '../../models/dashboard';
import {DashboardService} from '../../service/dashboard.service';
import {CardModule} from 'primeng/card';

interface DashboardItem {
    title: string;
    value: number;
    icon: string;
}

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [
        CommonModule,
        CarouselModule,
        CardModule
    ],
    providers: [
        DashboardService
    ],
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

    dashboard?: Dashboard;

    dashboardItems: DashboardItem[] = [];

    loading = false;

    responsiveOptions = [
        {
            breakpoint: '1400px',
            numVisible: 3,
            numScroll: 1
        },
        {
            breakpoint: '1024px',
            numVisible: 2,
            numScroll: 1
        },
        {
            breakpoint: '768px',
            numVisible: 1,
            numScroll: 1
        }
    ];

    constructor(
        private dashboardService: DashboardService
    ) {
    }

    ngOnInit(): void {
        this.loadDashboard();
    }

    private loadDashboard(): void {
        this.loading = true;
        this.dashboardService
            .GetDasboard()
            .then((data) => {
                this.dashboard = data;
                this.dashboardItems = [
                    {
                        title: 'Available books',
                        value: data.booksCount,
                        icon: 'pi pi-book'
                    },
                    {
                        title: 'Users in system',
                        value: data.usersCount,
                        icon: 'pi pi-users'
                    },
                    {
                        title: 'Books currently on loan',
                        value: data.borrowedBooksCount,
                        icon: 'pi pi-arrow-right-arrow-left'
                    }
                ];

                this.loading = false;
            })
            .catch((error) => {

                console.error('Error loading dashboard:', error);

                this.loading = false;
            });
    }
}
