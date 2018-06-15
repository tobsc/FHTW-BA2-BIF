import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import {TopNavbarComponent} from "./ui/top-navbar.component";
import {SidebarComponent} from "./ui/sidebar.component";
import {FooterComponent} from "./ui/footer.component";
import {PageNotFoundComponent} from "./page-not-found/page-not-found.component";
import { BsDropdownModule, CollapseModule, AccordionModule} from "ngx-bootstrap";
import {RouterModule} from "@angular/router";
import { PanelComponent } from './panel/panel.component';
import { LoadingIncidcatorComponent } from './loading-indicator/loading-incidcator.component';
import {OrderStatusLabelClassDirective} from "./directives/order-status-label-class.directive";
import {DeviceStatusLabelClassDirective} from "./directives/device-status-label-class.directive";
import {ExcludeFieldGroupPipe} from "../shared/pipes/exclude-field-group.pipe";
import { SearchComponent } from './search/search.component';
import { FormsModule } from "@angular/forms";
import { SearchResultListComponent } from './search/search-result-list/search-result-list.component';
import { AlertModule } from 'ngx-bootstrap';
import { TrackscrollDirective } from './directives/trackscroll.directive';
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
        SearchComponent,
        SearchResultListComponent,
        TrackscrollDirective,
    ],
    imports: [
        CommonModule,
        BsDropdownModule.forRoot(),
        CollapseModule.forRoot(),
        RouterModule,
        FormsModule,
        AccordionModule.forRoot(),
        AlertModule.forRoot(),
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
        TrackscrollDirective,
    ]
})
export class CoreModule {}
