import {Routes, RouterModule} from "@angular/router";
import {AuthGuard} from "../authentication/auth.guard";
import {OrderComponent} from "./order.component";
const ORDER_ROUTES: Routes = [
  { path: '', component: OrderComponent, canActivate: [AuthGuard]}
];
export const orderRouting = RouterModule.forChild(ORDER_ROUTES);
