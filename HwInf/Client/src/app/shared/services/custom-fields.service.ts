import { Injectable } from '@angular/core';
import {JwtHttpService} from "./jwt-http.service";

@Injectable()
export class CustomFieldsService {

  constructor(
      private http: JwtHttpService
  ) { }



}
