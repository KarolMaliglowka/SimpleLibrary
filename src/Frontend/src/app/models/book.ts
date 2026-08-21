import {Author} from './author';
import {Category} from './category';
import {Publisher} from './publisher';

export interface Book {
    id?: string;
    name: string;
    description?: string;
    pagesCount?: number;
    authors?: Author[];
    publisher?: Publisher;
    isbn?: string;
    yearOfRelease?: string;
    category?: Category;
    isAvailable?: boolean;
    isDeleted?: boolean;
    code: string;
}
