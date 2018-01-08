package pages.app

import geb.Page
import extensions.ReactJSAware

class ProjectPage extends Page implements ReactJSAware {
    static at = { reactReady && title == "MOTI Hired Equipment Tracking System" && $("h1", text: startsWith("Projects")) }
    static url = "?#/projects"

    static content = {
            
    }
}