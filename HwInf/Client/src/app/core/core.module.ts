import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import {TopNavbarComponent} from "./ui/top-navbar.component";
import {SidebarComponent} from "./ui/sidebar.component";
import {FooterComponent} from "./ui/footer.component";
import {PageNotFoundComponent} from "./page-not-found/page-not-found.component";
import {DropdownModule, CollapseModule} from "ng2-bootstrap";
@NgModule({
    declarations: [
        TopNavbarComponent,
        SidebarComponent,
        FooterComponent,
        PageNotFoundComponent,
    ],
    imports: [
        CommonModule,
        DropdownModule.forRoot(),
        CollapseModule.forRoot(),
    ],
    exports: [
        TopNavbarComponent,
        SidebarComponent,
        FooterComponent,
        PageNotFoundComponent
    ]
})
export class CoreModule {}