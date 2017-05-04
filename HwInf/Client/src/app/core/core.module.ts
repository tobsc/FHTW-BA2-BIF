import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import {TopNavbarComponent} from "./ui/top-navbar.component";
import {SidebarComponent} from "./ui/sidebar.component";
import {FooterComponent} from "./ui/footer.component";
import {PageNotFoundComponent} from "./page-not-found/page-not-found.component";
import {DropdownModule, CollapseModule} from "ng2-bootstrap";
import {RouterModule} from "@angular/router";
import { PanelComponent } from './panel/panel.component';
import { LoadingIncidcatorComponent } from './loading-indicator/loading-incidcator.component';
import {OrderStatusLabelClassDirective} from "./directives/order-status-label-class.directive";
import {DeviceStatusLabelClassDirective} from "./directives/device-status-label-class.directive";
import {ExcludeFieldGroupPipe} from "../shared/pipes/exclude-field-group.pipe";
@NgModule({
    declarations: [
        TopNavbarComponent,
        SidebarComponent,
        FooterComponent,
        PageNotFoundComponent,
        PanelComponent,
        LoadingIncidcatorComponent,
        OrderStatusLabelClassDirective,
        DeviceStatusLabelClassDirective,
        ExcludeFieldGroupPipe,
    ],
    imports: [
        CommonModule,
        DropdownModule.forRoot(),
        CollapseModule.forRoot(),
        RouterModule,
    ],
    exports: [
        TopNavbarComponent,
        SidebarComponent,
        FooterComponent,
        PageNotFoundComponent,
        PanelComponent,
        LoadingIncidcatorComponent,
        OrderStatusLabelClassDirective,
        DeviceStatusLabelClassDirective,
        ExcludeFieldGroupPipe,
    ]
})
export class CoreModule {}