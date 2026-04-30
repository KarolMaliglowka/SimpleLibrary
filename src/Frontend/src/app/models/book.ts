import {Author} from './author';

export interface Book {
    id?: string;
    name?: string;
    description?: string;
    pagesCount?: number;
    authors?: Author[];
    publisher?: string;
    isbn?: string;
    yearOfRelease?: string;
    category?: string;
    isAvailable?: boolean;
}
