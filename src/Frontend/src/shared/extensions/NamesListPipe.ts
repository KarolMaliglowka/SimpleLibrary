import { Pipe, PipeTransform } from '@angular/core';

interface Person {
    id: string;
    name: string;
    surname: string;
}

@Pipe({
    name: 'namesList',
    standalone: true
})
export class NamesListPipe implements PipeTransform {
    transform(value: Person[] | null | undefined): string {
        if (!value || value.length === 0) {
            return '';
        }

        return value
            .map(person => `${person.surname} ${person.name}`)
            .join(', ');
    }
}
