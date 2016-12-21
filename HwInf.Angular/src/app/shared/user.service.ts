import { Injectable } from '@angular/core';
import {JwtHttpService} from "./jwt-http.service";

@Injectable()
export class UserService {

  constructor(private http: JwtHttpService) { }

}
