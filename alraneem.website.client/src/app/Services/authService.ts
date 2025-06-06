import { Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private msalService: MsalService) { }

  isLoggedIn(): boolean {
    const account = this.msalService.instance.getActiveAccount();
    return !!account;
  }
}
