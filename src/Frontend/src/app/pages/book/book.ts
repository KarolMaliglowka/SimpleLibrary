export interface Book {
    id?: string;
    name?: string;
    description?: string;
    pagesCount?: number;
    authors?: string;
    publisher?: string;
    isbn?: string;
    yearOfRelease?: string;
    category?: number;
    isBorrowed: boolean;
}
