from gamium import *
import sys


gamium = GamiumClient("127.0.0.1", 50061)
gamium.connect()
ui = gamium.ui()


def mouse_keyboard_test():
    screen = gamium.screen()
    for _ in range(3):
        gamium.actions().click(Vector2(screen.width / 2, screen.height / 2)).perform()
        gamium.send_key(KeyBy.unity_keycode("W"), SendKeyOptions(300))
        gamium.send_key(KeyBy.unity_keycode("A"), SendKeyOptions(300))
        gamium.send_key(KeyBy.unity_keycode("S"), SendKeyOptions(300))
        gamium.send_key(KeyBy.unity_keycode("D"), SendKeyOptions(300))


mouse_keyboard_test()


def settings_test():
    gamium.send_key(KeyBy.unity_keycode("P"), SendKeyOptions(300))

    buttonPaths = [
        "/UI[1]/UI - Menu[1]/UI Menu - Canvas[1]/Panel_MiddleStrip[1]/Panel_Left[1]/VerticalLayout_ButtonOptions[1]/UI_Button_Option_Rebind[1]",
        "/UI[1]/UI - Menu[1]/UI Menu - Canvas[1]/Panel_MiddleStrip[1]/Panel_Left[1]/VerticalLayout_ButtonOptions[1]/UI_Button_Option_Quit[1]",
        "/UI[1]/UI - Menu[1]/UI Menu - Canvas[1]/Panel_MiddleStrip[1]/Panel_Left[1]/VerticalLayout_ButtonOptions[1]/UI_Button_Option_About[1]",
    ]

    for path in buttonPaths:
        ui.click(By.path(path))

    gamium.send_key(KeyBy.unity_keycode("P"), SendKeyOptions(300))


settings_test()


def keyboard_test():
    keys = []
    for attr_name in dir(UnityKeyboard):
        if not attr_name.startswith("__"):
            keys.append(attr_name)

    for key in keys:
        gamium.send_key(KeyBy.unity_keyboard(key))
        text = ui.get_text(By.path("/UI[1]/Canvas[1]/Text (TMP)[1]"))
        assert getattr(UnityKeyboard, text) == getattr(
            UnityKeyboard, key
        ), f"keyboard test failed: {getattr(UnityKeyCode, text)} != {getattr(UnityKeyCode, key)}"


keyboard_test()


def quit():
    gamium.actions().app_quit().perform()
    sys.exit(0)


quit()
