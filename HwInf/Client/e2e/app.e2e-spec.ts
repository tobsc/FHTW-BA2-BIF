import { HwInfPage } from './app.po';

describe('hw-inf App', function() {
  let page: HwInfPage;

  beforeEach(() => {
    page = new HwInfPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('hw-inf works!');
  });
});
