import { Routes } from '@angular/router';
import { Empty } from './empty/empty';
import { BookComponent} from './book/book.component'
import { UserComponent} from './user/user.component'
import { BorrowComponent} from './borrow/borrow.component'
import { SettingComponent} from './setting/setting.component'

export default [
    { path: 'empty', component: Empty },
    { path: 'book', component: BookComponent },
    { path: 'user', component: UserComponent },
    { path: 'borrow', component: BorrowComponent },
    { path: 'setting', component: SettingComponent },
    { path: '**', redirectTo: '/notfound' }
] as Routes;

