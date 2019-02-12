import geb.spock.GebReportingSpec
import pages.app.OwnersPage
import pages.app.LoginPage
import pages.app.HomePage
import spock.lang.*

@Stepwise
@Title("Owner's Page Validation")
class OwnersSpecs extends GebReportingSpec {

    def "Login Once"(){
        when: "I go to the HETS URL "
            def env = System.getenv()
            go baseUrl
            waitFor { LoginPage }
        and: "I log in on the SiteMinder Login page"    
            at LoginPage
            userName.value(env['TEST_USERNAME'])
            passWord.value(env['TEST_PASSWORD'])
            logIn.click()
        then: "I will arrive at the HETS Home page"    
            waitFor { HomePage }
            at HomePage
    }

    def "Check all elements on Owners page"(){
        when: "I go to the Owners Page"
            to OwnersPage
        then: "I should see all defined screen elements"
            waitFor { assert owners_table }
    }
}
