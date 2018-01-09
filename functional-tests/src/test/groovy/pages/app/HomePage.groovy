package pages.app

import geb.Page
import extensions.ReactJSAware

class HomePage extends Page implements ReactJSAware {
    static at = { reactReady && title == "MOTI Hired Equipment Tracking System" && $("h1", text: startsWith("Roland Stens"))}
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