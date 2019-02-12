package pages.app

import geb.Page

// Default SiteMinder Login Page (IDIR)
class LoginPage extends Page {
    static at = { title == "Government of British Columbia" && $("#login-to").text() == "Log in to dev-hets.th.gov.bc.ca"}
    //static url = ""
    static content = {
        userName { $("#user") }
        passWord {  $("#password") }
        logIn { $("input", type:"submit", value:"Continue") }
    }
}
