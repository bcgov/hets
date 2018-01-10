package pages.app

import geb.Page
import extensions.ReactJSAware

class RequestPage extends Page implements ReactJSAware {
    static at = { reactReady && title == "MOTI Hired Equipment Tracking System"  && $("h1", text: startsWith("Rental Requests")) }
    static url = "?#/rental-requests"

    static content = {
        }
}