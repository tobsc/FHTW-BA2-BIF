import {NgModule} from "@angular/core";
import { ConfirmButtonComponent } from './confirm-button/confirm-button.component';
import {PopoverModule} from "ng2-bootstrap";
import { ClickOutsideDirective } from './click-outside.directive';
@NgModule({
    declarations: [
        ConfirmButtonComponent,
        ClickOutsideDirective,
    ],
    imports: [
        PopoverModule.forRoot()
    ],
    exports: [
        ConfirmButtonComponent
    ],
})
export class ConfirmDialogModule {}