import { browser, element, by } from 'protractor';

export class HwInfPage {
  navigateTo() {
    return browser.get('/');
  }

  getParagraphText() {
    return element(by.css('hw-inf-root h1')).getText();
  }
}
