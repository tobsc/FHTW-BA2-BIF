import { Component, OnInit } from '@angular/core';
import { PubSubService } from '../../shared/services/pub-sub.service';
@Component({
  selector: 'hwinf-loading-incidcator',
  templateUrl: './loading-incidcator.component.html',
  styleUrls: ['./loading-incidcator.component.scss']
})
export class LoadingIncidcatorComponent implements OnInit {

    private showLoader: boolean = false;

  constructor(private pubsub: PubSubService) { }

  ngOnInit() {
      this.pubsub.beforeRequest.subscribe(data => this.showLoader = true);
      this.pubsub.afterRequest.subscribe(data => this.showLoader = false); 
  }

}
