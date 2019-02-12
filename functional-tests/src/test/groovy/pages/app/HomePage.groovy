package pages.app

import geb.Page
import extensions.ReactJSAware

class HomePage extends Page implements ReactJSAware {
    static at = { reactReady && title == "MOTI Hired Equipment Tracking System" && $("button", id:"profile-menu")}
    static url = "?#/home"

    static content = {
        HomeLink { $("a", text:"Home", "action":"push") }
        OwnersLink { $("a", text:"Owners", "action":"push") }
        EquipmentLink { $("a", text:"Equipment", "action":"push") }
        RequestsLink { $("a", text:"Requests", "action":"push") }
        Projectslink { $("a", text:"Projects", "action":"push") }
        AdministrationLink { $("a", text:"Administration", "action":"push") } 
        DistrictadminLink { $("a", text:"District Admin", "action":"push") } 
    }
}