from gamium import *
import sys


service = TcpGamiumService("127.0.0.1", 50061)
gamium = GamiumClient(service)
gamium.connect()
ui = gamium.ui()

def quit():
    gamium.actions().app_quit().perform()
    sys.exit(0)

quit()
