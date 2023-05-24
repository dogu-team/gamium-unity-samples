import uuid
from gamium import *
import asyncio
import sys


async def run():
    gamium = GamiumClient("127.0.0.1", 50061)
    await gamium.connect()
    ui = gamium.ui()

    async def create_account():
        ret = await ui.try_find(By.path("/Canvas[1]/Start[1]/DeleteAccountButton[1]"))
        if ret.success and (await ret.value.try_wait_interactable()).success:
            await ret.value.click()
        await ui.click(By.path("/Canvas[1]/Login[1]/Panel[1]/GuestLoginBtn[1]"))
        await ui.set_text(By.path("/Canvas[1]/Register[1]/InputField[1]"), str(uuid.uuid4())[2:11])
        await ui.click(By.path("/Canvas[1]/Register[1]/OkBtn[1]"))
        await ui.click(By.path("/Canvas[1]/Start[1]/Desc[1]"))

    await create_account()

    async def create_character():
        await ui.click(By.path("/Canvas[1]/SelectCharacter[1]/RightPanel[1]/CharacterScrollView[1]/Viewport[1]/Content/SquareButton(Clone)[1]"))
        await ui.click(By.path("/Canvas[1]/CreateCharacter[1]/RightPanel[1]/CharacterScrollView[1]/Viewport[1]/Content[1]/SquareButton(Clone)[2]"))
        await ui.set_text(
            By.path("/Canvas[1]/CreateCharacter[1]/RightPanel[1]/NicknamePanel[1]/InputField[1]"),
            str(uuid.uuid4())[2:11],
        )
        await ui.click(By.path("/Canvas[1]/CreateCharacter[1]/RightPanel[1]/NicknamePanel[1]/OkButton[1]"))
        await ui.click(By.path("/Canvas[1]/SelectCharacter[1]/RightPanel[1]/StartBtn[1]"))

    await create_character()

    async def go_to_shop():
        await ui.find(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]"))

        player = await gamium.player(By.path("/PlayerSpawnPoint[1]/WizardCharacter(Clone)[1]"))
        await player.move(
            By.path("/Main Camera[1]"),
            By.path("/Shops[1]/PotionShop[1]"),
            MovePlayerOptions(MovePlayerBy.Navigation),
        )

    await go_to_shop()

    async def buy_products():
        products = await ui.finds(
            By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/LeftPanel[1]/Products[1]/Scroll View[1]/Viewport[1]/Content[1]/ProductSlot(Clone)")
        )
        scrollBar = await ui.find(
            By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/LeftPanel[1]/Products[1]/Scroll View[1]/Scrollbar[1]/Sliding Area[1]/Handle[1]/Image 1[1]"),
        )
        await scrollBar.wait_interactable()
        for item in products:

            async def wait_until_interactable():
                result = await gamium.try_wait(Until.element_interactable(item), WaitOptions(300))
                if True == result.success:
                    await result.value.click()
                    return True
                await scrollBar.drag(
                    Vector2(scrollBar.info.position.x, scrollBar.info.position.y - 100),
                    ActionDragOptions(duration_ms=100, interval_ms=10),
                )
                return False

            await gamium.wait(wait_until_interactable, WaitOptions(timeout_ms=10000))

            await ui.click(By.path("/Canvas[1]/ShopView[1]/MultipurposePopup(Clone)[1]/UIRoot[1]/Bottom[1]/Confirm[1]/Text[1]"))

    await buy_products()

    async def sell_items():
        async def wait_until_interactable():
            items = await ui.finds(By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/RightPanel[1]/ItemGridView[1]/GridPanel[1]/ItemSlot(Clone)/Text"))
            if len(items) < 2:
                return True
            item = items[len(items) - 1]
            await item.click()

            await ui.click(By.path("/Canvas[1]/ShopView[1]/MultipurposePopup(Clone)[1]/UIRoot[1]/Bottom[1]/Confirm[1]/Text[1]"))
            return False

        await gamium.wait(wait_until_interactable, WaitOptions(timeout_ms=10000))

        await ui.click(By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/RoundButton[1]"))

    await sell_items()

    async def go_to_equipment_shop():
        player = await gamium.player(By.path("/PlayerSpawnPoint[1]/WizardCharacter(Clone)[1]"))
        await player.move(By.path("/Main Camera[1]"), By.path("/Shops[1]/EquipmentShop[1]"), MovePlayerOptions(MovePlayerBy.Navigation))

    await go_to_equipment_shop()

    async def buy_equipment_products():
        products = await ui.finds(
            By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/LeftPanel[1]/Products[1]/Scroll View[1]/Viewport[1]/Content[1]/ProductSlot(Clone)")
        )
        scrollBar = await ui.find(
            By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/Layout[1]/LeftPanel[1]/Products[1]/Scroll View[1]/Scrollbar[1]/Sliding Area[1]/Handle[1]/Image 1[1]")
        )
        await scrollBar.wait_interactable()

        target_indexes = [2, 3, 5, 7, 9]
        for i, item in enumerate(products):
            if i not in target_indexes:
                continue

            async def wait_until_interactable() -> bool:
                result = await gamium.try_wait(Until.element_interactable(item), WaitOptions(300))
                if result.success:
                    await result.value.click()
                    return True
                await scrollBar.drag(Vector2(scrollBar.info.position.x, scrollBar.info.position.y - 100), ActionDragOptions(100, 10))
                return False

            await gamium.wait(wait_until_interactable, WaitOptions(10000))

            await ui.click(By.path("/Canvas[1]/ShopView[1]/MultipurposePopup(Clone)[1]/UIRoot[1]/Bottom[1]/Confirm[1]/Text[1]"))

        await ui.click(By.path("/Canvas[1]/ShopView[1]/UIRoot[1]/RoundButton[1]"))

    await buy_equipment_products()

    async def equip():
        await ui.click(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]/InventoryButton[1]"))

        equipments = await ui.finds(By.path("/Canvas[1]/InventoryView[1]/UIRoot[1]/Layout[1]/RightPanel[1]/ItemGridView[1]/GridPanel[1]/ItemSlot(Clone)"))
        for i in range(1, len(equipments)):
            item = equipments[i]
            await item.wait_interactable()
            await item.click()

            await ui.click(By.path("/Canvas[1]/InventoryView[1]/MultipurposePopup(Clone)[1]/UIRoot[1]/Bottom[1]/Confirm[1]"))

        await ui.click(By.path("/Canvas[1]/InventoryView[1]/UIRoot[1]/RoundButton[1]"))

    await equip()

    async def quest():
        await ui.click(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]/QuestButton[1]"))

        await ui.click(
            By.path(
                "/Canvas[1]/QuestView[1]/UIRoot[1]/Layout[1]/CenterPanel[1]/Bg[1]/Scroll View[1]/Viewport[1]/Content[1]/QuestSlot(Clone)[1]/TextPanel[1]/SquareButton[1]"
            )
        )

        await ui.click(By.path("/Canvas[1]/QuestView[1]/UIRoot[1]/RoundButton[1]"))

    await quest()

    async def hunt():
        await ui.click(By.path("/Canvas[1]/GameSceneView[1]/BottomPanel[1]/AutoHunt[1]"))

    await hunt()

    async def check_quest_done():
        async def wait_until_quest_done() -> bool:
            progress = await ui.get_text(
                By.path(
                    "/Canvas[1]/GameSceneView[1]/QuestStackView[1]/Scroll View[1]/Viewport[1]/Content[1]/QuestStackSlot(Clone)[1]/TextPanel[1]/ProgressText[1]"
                )
            )
            if progress == "2 / 2":
                return True
            await gamium.sleep(1000)
            return False

        await gamium.wait(wait_until_quest_done, WaitOptions(80000))

        # hunt off
        await ui.click(By.path("/Canvas[1]/GameSceneView[1]/BottomPanel[1]/AutoHunt[1]"))

        # quest done
        await ui.click(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]/QuestButton[1]"))

        await ui.click(
            By.path(
                "/Canvas[1]/QuestView[1]/UIRoot[1]/Layout[1]/CenterPanel[1]/Bg[1]/Scroll View[1]/Viewport[1]/Content[1]/QuestSlot(Clone)[1]/TextPanel[1]/SquareButton[1]"
            )
        )

        await ui.click(By.path("/Canvas[1]/QuestView[1]/UIRoot[1]/RoundButton[1]"))
        await ui.click(By.path("/Canvas[1]/GameSceneView[1]/MainTopBar[1]/InventoryButton[1]"))

    await check_quest_done()

    async def quit():
        await gamium.sleep(4000)
        await gamium.actions().app_quit().perform()

        sys.exit(0)

    await quit()


asyncio.run(run())
