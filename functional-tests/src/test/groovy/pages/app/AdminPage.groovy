package pages.app

import geb.Page
import extensions.ReactJSAware

class AdminPage extends Page implements ReactJSAware {
    static at = { reactReady && title == "MOTI Hired Equipment Tracking System" }
    static url = "?#/admin"

    static content = {
            
    }
}