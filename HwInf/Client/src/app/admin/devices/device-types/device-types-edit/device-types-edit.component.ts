import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'hwinf-device-types-edit',
  templateUrl: './device-types-edit.component.html',
  styleUrls: ['./device-types-edit.component.scss']
})
export class DeviceTypesEditComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  private value: any[] = [];

  public items: Array<string> = ['Amsterdam', 'Antwerp', 'Athens', 'Barcelona',
    'Berlin', 'Birmingham', 'Bradford', 'Bremen', 'Brussels', 'Bucharest',
    'Budapest', 'Cologne', 'Copenhagen', 'Dortmund', 'Dresden', 'Dublin', 'Düsseldorf',
    'Essen', 'Frankfurt', 'Genoa', 'Glasgow', 'Gothenburg', 'Hamburg', 'Hannover',
    'Helsinki', 'Leeds', 'Leipzig', 'Lisbon', 'Łódź', 'London', 'Kraków', 'Madrid',
    'Málaga', 'Manchester', 'Marseille', 'Milan', 'Munich', 'Naples', 'Palermo',
    'Paris', 'Poznań', 'Prague', 'Riga', 'Rome', 'Rotterdam', 'Seville', 'Sheffield',
    'Sofia', 'Stockholm', 'Stuttgart', 'The Hague', 'Turin', 'Valencia', 'Vienna',
    'Vilnius', 'Warsaw', 'Wrocław', 'Zagreb', 'Zaragoza'];

  public refreshValue(value:any):void {
    this.value = value;
  }


  public foo(f) {
    console.log(this.value);
    this.value.map(i => i.text).forEach(i => console.log(i));
  }
}
