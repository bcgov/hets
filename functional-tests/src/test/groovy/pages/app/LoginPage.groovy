package pages.app

import geb.Page

// Default SiteMinder Login Page (IDIR or BCeID)
class LoginPage extends Page {
    static at = { title == "Government of British Columbia" && $("#login-to").text() == "Log in to devsm.th.gov.bc.ca"}
    //static url = "https://logontest.gov.bc.ca/clp-cgi/int/logon.cgi?flags=1000:1,0&TARGET=\$SM\$https%3a%2f%2fdevsm%2eth%2egov%2ebc%2eca%2fhets%2f%3f"
    static content = {
        userName { $("#user") }
        passWord {  $("#password") }
        logIn { $("input", type:"submit", value:"Continue") }
    }
}

