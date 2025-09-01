import { Routes } from '@angular/router';
import { Empty } from './empty/empty';
import { TableBook } from './book/book.component'
import { UserComponent} from './user/user.component'
import { BorrowComponent} from './borrow/borrow.component'
import { SettingComponent} from './setting/setting.component'
import { CategoryComponent} from './category/category.component'
import { PublisherComponent} from './publisher/publisher.component'
import { AuthorComponent} from './author/author.component'

export default [
    { path: 'empty', component: Empty },
    { path: 'book', component: TableBook },
    { path: 'user', component: UserComponent },
    { path: 'borrow', component: BorrowComponent },
    { path: 'category', component: CategoryComponent },
    { path: 'author', component: AuthorComponent },
    { path: 'publisher', component: PublisherComponent },
    { path: 'setting', component: SettingComponent },
    { path: '**', redirectTo: '/notfound' }
] as Routes;

