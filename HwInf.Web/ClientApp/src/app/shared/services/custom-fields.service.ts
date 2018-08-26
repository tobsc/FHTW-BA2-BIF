import { Injectable } from '@angular/core';
import {FieldGroup} from "../models/fieldgroup.model";
import {Observable} from "rxjs";
import { Response } from "@angular/http";
import {HttpClient} from "@angular/common/http";
import { HttpHeaders } from "@angular/common/http";
import "rxjs";

@Injectable()
export class CustomFieldsService {

  constructor(
      public http: HttpClient
  ) { }

  public addFieldGroup(body: FieldGroup): Observable<FieldGroup> {
      let bodyString = JSON.stringify(body);
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post<FieldGroup>('/api/customfields/fieldgroups', bodyString, { headers });
  }


  public getFieldGroups(): Observable<FieldGroup[]> {
    return this.http.get<FieldGroup[]>('/api/customfields/fieldgroups');
  }

  public getFieldGroupsOfType ( deviceTypeSlug: string = ''): Observable<FieldGroup[]> {
    return this.http.get<FieldGroup[]>('/api/customfields/fieldgroups/' + deviceTypeSlug);
  }


  public getFieldGroupsOfTypeForFilter ( deviceTypeSlug: string = ''): Observable<FieldGroup[]> {
    return this.http.get<FieldGroup[]>('/api/customfields/filter/fieldgroups/' + deviceTypeSlug);
  }

  public editFieldGroup(body: FieldGroup): Observable<FieldGroup> {
      let bodyString = JSON.stringify(body);
      console.log(bodyString);
      let headers = new HttpHeaders({
          'Content-Type': 'application/json'
      });
    return this.http.put<FieldGroup>('/api/customfields/fieldgroups/' + body.Slug, bodyString, { headers });
  }

  public deleteFieldGroup(slug: string) {
      return this.http.delete('/api/customfields/fieldgroups/' + slug);
  }
}
