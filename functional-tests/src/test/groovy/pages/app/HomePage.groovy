package pages.app

import geb.Page
import extensions.ReactJSAware

class HomePage extends Page implements ReactJSAware {
    static at = { reactReady && title == "MOTI Hired Equipment Tracking System" && $("#navbar-current-user > li > a", text: startsWith("Hets Test"))}
    static url = "?#/home"

    static content = {
        HomeLink { $("a", text:"Home")[0] }
        OwnersLink { $("a", text:"Owners")[0] }
        EquipmentLink { $("a", text:"Equipment")[0] }
        RequestsLink { $("a", text:"Requests")[0] }
        Projectslink { $("a", text:"Projects")[0] }
        AdministrationLink { $("a", text:"Administration")[0] } 
          
    }
}