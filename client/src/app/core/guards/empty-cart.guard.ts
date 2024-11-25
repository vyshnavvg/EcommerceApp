import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { SnackbarService } from '../services/snackbar.service';
import { concatWith } from 'rxjs';

export const emptyCartGuard: CanActivateFn = (route, state) => {
  const cartService = inject(CartService);
  const snackbar = inject(SnackbarService);
  const router = inject(Router);

  if(!cartService.cart() || cartService.cart()?.items.length === 0){
    snackbar.error("Cart Cannot be empty");
    router.navigateByUrl('/cart');
    return false;
  }
  return true;
};
