import geb.spock.GebReportingSpec

import pages.app.AdminPage
import pages.app.EquipmentPage
import pages.app.HomePage
import pages.app.LoginPage
import pages.app.OwnersPage
import pages.app.ProjectPage
import pages.app.RequestPage

/* import pages.external.Accessability
import pages.external.Copyright
import pages.external.Disclaimer
import pages.external.Privacy */

import spock.lang.*

@Stepwise
@Title("Basic Navigational script to verify links and pages.")
class FlowSpecs extends GebReportingSpec {

    def "Login Once"(){
        when: "I go to the HETS URL "
            def env = System.getenv()
            go baseUrl
        and: "I log in on the SiteMinder Login page"    
            at LoginPage
            userName.value(env['TEST_USERNAME'])
            passWord.value(env['TEST_PASSWORD'])
            logIn.click()
        then: "I will arrive at the HETS Home page"    
            at HomePage
    }

    @Unroll
    def "Navigate Page from: #startPage, click Link: #clickLink, Assert Page: #assertPage"(){
        when: "I am on #startPage"
            to startPage
        and: "I click on #clickLink"
            waitFor { page."$clickLink".click() }
        then: "I should see #assertPage"
            waitFor { at assertPage }
        where:
            startPage                 | clickLink                    || assertPage
            HomePage                  | "HomeLink"                   || HomePage
            HomePage                  | "OwnersLink"                 || OwnersPage
            HomePage                  | "EquipmentLink"              || EquipmentPage
            HomePage                  | "RequestsLink"               || RequestPage
            HomePage                  | "Projectslink"               || ProjectPage
            //HomePage                  | "AdministrationLink"         || AdminPage

            //Test Externally Linked Pages
    /*         <application Page>        | "footer-about-copyright"     || Copyright
            <application Page>        | "footer-about-disclaimer"    || Disclaimer
            <application Page>        | "footer-about-privacy"       || Privacy
            <application Page>        | "footer-about-accessibility" || Accessability */
    }
}
