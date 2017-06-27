import { Injectable } from '@angular/core';
import { Jwt } from '../models/jwt.model';

@Injectable()
export class JwtService {

    private readonly TOKEN: string = "auth_token";
    private jwt: Jwt = null;

  constructor() { }

  public getToken() : string {
      return localStorage.getItem(this.TOKEN);
  }

  public setToken(newToken: string): void {
      localStorage.setItem(this.TOKEN, newToken);
  }

  public parseJwt(token): any {
      if (this.jwt == null) {
          var base64Url = token.split('.')[1];
          var base64 = base64Url.replace('-', '+').replace('_', '/');
          this.jwt=JSON.parse(window.atob(base64));
      }
      return this.jwt;
      
  };

  public getRole(): string {
      return this.parseJwt(this.getToken()).role;
  }

  public isAdmin(): boolean {
      return this.getRole().toLowerCase() === "admin";
  }

  public isVerwalter(): boolean {
      return this.getRole().toLowerCase() === "verwalter";
  }

  public getUid(): string {
      return this.parseJwt(this.getToken()).uid;
  }

  public getName(): string {
      return this.parseJwt(this.getToken()).name;
  }

  public removeToken(): void {
      localStorage.removeItem(this.TOKEN);
      this.jwt = null;
  }

    //checks if token is valid
  public checkToken(): boolean {
      return !!this.getToken();
  }

    public isLoggedInAs(): boolean {
        return this.parseJwt(this.getToken()).isLoggedInAs == '1';
    }
}
