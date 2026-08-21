import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BorrowComponent } from './borrow.component';
import { BorrowsService } from '../../service/borrow.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { of } from 'rxjs';

// Mock serwisu BorrowService
class MockBorrowsService {
    GetAllBorrows() {
        return Promise.resolve([
            { id: '1', bookId: 'b1', bookName: 'Angular Basics', userId: 'u1', userFullName: 'Jan Kowalski', borrowDate: new Date().toISOString() }
        ]);
    }
    DeleteBorrow(borrow: any) {
        return Promise.resolve();
    }
}

describe('BorrowComponent', () => {
    let component: BorrowComponent;
    let fixture: ComponentFixture<BorrowComponent>;
    let borrowService: BorrowsService;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [BorrowComponent], // standalone component
            providers: [
                MessageService,
                ConfirmationService,
                { provide: BorrowsService, useClass: MockBorrowsService }
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(BorrowComponent);
        component = fixture.componentInstance;
        borrowService = TestBed.inject(BorrowsService);
    });

    it('powinien utworzyć komponent', () => {
        expect(component).toBeTruthy();
    });

    it('powinien załadować dane przy init', async () => {
        await component.loadData();
        expect(component.borrows.length).toBeGreaterThan(0);
        expect(component.loading).toBeFalse();
    });

    it('powinien otworzyć formularz dodawania (openNew)', () => {
        component.openNew();
        expect(component.borrowDialog).toBeTrue();
        expect(component.submitted).toBeFalse();
    });

    it('powinien znaleźć indeks po ID', () => {
        component.borrows = [{ id: '123' } as any, { id: '456' } as any];
        expect(component.findIndexById('456')).toBe(1);
        expect(component.findIndexById('not-exist')).toBe(-1);
    });
});
