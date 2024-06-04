from gamium import *
import sys


service = TcpGamiumService("127.0.0.1", 50061)
gamium = GamiumClient(service)
gamium.connect()
ui = gamium.ui()


def rpc_method_test():
    ret = gamium.execute_rpc(RpcBy.method("Gamium.Private.CodebaseSample","CallStringParam1","asdf"))
    assert ret == None
    ret = gamium.execute_rpc(RpcBy.method("Gamium.Private.CodebaseSample", "CallEmptyParam"))
    assert ret == None
    ret = gamium.execute_rpc(RpcBy.method("Gamium.Private.CodebaseSample", "CallParam1", 10))
    assert ret == None
    ret = gamium.execute_rpc(RpcBy.method("Gamium.Private.CodebaseSample", "CallParam2", 10, 10.0))
    assert ret == None
    ret = gamium.execute_rpc(RpcBy.method("Gamium.Private.CodebaseSample", "CallParamReturn", 10, 10.1))
    assert ret == 1
    ret = gamium.execute_rpc(RpcBy.method("Gamium.Private.CodebaseSample", "CallParamReturn2", 3))
    assert ret == 3
    ret = gamium.execute_rpc(RpcBy.method("Gamium.Private.CodebaseSample", "CallParamReturn2", 1234567890))
    assert ret == 1234567890
    ret = gamium.execute_rpc(RpcBy.method("Gamium.Private.CodebaseSample", "CallParamReturn2", 1234567890.0123))
    assert ret == 1234567890.0123
    ret = gamium.execute_rpc(RpcBy.method("Gamium.Private.CodebaseSample", "CallParamReturn3", 10, 10.1))
    assert ret == {
        "double1": 0.123456,
        "double2": 1,
        "double3": 1000,
        "double4": 1234567890,
        "double5": 1234567890.12345,
        "intMax": 2147483647,
        "intMin": -2147483648,
        "hello": "nice",
        "nestedDict": {"hello": "nice2"},
        "nestedArray": ["hello", "nice2"],
    }


rpc_method_test()


def rpc_field_test():
    ret = gamium.execute_rpc(RpcBy.field("Gamium.Private.CodebaseSample", "field1"))
    assert ret == 10.0
    ret = gamium.execute_rpc(RpcBy.field("Gamium.Private.CodebaseSample", "field2"))
    assert ret == [1, 2]
    ret = gamium.execute_rpc(RpcBy.field("Gamium.Private.CodebaseSample", "field3"))
    assert ret == {"hello": "nice"}


rpc_field_test()


def rpc_property_test():
    ret = gamium.execute_rpc(RpcBy.property("Gamium.Private.CodebaseSample", "property1"))
    assert ret == 0
    ret = gamium.execute_rpc(RpcBy.property("Gamium.Private.CodebaseSample", "property2"))
    assert ret == []


rpc_property_test()


def click_test():
    ui.click(By.path("/Canvas[1]/MainMenu[1]/Window[1]/Settings[1]"))
    ui.click(By.path("/Canvas[1]/Settings[1]/SettingsWindow[1]/Video[1]"))
    ui.click(By.path("/Canvas[1]/Settings[1]/SettingsPanels[1]/VideoWindow[1]/Panel[1]/Settings[1]/Anti-Aliasing toggle[1]/Checkmark[1]"))
    ui.click(By.path("/Canvas[1]/Settings[1]/SettingsPanels[1]/VideoWindow[1]/Panel[1]/Settings[1]/AO Toggle[1]/Checkmark[1]"))
    ui.click(By.path("/Canvas[1]/Settings[1]/SettingsPanels[1]/VideoWindow[1]/Panel[1]/Close[1]"))
    ui.click(By.path("/Canvas[1]/Settings[1]/SettingsWindow[1]/Back[1]"))
    ui.click(By.path("/Canvas[1]/MainMenu[1]/Window[1]/NextScene[1]"))


click_test()


def drag_test():
    ui.find(By.path("/Canvas[1]/Panel Top[1]/Drag Box[1]/Drag Image[1]"))
    srcElem = ui.find(By.path("/Canvas[1]/Panel Top[1]/Drag Box[1]/Drag Image[1]"))
    src2Elem = ui.find(By.path("/Canvas[1]/Panel Top[1]/Drag Box[2]/Drag Image[1]"))
    destElem = ui.find(By.path("/Canvas[1]/Panel 1[1]/Drop Box[1]/Drop Image[1]"))
    dest2Elem = ui.find(By.path("/Canvas[1]/Panel 2[1]/Drop Box[1]/Drop[1]"))
    srcElem.wait_interactable()
    src2Elem.wait_interactable()
    destElem.wait_interactable()
    dest2Elem.wait_interactable()
    srcElem.drag(destElem, ActionDragOptions(1000, 100))
    src2Elem.drag(dest2Elem, ActionDragOptions(1000, 100))

    ui.click(By.path("/Canvas[1]/NextScene[1]"))


drag_test()


def controls_test():
    handle = ui.find(By.path("/Canvas[1]/Panel[1]/Slider[1]/Handle Slide Area[1]/Handle[1]"))
    handle.wait_interactable()
    handle.drag(Vector2(handle.info.screen_position.x - 30, handle.info.screen_position.y), ActionDragOptions(1000, 100))

    toggles = [
        "/Canvas[1]/Panel[1]/Toggle[1]/Label[1]",
        "/Canvas[1]/Panel[1]/ToggleGroup[1]/OptionA[1]",
        "/Canvas[1]/Panel[1]/ToggleGroup[1]/OptionB[1]",
        "/Canvas[1]/Panel[1]/ToggleGroup[1]/OptionC[1]",
    ]
    for toggleName in toggles:
        ui.click(By.path(toggleName))

    dropDowns = ["/Canvas[1]/Panel[1]/Dropdown[1]/Label[1]", "/Canvas[1]/Panel[1]/Dropdown[1]/Dropdown List[1]/Viewport[1]/Content[1]/Item 1: Option 2[1]"]
    for dropDownName in dropDowns:
        ui.click(By.path(dropDownName))

    ui.click(By.path("/Canvas[1]/Panel[1]/InputField[1]/Text[1]"))
    ui.set_text(By.path("/Canvas[1]/Panel[1]/InputField[1]/Text[1]"), "Hello World")

    ui.click(By.path("/Canvas[1]/Panel[1]/InputArea[1]/Text[1]"))
    ui.set_text(By.path("/Canvas[1]/Panel[1]/InputArea[1]/Text[1]"), "Hello World\nasdf 안녕 반가워")

    ui.click(By.path("/Canvas[1]/Panel[1]/Button[1]"))

    scroll = ui.find(By.path("/Canvas[1]/Scroll View[1]/Viewport[1]/Content[1]"))
    scroll.wait_interactable()
    scroll.scroll(Vector2(0, -0.1), ActionScrollOptions(300))
    scroll.scroll(Vector2(0, 1))

    ui.click(By.path("/Canvas[1]/NextScene[1]"))


controls_test()


def keycode_test():
    keys = []
    for attr_name in dir(UnityKeyCode):
        if not attr_name.startswith("__"):
            keys.append(attr_name)
    keys.remove("LeftMeta")
    keys.remove("RightMeta")

    title = ui.find(By.path("/Canvas[1]/Window[1]/SF Title[1]/TitleLabel[1]"))

    for key in keys:
        gamium.send_key(KeyBy.unity_keycode(key), SendKeyOptions(30))
        gamium.send_key(KeyBy.unity_keycode(key.lower()), SendKeyOptions(30))
        text = title.get_text()
        assert getattr(UnityKeyCode, text) == getattr(UnityKeyCode, key), f"keycode test failed: {getattr(UnityKeyCode, text)} != {getattr(UnityKeyCode, key)}"


keycode_test()


def quit():
    gamium.actions().app_quit().perform()
    sys.exit(0)

quit()
