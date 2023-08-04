import uuid
from gamium import *
import sys


service = TcpGamiumService("127.0.0.1", 50061)
gamium = GamiumClient(service)
gamium.connect()
ui = gamium.ui()


def reset_playerdata():
    buttonPaths = [
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/OptionsBar[1]/options-bar[1]/options-bar__button[1]",
        # reset
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-left[1]/settings-account[1]/settings__social-button1[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-left[1]/settings-account[1]/settings__social-button2[1]",
    ]

    for path in buttonPaths:
        ui.click(By.path(path))


reset_playerdata()


def get_set_text():
    textElemPath = "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-left[1]/settings-player-settings[1]/settings__player-textfield[1]/unity-text-input[1]"
    uuid_value = str(uuid.uuid4())[2:11]
    ui.set_text(By.path(textElemPath), uuid_value)
    textValue = ui.get_text(By.path(textElemPath))
    assert textValue == uuid_value


get_set_text()


def drag_toggle_radio():
    dropdownPaths = [
        "/MainMenuPanelSettings/MainMenu-container/SettingsScreen/settings-screen-overlay/settings-screen-background/settings__panel/settings__panel-body/settings__panel-body-right/settings__dropdown-container/settings__language-dropdown",
        "/MainMenuPanelSettings[1]/[1]/[1]/[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]",
    ]

    for path in dropdownPaths:
        ui.click(By.path(path))

    musicDragger = ui.find(
        By.path(
            "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__slider1-container[1]/settings__slider1-drag-area[1]/settings__slider1[1]/[1]/unity-drag-container[1]/unity-dragger[1]"
        )
    )
    musicDragger.wait_interactable()
    musicDragger.drag(Vector2(musicDragger.info.screen_position.x + 50, musicDragger.info.screen_position.y))
    musicDragger.drag(Vector2(musicDragger.info.screen_position.x - 100, musicDragger.info.screen_position.y))

    sfxDragger = ui.find(
        By.path(
            "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__slider2-container[1]/settings__slider2-drag-area[1]/settings__slider2[1]/[1]/unity-drag-container[1]/unity-dragger[1]"
        )
    )
    sfxDragger.wait_interactable()
    sfxDragger.drag(Vector2(sfxDragger.info.screen_position.x + 50, sfxDragger.info.screen_position.y))
    sfxDragger.drag(Vector2(sfxDragger.info.screen_position.x - 100, sfxDragger.info.screen_position.y))

    otherButtonPaths = [
        # toggle
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__slide-toggle-container[1]/settings-notifications__toggle[1]/[2]/[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__slide-toggle-container[1]/settings-notifications__toggle[1]/[2]/[1]",
        # radio
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__radio-button-container[1]/settings__graphics-radio-button-group[1]/[1]/[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__radio-button-container[1]/settings__graphics-radio-button-group[1]/[1]/[2]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings__panel-body[1]/settings__panel-body-right[1]/settings__radio-button-container[1]/settings__graphics-radio-button-group[1]/[1]/[3]",
        # back
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/SettingsScreen[1]/settings-screen-overlay[1]/settings-screen-background[1]/settings__panel[1]/settings-header[1]/settings__panel-back-button[1]",
    ]
    for path in otherButtonPaths:
        ui.click(By.path(path))


drag_toggle_radio()


def scroll():
    ui.click(
        By.path("/MainMenuPanelSettings[1]/MainMenu-container[1]/OptionsBar[1]/options-bar[1]/options-bar__gold-currency-group[1]/options-bar__gold-button[1]")
    )

    dragger = ui.find(
        By.path(
            "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gold-shopcontent[1]/shop__gold-scrollview[1]/[1]/unity-slider[1]/[1]/unity-drag-container[1]/unity-dragger[1]"
        )
    )
    dragger.wait_interactable()
    dragger.scroll(Vector2(0, -0.5), ActionScrollOptions(2000))
    dragger.scroll(Vector2(0, 0.5), ActionScrollOptions(2000))


scroll()


def buy():
    buttonPaths = [
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/OptionsBar[1]/options-bar[1]/options-bar__gem-currency-group[1]/options-bar__gem-button[1]",
        # buy gem
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-gem-shopcontent[1]/shop__gem-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[3]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
        # buy potion
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-screen-tabs[1]/shop-potion-shoptab[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-potion-shopcontent[1]/shop__potion-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[1]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-potion-shopcontent[1]/shop__potion-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[1]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/ShopScreen[1]/shop-screen-anchor[1]/shop-screen-container[1]/shop-screen-content[1]/shop-potion-shopcontent[1]/shop__potion-scrollview[1]/unity-content-and-vertical-scroll-container[1]/unity-content-viewport[1]/unity-content-container[1]/[1]/shop-item__parent-container[1]/shop-item__contents[1]/shop-item__buy-button[1]",
    ]

    for path in buttonPaths:
        ui.click(By.path(path))


buy()


def level_up():
    buttonPaths = [
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/MenuBar[1]/menu__background[1]/menu__button-group[1]/menu__char-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/CharScreen[1]/char-screen__anchor--center[1]/char__panel[1]/char__next-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/CharScreen[1]/char-screen__anchor--center[1]/char__panel[1]/char__panel-footer[1]/char__level-up-button[1]",
    ]

    for path in buttonPaths:
        ui.click(By.path(path))


level_up()


def kill_monster():
    buttonPaths = [
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/MenuBar[1]/menu__background[1]/menu__button-group[1]/menu__home-button[1]",
        "/MainMenuPanelSettings[1]/MainMenu-container[1]/HomeScreen[1]/home__panel[1]/home-play__level-panel[1]/home-play__level-frame[1]/home-play__level-button[1]",
    ]

    for path in buttonPaths:
        ui.click(By.path(path))


kill_monster()


def quit():
    gamium.sleep(4000)
    gamium.actions().app_quit().perform()

    sys.exit(0)


quit()
