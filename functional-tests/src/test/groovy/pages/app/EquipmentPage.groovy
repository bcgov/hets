package pages.app

import geb.Page
import extensions.ReactJSAware

class EquipmentPage extends Page implements ReactJSAware {
    static at = { reactReady && title == "MOTI Hired Equipment Tracking System" && $("h1", text: startsWith("Equipment")) }
    static url = "?#/equipment"

    static content = {
            
    }
}