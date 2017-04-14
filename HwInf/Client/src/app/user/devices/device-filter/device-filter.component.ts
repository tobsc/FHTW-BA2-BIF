import {Component, OnInit, Input} from '@angular/core';
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";
import {DeviceType} from "../../../shared/models/device-type.model";
import {Observable, Subscription} from "rxjs";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'hwinf-device-filter',
  templateUrl: './device-filter.component.html',
  styleUrls: ['./device-filter.component.scss']
})
export class DeviceFilterComponent implements OnInit {

  @Input() deviceType: DeviceType;
  private fieldGroups: FieldGroup[];
  private sub: Subscription;

  constructor(
      private customFieldsService: CustomFieldsService,
      private route: ActivatedRoute
  ) { }

  ngOnInit() {

      this.sub = this.route.params.map( i => i['type']).flatMap( i => this.customFieldsService.getFieldGroupsOfType(i))
          .subscribe((data) => this.fieldGroups = data );
  }

}
