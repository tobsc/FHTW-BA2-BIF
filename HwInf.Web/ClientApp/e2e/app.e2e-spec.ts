import { ItpPage } from './app.po';

describe('itp App', function() {
  let page: ItpPage;

  beforeEach(() => {
    page = new ItpPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
