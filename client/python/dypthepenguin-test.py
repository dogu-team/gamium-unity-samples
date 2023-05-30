import uuid
from gamium import *
import asyncio
import sys


gamium = GamiumClient("127.0.0.1", 50061)
gamium.connect()
ui = gamium.ui()

player = gamium.player(By.path("/Dyp[1]/Dyp[1]/Root[1]"))
cameraLocator = By.path("/Cameras[1]/Main Camera[1]")


def pick_first_snowball():
    for index in range(3):
        snowBall1Locator = By.path("/World[1]/Item Pickups[1]/SnowBallPickup[1]")
        snowBall = ui.find(snowBall1Locator)
        snowBall.wait_interactable()
        player.move(cameraLocator, snowBall1Locator)
        gamium.send_key(KeyBy.unity_keycode("E"))

        snowBall2Locator = By.path("/World[1]/Item Pickups[1]/SnowBallPickup (1)[1]")
        snowBall2 = ui.find(snowBall2Locator)
        snowBall2.wait_interactable()
        player.move(cameraLocator, snowBall2Locator)
        gamium.send_key(KeyBy.unity_keycode("E"))


pick_first_snowball()


def pick_fish():
    player.move(cameraLocator, Vector3(-1.74, 0.11, 0.07))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, By.path("/World[1]/Item Pickups[1]/FishPickup (1)[1]"))
    gamium.send_key(KeyBy.unity_keycode("E"))
    gamium.send_key(KeyBy.unity_keycode("Alpha2"))


pick_fish()


def kill_enemy1():
    player.move(cameraLocator, Vector3(-19.66, -5.7, 28.51))
    player.move(cameraLocator, Vector3(-20.63, -5.7, 32.98))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(-24.56, -0.7, 41.63))
    gamium.send_keys([KeyBy.unity_keycode("W"), KeyBy.unity_keycode("A")], SendKeyOptions(300))
    gamium.send_key(KeyBy.unity_keycode("LeftControl"))
    gamium.sleep(300)
    gamium.send_key(KeyBy.unity_keycode("LeftControl"))


kill_enemy1()


def move():
    player.move(cameraLocator, Vector3(-13.83, -5.76, 21.76))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(-3.22, -2.37, 6.04))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(2.59, 0.11, 3.84))


move()


def ride_movingplatform1():
    player.move(cameraLocator, Vector3(4.88, 0.11, 0.84))
    player.move(
        cameraLocator, By.path("/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/MovingPlatform[1]/M_Cylinder_Rounded[1]")
    )
    player.move(
        cameraLocator,
        By.path(
            "/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/MovingPlatform[1]/Platform Mover Interactable[1]/Crystal[1]"
        ),
        MovePlayerOptions(epsilon=1.6),
    )
    gamium.send_key(KeyBy.unity_keycode("E"))
    while True:
        if player.is_near(
            By.path(
                "/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/Mover Interactables[1]/Platform Mover Interactable (2)[1]/Crystal[1]"
            ),
            10,
        ):
            break
        gamium.sleep(100)
    gamium.sleep(2000)


ride_movingplatform1()


def kill_enemy2():
    player.move(cameraLocator, Vector3(20.88, -3.89, 24.6))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(26.59, -2.29, 33.18))
    gamium.send_keys([KeyBy.unity_keycode("W"), KeyBy.unity_keycode("D")], SendKeyOptions(300))
    gamium.send_key(KeyBy.unity_keycode("LeftControl"))
    gamium.sleep(300)
    gamium.send_key(KeyBy.unity_keycode("LeftControl"))


kill_enemy2()


def ride_movingplatform2():
    player.move(
        cameraLocator, By.path("/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/MovingPlatform[1]/M_Cylinder_Rounded[1]")
    )
    player.move(
        cameraLocator,
        By.path(
            "/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/MovingPlatform[1]/Platform Mover Interactable[1]/Crystal[1]"
        ),
        MovePlayerOptions(epsilon=1.6),
    )
    gamium.send_key(KeyBy.unity_keycode("E"))
    while True:
        if player.is_near(
            By.path(
                "/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent[1]/Mover Interactables[1]/Platform Mover Interactable (1)[1]/Crystal[1]"
            ),
            10,
        ):
            break
        gamium.sleep(100)
    gamium.sleep(2000)


ride_movingplatform2()


def jump_platform():
    player.move(cameraLocator, Vector3(9.15, 0.11, -0.99))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(19.14, -0.18, -0.19))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(29.34, -0.18, -0.35))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(39.33, -0.18, -0.67))
    gamium.send_key(KeyBy.unity_keycode("Space"))


jump_platform()


def kill_enemy3():
    for index in range(2):
        player.move(cameraLocator, Vector3(57.1, 0.11, 0.69))
        player.move(cameraLocator, Vector3(47.74, 0.11, 1.34))
        gamium.send_keys([KeyBy.unity_keycode("W"), KeyBy.unity_keycode("D")], SendKeyOptions(500))
        gamium.send_key(KeyBy.unity_keycode("LeftControl"))
    player.move(cameraLocator, Vector3(45.05, 0.11, 0.12))


kill_enemy3()


def jump_platform_back():
    player.move(cameraLocator, Vector3(44.11, -0.17, 0.22))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(33.91, -0.18, -0.78))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(23.87, -0.18, -0.56))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(14.1, -0.18, -0.91))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(6.84, 0.11, -0.45))


jump_platform_back()


def go_to_pick_key():
    player.move(cameraLocator, Vector3(-1.81, -2.37, 15.22))
    player.move(cameraLocator, Vector3(8.35, -8.8, 35.16))
    player.move(cameraLocator, Vector3(-3.17, -9, 45.8))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(
        cameraLocator,
        By.path("/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent (1)[1]/MovingPlatform[1]/M_Cylinder_Rounded[1]"),
    )
    player.move(
        cameraLocator,
        By.path(
            "/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent (1)[1]/MovingPlatform[1]/Platform Mover Interactable[1]/Crystal[1]"
        ),
        MovePlayerOptions(epsilon=1.6),
    )
    gamium.send_key(KeyBy.unity_keycode("E"))
    while True:
        if player.is_near(
            By.path(
                "/World[1]/World Environment[1]/Dynamic[1]/Moving Platforms[1]/MovingPlatform Parent (1)[1]/Mover Interactables[1]/Platform Mover Interactable (2)[1]/Crystal[1]",
            ),
            10,
        ):
            break
        gamium.sleep(100)
    gamium.sleep(2000)

    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, By.path("/World[1]/Item Pickups[1]/Special Pick Axe Pickup[1]"))
    gamium.send_key(KeyBy.unity_keycode("E"))


go_to_pick_key()


def go_to_save_penguin():
    player.move(cameraLocator, Vector3(6.29, -8.8, 36.16))
    player.move(cameraLocator, Vector3(-0.47, -8.8, 35.29))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(-15.06, -5.7, 31.77))
    player.move(cameraLocator, Vector3(-10.58, -5.78, 23.14))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(-2.55, -2.37, 8.21))
    gamium.send_key(KeyBy.unity_keycode("Space"))
    player.move(cameraLocator, Vector3(2.02, 0.11, 1.58))
    player.move(cameraLocator, Vector3(-1.36, 0.11, 0.61))
    gamium.send_key(KeyBy.unity_keycode("Space"))


go_to_save_penguin()


def save_penguin():
    player.move(
        cameraLocator,
        By.path("/World[1]/World Environment[1]/Interactables[1]/Shackled Nice Penguin[1]/Nice Dyp[1]/penguin[1]"),
        MovePlayerOptions(epsilon=2.5),
    )
    gamium.send_key(KeyBy.unity_keycode("E"))
    player.move(cameraLocator, Vector3(-12.61, 3.56, -1.43), MovePlayerOptions(epsilon=1.3))
    gamium.send_keys([KeyBy.unity_keycode("W"), KeyBy.unity_keycode("A")], SendKeyOptions(100))
    gamium.send_key(KeyBy.unity_keycode("LeftControl"))
    gamium.sleep(1000)
    player.move(cameraLocator, Vector3(-8.8, 3.56, 3.53), MovePlayerOptions(epsilon=1.3))
    gamium.send_keys([KeyBy.unity_keycode("W"), KeyBy.unity_keycode("A")], SendKeyOptions(100))
    gamium.send_key(KeyBy.unity_keycode("LeftControl"))


save_penguin()


def quit():
    gamium.sleep(4000)
    gamium.actions().app_quit().perform()

    sys.exit(0)


quit()
