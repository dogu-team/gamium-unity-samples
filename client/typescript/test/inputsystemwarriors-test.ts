import assert from 'assert';
import { By, GamiumClient, KeyBy, NodeGamiumService, Player, UnityKeyboard } from 'gamium';
import { test } from './functions';

(async () => {
  const gamiumService = new NodeGamiumService('127.0.0.1', 50061);
  await gamiumService.connect();
  const gamium = new GamiumClient(gamiumService);
  const ui = gamium.ui();

  await test('Mouse Keyboard test', async () => {
    const screen = await gamium.screen();
    for (let index = 0; index < 3; index++) {
      await gamium
        .actions()
        .click({ x: screen.width / 2, y: screen.height / 2 })
        .perform();
      await gamium.sendKey(KeyBy.unityKeycode('W'), { duratiomMs: 300 });
      await gamium.sendKey(KeyBy.unityKeycode('A'), { duratiomMs: 300 });
      await gamium.sendKey(KeyBy.unityKeycode('S'), { duratiomMs: 300 });
      await gamium.sendKey(KeyBy.unityKeycode('D'), { duratiomMs: 300 });
    }
  });

  await test('Settings test', async () => {
    await gamium.sendKey(KeyBy.unityKeycode('P'), { duratiomMs: 300 });

    const buttonPaths = [
      '/UI[1]/UI - Menu[1]/UI Menu - Canvas[1]/Panel_MiddleStrip[1]/Panel_Left[1]/VerticalLayout_ButtonOptions[1]/UI_Button_Option_Rebind[1]',
      '/UI[1]/UI - Menu[1]/UI Menu - Canvas[1]/Panel_MiddleStrip[1]/Panel_Left[1]/VerticalLayout_ButtonOptions[1]/UI_Button_Option_Quit[1]',
      '/UI[1]/UI - Menu[1]/UI Menu - Canvas[1]/Panel_MiddleStrip[1]/Panel_Left[1]/VerticalLayout_ButtonOptions[1]/UI_Button_Option_About[1]',
    ];

    for (const path of buttonPaths) {
      await ui.click(By.path(path));
    }

    await gamium.sendKey(KeyBy.unityKeycode('P'), { duratiomMs: 300 });
  });

  await test('Keyboard test', async () => {
    const keys = Object.keys(UnityKeyboard).filter((v) => isNaN(Number(v)));

    for (const key of keys) {
      await gamium.sendKey(KeyBy.unityKeyboard(key as keyof typeof UnityKeyboard));
      const text = await ui.getText(By.path('/UI[1]/Canvas[1]/Text (TMP)[1]'));
      assert.deepStrictEqual(UnityKeyboard[text], UnityKeyboard[key], `fail ${UnityKeyboard[key]} is not equal to ${UnityKeyboard[text]}`);
    }
  });

  await test('Quit', async () => {
    await gamium.actions().appQuit().perform();
    process.exit(0);
  });
})().catch((e) => {
  console.error(e);
  process.exit(1);
});
