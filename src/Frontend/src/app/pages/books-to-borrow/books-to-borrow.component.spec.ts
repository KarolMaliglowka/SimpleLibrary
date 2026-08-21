import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BooksToBorrowComponent } from './books-to-borrow.component';

describe('BooksToBorrowComponent', () => {
  let component: BooksToBorrowComponent;
  let fixture: ComponentFixture<BooksToBorrowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BooksToBorrowComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BooksToBorrowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
