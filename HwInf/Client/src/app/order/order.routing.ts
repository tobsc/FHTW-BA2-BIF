import {Routes, RouterModule} from "@angular/router";
import {AuthGuard} from "../authentication/auth.guard";
import {OrderComponent} from "./order.component";
import {OrderCreateComponent} from "./create/order-create.component";
const ORDER_ROUTES: Routes = [
  { path: '', component: OrderComponent, canActivate: [AuthGuard]},
  { path: 'create', component: OrderCreateComponent, canActivate: [AuthGuard]}

];
export const orderRouting = RouterModule.forChild(ORDER_ROUTES);
