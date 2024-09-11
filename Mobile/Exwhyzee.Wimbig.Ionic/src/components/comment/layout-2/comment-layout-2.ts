import { Component, Input } from '@angular/core';
import { IonicPage } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'comment-layout-2',
    templateUrl: 'comment.html'
})
export class CommentLayout2 {
    @Input() data: any;
    @Input() events: any;

    constructor() { }

    onEvent(event: string, item: any, e: any) {
        if (this.events[event]) {
            this.events[event](item);
        }
    }
}
