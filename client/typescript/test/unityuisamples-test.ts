import assert from 'assert';
import { By, GamiumClient, KeyBy, NodeGamiumService, Player, RpcBy, UnityKeyCode } from 'gamium';
import { test } from './functions';

(async () => {
  const gamiumService = new NodeGamiumService('127.0.0.1', 50061);
  await gamiumService.connect();
  const gamium = new GamiumClient(gamiumService);
  const ui = gamium.ui();

  await test('RpcMethod test', async () => {
    let ret = await gamium.executeRpc(RpcBy.method('Gamium.Private.CodebaseSample', 'CallStringParam1', "asdf"));
    assert.strictEqual(ret, undefined);
    ret = await gamium.executeRpc(RpcBy.method('Gamium.Private.CodebaseSample', 'CallEmptyParam'));
    assert.strictEqual(ret, undefined);
    ret = await gamium.executeRpc(RpcBy.method('Gamium.Private.CodebaseSample', 'CallParam1', 10));
    assert.strictEqual(ret, undefined);
    ret = await gamium.executeRpc(RpcBy.method('Gamium.Private.CodebaseSample', 'CallParam2', 10, 10.0));
    assert.strictEqual(ret, undefined);
    ret = await gamium.executeRpc(RpcBy.method('Gamium.Private.CodebaseSample', 'CallParamReturn', 10, 10.1));
    assert.strictEqual(ret, 1);
    ret = await gamium.executeRpc(RpcBy.method('Gamium.Private.CodebaseSample', 'CallParamReturn2', 3));
    assert.strictEqual(ret, 3);
    ret = await gamium.executeRpc(RpcBy.method('Gamium.Private.CodebaseSample', 'CallParamReturn2', 1234567890));
    assert.strictEqual(ret, 1234567890);
    ret = await gamium.executeRpc(RpcBy.method('Gamium.Private.CodebaseSample', 'CallParamReturn2', 1234567890.0123));
    assert.strictEqual(ret, 1234567890.0123);
    ret = await gamium.executeRpc(RpcBy.method('Gamium.Private.CodebaseSample', 'CallParamReturn3', 10, 10.1));
    assert.deepStrictEqual(ret, {
      double1: 0.123456,
      double2: 1,
      double3: 1000,
      double4: 1234567890,
      double5: 1234567890.12345,
      intMax: 2147483647,
      intMin: -2147483648,
      hello: 'nice',
      nestedDict: { hello: 'nice2' },
      nestedArray: ['hello', 'nice2'],
    });
  });

  await test('RpcField test', async () => {
    let ret = await gamium.executeRpc(RpcBy.field('Gamium.Private.CodebaseSample', 'field1'));
    assert.strictEqual(ret, 10.0);
    ret = await gamium.executeRpc(RpcBy.field('Gamium.Private.CodebaseSample', 'field2'));
    assert.deepStrictEqual(ret, [1, 2]);
    ret = await gamium.executeRpc(RpcBy.field('Gamium.Private.CodebaseSample', 'field3'));
    assert.deepStrictEqual(ret, { hello: 'nice' });
  });

  await test('RpcProperty test', async () => {
    let ret = await gamium.executeRpc(RpcBy.property('Gamium.Private.CodebaseSample', 'property1'));
    assert.strictEqual(ret, 0);
    ret = await gamium.executeRpc(RpcBy.property('Gamium.Private.CodebaseSample', 'property2'));
    assert.deepStrictEqual(ret, []);
  });

  await test('Click Test', async () => {
    await ui.click(By.path('/Canvas[1]/MainMenu[1]/Window[1]/Settings[1]'));
    await ui.click(By.path('/Canvas[1]/Settings[1]/SettingsWindow[1]/Video[1]'));
    await ui.click(By.path('/Canvas[1]/Settings[1]/SettingsPanels[1]/VideoWindow[1]/Panel[1]/Settings[1]/Anti-Aliasing toggle[1]/Checkmark[1]'));
    await ui.click(By.path('/Canvas[1]/Settings[1]/SettingsPanels[1]/VideoWindow[1]/Panel[1]/Settings[1]/AO Toggle[1]/Checkmark[1]'));
    await ui.click(By.path('/Canvas[1]/Settings[1]/SettingsPanels[1]/VideoWindow[1]/Panel[1]/Close[1]'));
    await ui.click(By.path('/Canvas[1]/Settings[1]/SettingsWindow[1]/Back[1]'));
    await ui.click(By.path('/Canvas[1]/MainMenu[1]/Window[1]/NextScene[1]'));
  });

  await test('Drag Test', async () => {
    await ui.find(By.path('/Canvas[1]/Panel Top[1]/Drag Box[1]/Drag Image[1]'));
    const srcElem = await ui.find(By.path('/Canvas[1]/Panel Top[1]/Drag Box[1]/Drag Image[1]'));
    const src2Elem = await ui.find(By.path('/Canvas[1]/Panel Top[1]/Drag Box[2]/Drag Image[1]'));
    const destElem = await ui.find(By.path('/Canvas[1]/Panel 1[1]/Drop Box[1]/Drop Image[1]'));
    const dest2Elem = await ui.find(By.path('/Canvas[1]/Panel 2[1]/Drop Box[1]/Drop[1]'));
    await srcElem.waitInteractable();
    await src2Elem.waitInteractable();
    await destElem.waitInteractable();
    await dest2Elem.waitInteractable();
    await srcElem.drag(destElem, { durationMs: 1000, intervalMs: 100 });
    await src2Elem.drag(dest2Elem, { durationMs: 1000, intervalMs: 100 });

    await ui.click(By.path('/Canvas[1]/NextScene[1]'));
  });

  await test('Controls Test', async () => {
    const handle = await ui.find(By.path('/Canvas[1]/Panel[1]/Slider[1]/Handle Slide Area[1]/Handle[1]'));
    await handle.waitInteractable();
    await handle.drag({ x: handle.info.screenPosition.x - 30, y: handle.info.screenPosition.y }, { durationMs: 1000, intervalMs: 100 });

    const toggles = [
      '/Canvas[1]/Panel[1]/Toggle[1]/Label[1]',
      '/Canvas[1]/Panel[1]/ToggleGroup[1]/OptionA[1]',
      '/Canvas[1]/Panel[1]/ToggleGroup[1]/OptionB[1]',
      '/Canvas[1]/Panel[1]/ToggleGroup[1]/OptionC[1]',
    ];
    for (const toggleName of toggles) {
      await ui.click(By.path(toggleName));
    }

    const dropDowns = ['/Canvas[1]/Panel[1]/Dropdown[1]/Label[1]', '/Canvas[1]/Panel[1]/Dropdown[1]/Dropdown List[1]/Viewport[1]/Content[1]/Item 1: Option 2[1]'];
    for (const dropDownName of dropDowns) {
      await ui.click(By.path(dropDownName));
    }

    await ui.click(By.path('/Canvas[1]/Panel[1]/InputField[1]/Text[1]'));
    await ui.setText(By.path('/Canvas[1]/Panel[1]/InputField[1]/Text[1]'), 'Hello World');

    await ui.click(By.path('/Canvas[1]/Panel[1]/InputArea[1]/Text[1]'));
    await ui.setText(By.path('/Canvas[1]/Panel[1]/InputArea[1]/Text[1]'), 'Hello World\nasdf 안녕 반가워');

    await ui.click(By.path('/Canvas[1]/Panel[1]/Button[1]'));

    const scroll = await ui.find(By.path('/Canvas[1]/Scroll View[1]/Viewport[1]/Content[1]'));
    await scroll.waitInteractable();
    await scroll.scroll({ x: 0, y: -0.1 }, { durationMs: 300 });
    await scroll.scroll({ x: 0, y: 1 });

    await ui.click(By.path('/Canvas[1]/NextScene[1]'));
  });

  await test('keycode test', async () => {
    const keys = Object.keys(UnityKeyCode).filter((v) => isNaN(Number(v)));
    const title = await ui.find(By.path('/Canvas[1]/Window[1]/SF Title[1]/TitleLabel[1]'));

    for (const key of keys) {
      await gamium.sendKey(KeyBy.unityKeycode(key as keyof typeof UnityKeyCode), { duratiomMs: 30 });
      const text = await title.getText();
      assert.equal(UnityKeyCode[text], UnityKeyCode[key], `fail ${key}`);
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
