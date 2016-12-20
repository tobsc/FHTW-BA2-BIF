import {CartComponent} from "./cart.component";
import {Routes, RouterModule} from "@angular/router";
import {AuthGuard} from "../shared/auth.guard";
const CART_ROUTES: Routes = [
  { path: '', component: CartComponent, canActivate: [AuthGuard],}
];
export const cartRouting = RouterModule.forChild(CART_ROUTES);
