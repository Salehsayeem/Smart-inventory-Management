import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Dashboard } from './features/dashboard/dashboard';
import { MainPage } from './features/main-page/main-page';
import { Profile } from './features/profile/profile';

const routes: Routes = [
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () =>
      import('./auth/auth-routing.module').then(m => m.AuthRoutingModule)
  },
  {
    path: '',
    component: MainPage,
    children: [
      { path: 'dashboard', component: Dashboard },
      // Add other routes here as they are implemented
       { path: 'profile', component: Profile },
      // { path: 'products', component: Products },
      // { path: 'categories', component: Categories },
      // { path: 'inventory-tracking', component: InventoryTracking },
      // { path: 'purchase-orders', component: PurchaseOrders },
      // { path: 'suppliers', component: Suppliers },
      // { path: 'reports-logs', component: ReportsLogs },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
