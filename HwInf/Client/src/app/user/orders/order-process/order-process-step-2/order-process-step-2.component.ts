import {Component, OnInit, OnDestroy} from '@angular/core';
import {OrderProcessService} from "../shared/order-process.service";
import {Router} from "@angular/router";
import {OrderFormDataService} from "../shared/order-form-data.service";
import {Order} from "../../../../shared/models/order.model";
import {Observable} from "rxjs";
import {CartService} from "../../../../shared/services/cart.service";
import {Device} from "../../../../shared/models/device.model";
import {OrderService} from "../../../../shared/services/order.service";
import { OrderItem } from "../../../../shared/models/order-item.model";
import { Overlay } from 'angular2-modal';
import { Modal } from 'angular2-modal/plugins/bootstrap';

@Component({
  selector: 'hwinf-order-process-step-2',
  templateUrl: './order-process-step-2.component.html',
  styleUrls: ['./order-process-step-2.component.scss']
})
export class OrderProcessStep2Component implements OnInit {

  private order: Order;
  private devices: Device[];
  constructor(
      private orderProcessService: OrderProcessService,
      private orderFormDataService: OrderFormDataService,
      private orderService: OrderService,
      private cartService: CartService,
      private router: Router,
      public modal: Modal
  ) { }

  ngOnInit() {

    if (!!this.orderFormDataService.getData().From) {
      this.onBack();
    }


    this.order = this.orderFormDataService.getData();
    this.devices = this.cartService.getItems();

    this.order.OrderItems = this.devices.map(i => {
      return new OrderItem(null, null, i);
    })
  }

  onBack() {
    this.router.navigate(['/anfrage/schritt-1']);
  }

  onContinue() {

    let x = ({
      From: this.order.From,
      To: this.order.To,
      OrderItems: [] = this.devices.map(device => {
        return ({Device: device});
      }),
      OrderReason: this.order.OrderReason
    });

   this.orderService.createOrder(x).subscribe(
        () => { this.router.navigate(['/anfrage/schritt-3']); },
        (err) => {console.log(err)}
    );

  }

  public showContract() {
      this.modal.alert()
          .size('lg')
          .showClose(true)
          .keyboard(27)
          .title('Vertragsbedingungen')
          .body(`<h1>§ 1 Vertragsgegenstand </h1>
                <p>(1) Gegenstand des Vertrages mit Zubehör an den/die EntleiherIn für die befristete Nutzung auch außerhalb der Zeiten der Lehrveranstaltungen nach § 6 Absatz 1 dieses Leihvertrages. </p>
                <p>Das Zubehör ist in der als Anlage zu diesem Vertrag beigefügten Bestandsliste aufgeführt. Die Bestandsliste ist Bestandteil dieses Vertrages. Entleiherin oder Entleiher können nur Studierende der FH Technikum Wien sein. Ein Anspruch auf Abschluss oder Verlängerung des Leihvertrages besteht nicht. </p>
                <p>(2) Die Leihe erfolgt im Rahmen der Ausbildung zur Unterstützung bei Projekten oder als befristete Übergangslösung.</p>
                <h1>§ 2 Vertragsgemäßer Gebrauch, Ausschluss von der Nutzung </h1>
                <p>(1) Das Gerät wird ausschließlich für Zwecke von Forschung, Lehre und Studium entliehen. Eine Nutzung für gewerbliche, erwerbswirtschaftliche, private oder sonstige Zwecke, die mit dem in § 1 Abs. 2 genannten Ausbildungszweck nicht vereinbar sind, ist untersagt.</p>
                <p>(2) Eine Weitergabe bzw. Überlassung des Gebrauchs des entliehenen Gerätes an Dritte ist untersagt. Technische Ressourcen des Gerätes dürfen Dritten nicht zugänglich gemacht werden. Die auf dem Gerät installierten Programme und Software dürfen vom/von der EntleiherIn nicht kopiert werden. Der/die EntleiherIn erhält ein vorinstalliertes System und muss das System während der Leihzeit selbst administrieren. Es bestehen keine Ansprüche auf regelmäßige Wartung des Systems seitens der VerleiherIn. Eingriffe in die Hardware bzw. die Ausstattung des Gerätes durch den/die EntleiherIn sind nicht gestattet. Solange der/die EntleiherIn das Gerät ausgeliehen hat, darf er/sie eigene legal erworbene Software installieren, soweit diese zur Erfüllung des Ausbildungszweckes erforderlich ist. Die VerleiherIn übernimmt für diesen Fall jedoch keine Gewähr für die Nutzbarkeit des Geräts. </p>
                <p> (3) Die VerleiherIn kann den/die EntleiherIn von der weiteren Nutzung des Gerätes ausschließen, wenn dieser/diese schuldhaft seinen/ihren Pflichten aus dieser Vereinbarung nicht nachkommt, das Gerät für strafbare Handlungen missbraucht oder der Fachhochschule Technikum Wien durch sonstiges rechtswidriges Nutzerverhalten Nachteile entstehen. </p>
                <h1>§ 3 Ausgabe und Rückgabe des Gerätes</h1>
                <p>Das Gerät nebst Zubehör wird von dem in dieser Vereinbarung genannten Institut ausgegeben. Bei diesem Institut ist das Gerät nebst Zubehör auch wieder zurückzugeben. </p>
                <h1>§ 4 Mängel, Sorgfaltspflichten und Haftung </h1>
                <p>(1) Der/die EntleiherIn hat sich bei der Übergabe von dem ordnungsgemäßen Zustand des Gerätes zu überzeugen und festgestellte Mängel und Schäden oder fehlendes Zubehör unverzüglich dem Fachbereich anzuzeigen. Unterlässt der/die EntleiherIn die Anzeige, so gilt das Gerät als in mangelfreiem Zustand und mit dem genannten Zubehör übergeben, es sei denn, dass es sich um einen Mangel handelt, der bei der Untersuchung nicht erkennbar war. Zeigt sich später ein solcher Mangel, so muss die Anzeige unverzüglich schriftlich an Herrn Benedikt Salzbrunn (salzbrunn@technikum-wien.at) bzw. dessen Vertretung gemacht werden, anderenfalls ist der Einwand ausgeschlossen, ein mangelhaftes Gerät erhalten zu haben. </p>
                <p>(2) Der/die EntleiherIn verpflichtet sich zur sorgfältigen Behandlung des Gerätes und des Zubehörs sowie dieses vor Verlusten (auch Diebstahl) und Beschädigungen zu schützen. </p>
                <p>(3) Der/die EntleiherIn hat Verluste sowie alle Mängel und Schäden des Gerätes, die während der Leihzeit auftreten, unverzüglich dem ausgebenden Fachbereich zu melden. Auf Verlangen ist ein schriftlicher Schadensbericht vorzulegen. Der Diebstahl des Gerätes ist darüber hinaus unverzüglich der Polizei anzuzeigen. Die Reparaturabwicklung bei aufgetretenen Mängeln und Schäden des Gerätes erfolgt ausschließlich über die IT-Abteilung des Technikum Wien. Der/die EntleiherIn darf Reparaturen weder selbst durchführen noch in Auftrag geben. </p>
                <p>(4) Der/die EntleiherIn haftet der VerleiherIn für die von ihm/ihr schuldhaft verursachten Schäden an der Leihgabe sowie für alle sonstigen Schäden, die dadurch entstehen, dass der/die EntleiherIn schuldhaft seinen/ihren Pflichten aus dieser Vereinbarung nicht nachkommt. Dem/der EntleiherIn obliegt der Beweis, dass ein schuldhaftes Verhalten nicht vorgelegen habe. Für Schäden an der Leihgabe, die durch vertragswidrigen Gebrauch entstehen, haftet der/die EntleiherIn unabhängig vom Verschulden.</p>
                <h1>§ 6 Laufzeit und Ende des Vertrages </h1>
                <p>(1) Das Gerät wird für die Zeit vom ausgewählten Datuem entliehen</p>
                <p>Der/die EntleiherIn ist verpflichtet, das Gerät während dieser Zeit der VerleiherIn auf deren Verlangen jederzeit vorzuzeigen. Die VerleiherIn ist berechtigt, die Leihe jederzeit zu kündigen. </p>
                <p>(2) Der Vertrag endet jedenfalls unabhängig von dem in Absatz (1) genannten Termin:  a) wenn der/die EntleiherIn nicht mehr Studierende(r) der FH Technikum Wien ist,  b) in den Fällen des § 2 Absatz 3 dieses Leihvertrages.</p>
                <p>(3) Mit dem Ende des Vertrages ist das Gerät nebst Zubehör unaufgefordert und unverzüglich zurückzugeben. </p>
                <h1>§ 7 Nebenbestimmungen </h1>
                <p>(1) Änderungen und Ergänzungen dieser Vereinbarung sowie Nebenabreden bedürfen der Schriftform.</p>
                <p>(2) Sollte eine Bestimmung dieses Vertrages unwirksam oder undurchführbar sein, beeinträchtigt dies nicht die Geltung der übrigen Bestimmungen dieses Vertrages. Die Vertragsparteien werden sich in einem solchen Fall bemühen, die unwirksame oder undurchführbare Bestimmung durch eine andere zu ersetzen, die der zu ersetzenden Bestimmung möglichst nahe kommt.</p>
                <p>(3) Es gilt österreichisches Recht, Gerichtsstand ist das sachlich zuständige Gericht in Wien. </p>`)
          .open();
  }
}
