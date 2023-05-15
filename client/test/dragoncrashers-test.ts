import { By, GamiumClient, MovePlayerBy, UI, Until, NodeGamiumService } from 'gamium';
import { test } from './functions';
import assert from 'assert';

(async () => {
  const gamiumService = new NodeGamiumService('127.0.0.1', 50061);
  await gamiumService.connect();
  const gamium = new GamiumClient(gamiumService);
  const ui = gamium.ui();

  await test('Reset PlayerData', async () => {
    const buttonPaths = [
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/OptionsBar[1]/options-bar[1]/options-bar__button[1]',

      // reset
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-left[1]/settings-account[1]/settings__social-button1[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-left[1]/settings-account[1]/settings__social-button2[1]',
    ];

    for (const path of buttonPaths) {
      await ui.click(By.path(path));
    }
  });

  await test('Get/Set Text', async () => {
    const textElemPath =
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-left[1]/settings-player-settings[1]/settings__player-textfield[1]/unity-text-input[1]';
    const uuid = crypto.randomUUID();
    await ui.setText(By.path(textElemPath), uuid);
    const textValue = await ui.getText(By.path(textElemPath));
    assert.equal(textValue, uuid);
  });

  await test('Drag, Toggle. Radio', async () => {
    const dropdownPaths = [
      '/MainMenuPanelSettings/MainMenu-container/SettingsScreen/settings-screen-overlay/settings-screen-background/settings__panel/settings__panel-body/settings__panel-body-right/settings__dropdown-container/settings__language-dropdown',
      '/MainMenuPanelSettings[1]/[1]/[1]/[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]',
    ];

    for (const path of dropdownPaths) {
      await ui.click(By.path(path));
    }
    const musicDragger = await ui.find(
      By.path(
        '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__slider1-container[1]/settings__slider1-drag-area[1]/settings__slider1[1]/[1]/unity-drag-container[1]/unity-dragger[1]',
      ),
    );
    await musicDragger.waitInteractable();
    await musicDragger.drag({ x: musicDragger.info.screenPosition.x + 50, y: musicDragger.info.screenPosition.y });
    await musicDragger.drag({ x: musicDragger.info.screenPosition.x - 100, y: musicDragger.info.screenPosition.y });

    const sfxDragger = await ui.find(
      By.path(
        '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__slider2-container[1]/settings__slider2-drag-area[1]/settings__slider2[1]/[1]/unity-drag-container[1]/unity-dragger[1]',
      ),
    );
    await sfxDragger.waitInteractable();
    await sfxDragger.drag({ x: sfxDragger.info.screenPosition.x + 50, y: sfxDragger.info.screenPosition.y });
    await sfxDragger.drag({ x: sfxDragger.info.screenPosition.x - 100, y: sfxDragger.info.screenPosition.y });

    const otherButtonPaths = [
      // toggle
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__slide-toggle-container[1]/settings-notifications__toggle[1]/[2]/[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__slide-toggle-container[1]/settings-notifications__toggle[1]/[2]/[1]',

      // radio
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__radio-button-container[1]/settings__graphics-radio-button-group[1]/[1]/[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__radio-button-container[1]/settings__graphics-radio-button-group[1]/[1]/[2]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__radio-button-container[1]/settings__graphics-radio-button-group[1]/[1]/[3]',

      // back
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings-header[1]/settings__panel-back-button[1]',
    ];
    for (const path of otherButtonPaths) {
      await ui.click(By.path(path));
    }
  });

  await test('Scroll', async () => {
    await ui.click(By.path('/MainMenuPanelSettings[1]/MainMenu-container[1]/OptionsBar[1]/options-bar[1]/options-bar__gold-currency-group[1]/options-bar__gold-button[1]'));

    const dragger = await ui.find(
      By.path(
        '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gold-shopcontent[1]/shop__gold-scrollview[1]/[1]/unity-slider[1]/[1]/unity-drag-container[1]/unity-dragger[1]',
      ),
    );
    await dragger.waitInteractable();
    await dragger.scroll({ x: 0, y: -0.5 }, { durationMs: 2000 });
    await dragger.scroll({ x: 0, y: 0.5 }, { durationMs: 2000 });
  });

  await test('Buy', async () => {
    const buttonPaths = [
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/OptionsBar[1]/options-bar[1]/options-bar__gem-currency-group[1]/options-bar__gem-button[1]',

      // buy gem
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',

      // buy potion
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-screen-tabs[1]/shop-potion-shoptab[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-potion-shopcontent[1]/shop__potion-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[1]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-potion-shopcontent[1]/shop__potion-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[1]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-potion-shopcontent[1]/shop__potion-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[1]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]',
    ];

    for (const path of buttonPaths) {
      await ui.click(By.path(path));
    }
  });

  await test('Level Up', async () => {
    const buttonPaths = [
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/MenuBar[1]/menu__background[1]/menu__button-group[1]/menu__char-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/CharScreen[1]/char-screen__anchor--center[1]/char__panel[1]/char__next-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/CharScreen[1]/char-screen__anchor--center[1]/char__panel[1]/char__panel-footer[1]/char__level-up-button[1]',
    ];

    for (const path of buttonPaths) {
      await ui.click(By.path(path));
    }
  });

  await test('Kill Monster', async () => {
    const buttonPaths = [
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/MenuBar[1]/menu__background[1]/menu__button-group[1]/menu__home-button[1]',
      '/MainMenuPanelSettings[1]/MainMenu-container[1]/HomeScreen[1]/home__panel[1]/home-play__level-panel[1]/home-play__level-frame[1]/home-play__level-button[1]',
    ];

    for (const path of buttonPaths) {
      await ui.click(By.path(path));
    }
  });
})().catch((e) => {
  console.error(e);
  process.exit(1);
});
