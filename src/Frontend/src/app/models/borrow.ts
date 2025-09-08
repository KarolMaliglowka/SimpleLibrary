import {Author} from './author';
export interface Borrow {
    id?: string;
    bookId?: string;
    bookName?: string;
    bookAuthors?: Author[];
    userId?: string;
    userFullName?: string;
    borrowDate?: string;
}
