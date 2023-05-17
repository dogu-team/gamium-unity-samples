import { By, GamiumClient, KeyBy, NodeGamiumService, Player } from 'gamium';
import { test } from './functions';

(async () => {
  const gamiumService = new NodeGamiumService('127.0.0.1', 50061);
  await gamiumService.connect();
  const gamium = new GamiumClient(gamiumService);
  const ui = gamium.ui();
  const player = await gamium.player(By.path('/Dyp[1]/Dyp[1]/Root[1]'));
  const cameraLocator = By.path('/Cameras[1]/Main Camera[1]');

  await test('Pick First SnowBall', async () => {
    for (let index = 0; index < 3; index++) {
      const snowBall1Locator = By.path('/World[1]/Item Pickups[1]/SnowBallPickup[1]');
      const snowBall = await ui.find(snowBall1Locator);
      await snowBall.waitInteractable();
      await player.move(cameraLocator, snowBall1Locator);
      await gamium.sendKey(KeyBy.unityKeycode('E'));

      const snowBall2Locator = By.path('/World[1]/Item Pickups[1]/SnowBallPickup (1)[1]');
      const snowBall2 = await ui.find(snowBall2Locator);
      await snowBall2.waitInteractable();
      await player.move(cameraLocator, snowBall2Locator);
      await gamium.sendKey(KeyBy.unityKeycode('E'));
    }
  });

  await test('Pick Fish', async () => {
    await player.move(cameraLocator, { x: -1.74, y: 0.11, z: 0.07 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, By.path('/World[1]/Item Pickups[1]/FishPickup (1)[1]'));
    await gamium.sendKey(KeyBy.unityKeycode('E'));
    await gamium.sendKey(KeyBy.unityKeycode('Alpha2'));
  });

  await test('Kill Enemy1', async () => {
    await player.move(cameraLocator, { x: -19.66, y: -5.7, z: 28.51 });
    await player.move(cameraLocator, { x: -20.63, y: -5.7, z: 32.98 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: -24.56, y: -0.7, z: 41.63 });
    await gamium.sendKeys([KeyBy.unityKeycode('W'), KeyBy.unityKeycode('A')], { duratiomMs: 300 });
    await gamium.sendKey(KeyBy.unityKeycode('LeftControl'));
    await gamium.sleep(300);
    await gamium.sendKey(KeyBy.unityKeycode('LeftControl'));
  });

  await test('Move', async () => {
    await player.move(cameraLocator, { x: -13.83, y: -5.76, z: 21.76 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: -3.22, y: -2.37, z: 6.04 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 2.59, y: 0.11, z: 3.84 });
  });

  await test('Ride MovingPlatform1', async () => {
    await player.move(cameraLocator, { x: 4.88, y: 0.11, z: 0.84 });
    await player.move(cameraLocator, By.path('/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/MovingPlatform[1]/M_Cylinder_Rounded[1]'));
    await player.move(
      cameraLocator,
      By.path('/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/MovingPlatform[1]/Platform Mover Interactable[1]/Crystal[1]'),
      { epsilon: 1.6 },
    );
    await gamium.sendKey(KeyBy.unityKeycode('E'));
    for (let i = 0; i < Infinity; i++) {
      if (
        await player.isNear(
          By.path('/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/Mover Interactables[1]/Platform Mover Interactable (2)[1]/Crystal[1]'),
          { epsilon: 10 },
        )
      ) {
        break;
      }
      await gamium.sleep(100);
    }
    await gamium.sleep(2000);
  });

  await test('Kill Enemy2', async () => {
    await player.move(cameraLocator, { x: 20.88, y: -3.89, z: 24.6 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 26.59, y: -2.29, z: 33.18 });
    await gamium.sendKeys([KeyBy.unityKeycode('W'), KeyBy.unityKeycode('D')], { duratiomMs: 300 });
    await gamium.sendKey(KeyBy.unityKeycode('LeftControl'));
    await gamium.sleep(300);
    await gamium.sendKey(KeyBy.unityKeycode('LeftControl'));
  });

  await test('Ride MovingPlatform2', async () => {
    await player.move(cameraLocator, By.path('/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/MovingPlatform[1]/M_Cylinder_Rounded[1]'));
    await player.move(
      cameraLocator,
      By.path('/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/MovingPlatform[1]/Platform Mover Interactable[1]/Crystal[1]'),
      { epsilon: 1.6 },
    );
    await gamium.sendKey(KeyBy.unityKeycode('E'));
    for (let i = 0; i < Infinity; i++) {
      if (
        await player.isNear(
          By.path('/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/Mover Interactables[1]/Platform Mover Interactable (1)[1]/Crystal[1]'),
          { epsilon: 10 },
        )
      ) {
        break;
      }
      await gamium.sleep(100);
    }
    await gamium.sleep(2000);
  });

  await test('Jump Platform', async () => {
    await player.move(cameraLocator, { x: 9.15, y: 0.11, z: -0.99 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 19.14, y: -0.18, z: -0.19 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 29.34, y: -0.18, z: -0.35 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 39.33, y: -0.18, z: -0.67 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
  });

  await test('Kill Enemy3', async () => {
    for (let index = 0; index < 2; index++) {
      await player.move(cameraLocator, { x: 57.1, y: 0.11, z: 0.69 });
      await player.move(cameraLocator, { x: 47.74, y: 0.11, z: 1.34 });
      await gamium.sendKeys([KeyBy.unityKeycode('W'), KeyBy.unityKeycode('D')], { duratiomMs: 500 });
      await gamium.sendKey(KeyBy.unityKeycode('LeftControl'));
    }
    await player.move(cameraLocator, { x: 45.05, y: 0.11, z: 0.12 });
  });

  await test('Jump Platform back', async () => {
    await player.move(cameraLocator, { x: 44.11, y: -0.17, z: 0.22 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 33.91, y: -0.18, z: -0.78 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 23.87, y: -0.18, z: -0.56 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 14.1, y: -0.18, z: -0.91 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 6.84, y: 0.11, z: -0.45 });
  });

  await test('Go To Pick Key', async () => {
    await player.move(cameraLocator, { x: -1.81, y: -2.37, z: 15.22 });
    await player.move(cameraLocator, { x: 8.35, y: -8.8, z: 35.16 });
    await player.move(cameraLocator, { x: -3.17, y: -9, z: 45.8 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, By.path('/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent (1)[1]/MovingPlatform[1]/M_Cylinder_Rounded[1]'));
    await player.move(
      cameraLocator,
      By.path('/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent (1)[1]/MovingPlatform[1]/Platform Mover Interactable[1]/Crystal[1]'),
      { epsilon: 1.6 },
    );
    await gamium.sendKey(KeyBy.unityKeycode('E'));
    for (let i = 0; i < Infinity; i++) {
      if (
        await player.isNear(
          By.path(
            '/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent (1)[1]/Mover Interactables[1]/Platform Mover Interactable (2)[1]/Crystal[1]',
          ),
          { epsilon: 10 },
        )
      ) {
        break;
      }
      await gamium.sleep(100);
    }
    await gamium.sleep(2000);

    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, By.path('/World[1]/Item Pickups[1]/Special Pick Axe Pickup[1]'));
    await gamium.sendKey(KeyBy.unityKeycode('E'));
  });

  await test('Go to save Penguin', async () => {
    await player.move(cameraLocator, { x: 6.29, y: -8.8, z: 36.16 });
    await player.move(cameraLocator, { x: -0.47, y: -8.8, z: 35.29 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: -15.06, y: -5.7, z: 31.77 });
    await player.move(cameraLocator, { x: -10.58, y: -5.78, z: 23.14 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: -2.55, y: -2.37, z: 8.21 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
    await player.move(cameraLocator, { x: 2.02, y: 0.11, z: 1.58 });
    await player.move(cameraLocator, { x: -1.36, y: 0.11, z: 0.61 });
    await gamium.sendKey(KeyBy.unityKeycode('Space'));
  });

  await test('Save Penguin', async () => {
    await player.move(cameraLocator, By.path('/World[1]/World Environment[1]/Interactables[1]/Shackled Nice Penguin[1]/Nice Dyp[1]/penguin[1]'), {
      epsilon: 2.5,
    });
    await gamium.sendKey(KeyBy.unityKeycode('E'));
    await player.move(cameraLocator, { x: -12.61, y: 3.56, z: -1.43 }, { epsilon: 1.3 });
    await gamium.sendKeys([KeyBy.unityKeycode('W'), KeyBy.unityKeycode('A')], { duratiomMs: 100 });
    await gamium.sendKey(KeyBy.unityKeycode('LeftControl'));
    await gamium.sleep(1000);
    await player.move(cameraLocator, { x: -8.8, y: 3.56, z: 3.53 }, { epsilon: 1.3 });
    await gamium.sendKeys([KeyBy.unityKeycode('W'), KeyBy.unityKeycode('A')], { duratiomMs: 100 });
    await gamium.sendKey(KeyBy.unityKeycode('LeftControl'));
  });

  await test('Quit', async () => {
    await gamium.actions().appQuit().perform();
    process.exit(0);
  });
})().catch((e) => {
  console.error(e);
  process.exit(1);
});
