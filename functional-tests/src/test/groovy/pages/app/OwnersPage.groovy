package pages.app

import geb.Page
import extensions.ReactJSAware

class OwnersPage extends Page implements ReactJSAware {
    static at = { reactReady && title == "MOTI Hired Equipment Tracking System" && $("#owners-list > div.page-header > h1", text: startsWith("Owners")) && $("table.table") }
    static url = "?#/owners"

    static content = {
        owners_table { $("table.table") }
    }
}