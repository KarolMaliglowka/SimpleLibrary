import {Book} from './book';

export interface User {
    id?: string;
    name?: string;
    surname?: string;
    email?: string;
    address?: string;
    phoneNumber?: string;
    city?: string;
    country?: string;
    postalCode?: string;
    isActive?: boolean;
    fullName?: string;
    books?: Book[];
    isDeleted?: boolean;
}
