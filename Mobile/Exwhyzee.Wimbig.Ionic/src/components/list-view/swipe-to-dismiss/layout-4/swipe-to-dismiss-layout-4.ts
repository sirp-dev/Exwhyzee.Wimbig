import { Component, Input, ViewChild, AfterViewInit } from '@angular/core';
import { IonicPage, Content, FabButton, ItemSliding } from 'ionic-angular';

@IonicPage()
@Component({
    selector: 'swipe-to-dismiss-layout-4',
    templateUrl: 'swipe-to-dismiss.html'
})
export class SwipeToDismissLayout4 implements AfterViewInit {
    @Input() data: any;
    @Input() events: any;
    @ViewChild(Content)
    content: Content;
    @ViewChild(FabButton)
    fabButton: FabButton;

    constructor() { }

    onEvent(event: string, item: any, e: any) {
        if (e) {
            e.stopPropagation();
        }
        if (this.events[event]) {
            this.events[event](item);
        }
    }

    undo = (slidingItem: ItemSliding) => {
        slidingItem.close();
    }

    delete = (item: any): void => {
        let index = this.data.items.indexOf(item);
        if (index > -1) {
            this.data.items.splice(index, 1);
        }
    }

    ngAfterViewInit() {
        this.content.ionScroll.subscribe((d) => {
            this.fabButton.setElementClass("fab-button-out", d.directionY == "down");
        });
    }

}
