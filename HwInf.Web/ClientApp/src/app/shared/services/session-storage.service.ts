import { Injectable } from '@angular/core';

@Injectable()
export class SessionStorageService {

  public sortingSetting : string = 'createdate';
  public isAscending : boolean = true;
  public sortingDeviceType: string = '';

  constructor() {

    if (!!sessionStorage.getItem('sorting_setting')) {
      this.sortingSetting = sessionStorage.getItem('sorting_setting');
    }

    if (!!sessionStorage.getItem('sorting_isascending')) {
      this.isAscending = sessionStorage.getItem('sorting_isascending') == 'true';
    }

    if (!!sessionStorage.getItem('sorting_devicetype')) {
      this.sortingDeviceType = sessionStorage.getItem('sorting_devicetype');
    }

  }

  public getSortingSetting() {
     return this.sortingSetting;
  }

  public getSortingDeviceType() {
    return this.sortingDeviceType;
  }

  public getSortingIsAscending() {
    return this.isAscending;
  }

  public setSortingSetting(setting : string, isAscending: boolean) {
    this.sortingSetting = setting;
    this.isAscending = isAscending;
    sessionStorage.setItem('sorting_setting', setting);
    sessionStorage.setItem('sorting_isascending', isAscending.toString());
  }

  public setSortingDeviceType(deviceType : string) {
    this.sortingDeviceType = deviceType;
    sessionStorage.setItem('sorting_devicetype', deviceType);
  }
}
