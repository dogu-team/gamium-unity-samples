import uuid
from gamium import *
import sys


service = TcpGamiumService("127.0.0.1", 50061)
gamium = GamiumClient(service)
gamium.connect()
ui = gamium.ui()


def create_account():
    ret = ui.try_find(By.path("/Canvas[1]/Start[1]/DeleteAccountButton[1]"))
    if ret.success and (ret.value.try_wait_interactable()).success:
        ret.value.click()
    ui.click(By.path("/Canvas[1]/Login[1]/Panel[1]/GuestLoginBtn[1]"))
    ui.set_text(By.path("/Canvas[1]/Register[1]/InputField[1]"), str(uuid.uuid4())[2:11])
    ui.click(By.path("/Canvas[1]/Register[1]/OkBtn[1]"))
    ui.click(By.path("/Canvas[1]/Start[1]/Desc[1]"))


create_account()


def create_character():
    ui.click(By.path("/Canvas[1]/SelectCharacter[1]/RightPanel[1]/CharacterScrollView[1]/Viewport[1]/Content/SquareButton(Clone)[1]"))
    ui.click(By.path("/Canvas[1]/CreateCharacter[1]/RightPanel[1]/CharacterScrollView[1]/Viewport[1]/Content[1]/SquareButton(Clone)[2]"))
    ui.set_text(
        By.path("/Canvas[1]/CreateCharacter[1]/RightPanel[1]/NicknamePanel[1]/InputField[1]"),
        str(uuid.uuid4())[2:11],
    )
    ui.click(By.path("/Canvas[1]/CreateCharacter[1]/RightPanel[1]/NicknamePanel[1]/OkButton[1]"))
    ui.click(By.path("/Canvas[1]/SelectCharacter[1]/RightPanel[1]/StartBtn[1]"))


create_character()


def go_to_shop():
    ui.find(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]"))

    player = gamium.player(By.path("/PlayerSpawnPoint[1]/WizardCharacter(Clone)[1]"))
    player.move(
        By.path("/Main Camera[1]"),
        By.path("/Shops[1]/PotionShop[1]"),
        MovePlayerOptions(MovePlayerBy.Navigation),
    )


go_to_shop()


def buy_products():
    products = ui.finds(By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/LeftPanel[1]/Products[1]/Scroll View[1]/Viewport[1]/Content[1]/ProductSlot(Clone)"))
    scrollBar = ui.find(
        By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/LeftPanel[1]/Products[1]/Scroll View[1]/Scrollbar[1]/Sliding Area[1]/Handle[1]/Image 1[1]"),
    )
    scrollBar.wait_interactable()
    for item in products:

        def wait_until_interactable():
            result = gamium.try_wait(Until.element_interactable(item), WaitOptions(300))
            if True == result.success:
                result.value.click()
                return True
            scrollBar.drag(
                Vector2(scrollBar.info.position.x, scrollBar.info.position.y - 100),
                ActionDragOptions(duration_ms=100, interval_ms=10),
            )
            return False

        gamium.wait(wait_until_interactable, WaitOptions(timeout_ms=10000))

        ui.click(By.path("/Canvas[1]/ShopView[1]/MultipurposePopup(Clone)[1]/UIRoot[1]/Bottom[1]/Confirm[1]/Text[1]"))


buy_products()


def sell_items():
    def wait_until_interactable():
        items = ui.finds(By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/RightPanel[1]/ItemGridView[1]/GridPanel[1]/ItemSlot(Clone)/Text"))
        if len(items) < 2:
            return True
        item = items[len(items) - 1]
        item.click()

        ui.click(By.path("/Canvas[1]/ShopView[1]/MultipurposePopup(Clone)[1]/UIRoot[1]/Bottom[1]/Confirm[1]/Text[1]"))
        return False

    gamium.wait(wait_until_interactable, WaitOptions(timeout_ms=10000))

    ui.click(By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/RoundButton[1]"))


sell_items()


def go_to_equipment_shop():
    player = gamium.player(By.path("/PlayerSpawnPoint[1]/WizardCharacter(Clone)[1]"))
    player.move(By.path("/Main Camera[1]"), By.path("/Shops[1]/EquipmentShop[1]"), MovePlayerOptions(MovePlayerBy.Navigation))


go_to_equipment_shop()


def buy_equipment_products():
    products = ui.finds(By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/LeftPanel[1]/Products[1]/Scroll View[1]/Viewport[1]/Content[1]/ProductSlot(Clone)"))
    scrollBar = ui.find(
        By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/LeftPanel[1]/Products[1]/Scroll View[1]/Scrollbar[1]/Sliding Area[1]/Handle[1]/Image 1[1]")
    )
    scrollBar.wait_interactable()

    target_indexes = [2, 3, 5, 7, 9]
    for i, item in enumerate(products):
        if i not in target_indexes:
            continue

        def wait_until_interactable() -> bool:
            result = gamium.try_wait(Until.element_interactable(item), WaitOptions(300))
            if result.success:
                result.value.click()
                return True
            scrollBar.drag(Vector2(scrollBar.info.position.x, scrollBar.info.position.y - 100), ActionDragOptions(100, 10))
            return False

        gamium.wait(wait_until_interactable, WaitOptions(10000))

        ui.click(By.path("/Canvas[1]/ShopView[1]/MultipurposePopup(Clone)[1]/UIRoot[1]/Bottom[1]/Confirm[1]/Text[1]"))

    ui.click(By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/RoundButton[1]"))


buy_equipment_products()


def equip():
    ui.click(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]/InventoryButton[1]"))

    equipments = ui.finds(By.path("/Canvas[1]/InventoryView[1]/UIRoot[1]/Layout[1]/RightPanel[1]/ItemGridView[1]/GridPanel[1]/ItemSlot(Clone)"))
    for i in range(1, len(equipments)):
        item = equipments[i]
        item.wait_interactable()
        item.click()

        ui.click(By.path("/Canvas[1]/InventoryView[1]/MultipurposePopup(Clone)[1]/UIRoot[1]/Bottom[1]/Confirm[1]"))

    ui.click(By.path("/Canvas[1]/InventoryView[1]/UIRoot[1]/RoundButton[1]"))


equip()


def quest():
    ui.click(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]/QuestButton[1]"))

    ui.click(
        By.path(
            "/Canvas[1]/QuestView[1]/UIRoot[1]/Layout[1]/CenterPanel[1]/Bg[1]/Scroll View[1]/Viewport[1]/Content[1]/QuestSlot(Clone)[1]/TextPanel[1]/SquareButton[1]"
        )
    )

    ui.click(By.path("/Canvas[1]/QuestView[1]/UIRoot[1]/RoundButton[1]"))


quest()


def hunt():
    ui.click(By.path("/Canvas[1]/GameSceneView[1]/BottomPanel[1]/AutoHunt[1]"))


hunt()


def check_quest_done():
    def wait_until_quest_done() -> bool:
        progress = ui.get_text(
            By.path("/Canvas[1]/GameSceneView[1]/QuestStackView[1]/Scroll View[1]/Viewport[1]/Content[1]/QuestStackSlot(Clone)[1]/TextPanel[1]/ProgressText[1]")
        )
        if progress == "2 / 2":
            return True
        gamium.sleep(1000)
        return False

    gamium.wait(wait_until_quest_done, WaitOptions(80000))

    # hunt off
    ui.click(By.path("/Canvas[1]/GameSceneView[1]/BottomPanel[1]/AutoHunt[1]"))

    # quest done
    ui.click(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]/QuestButton[1]"))

    ui.click(
        By.path(
            "/Canvas[1]/QuestView[1]/UIRoot[1]/Layout[1]/CenterPanel[1]/Bg[1]/Scroll View[1]/Viewport[1]/Content[1]/QuestSlot(Clone)[1]/TextPanel[1]/SquareButton[1]"
        )
    )

    ui.click(By.path("/Canvas[1]/QuestView[1]/UIRoot[1]/RoundButton[1]"))
    ui.click(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]/InventoryButton[1]"))


check_quest_done()


def quit():
    gamium.sleep(4000)
    gamium.actions().app_quit().perform()

    sys.exit(0)


quit()
