import geb.spock.GebReportingSpec

// Import pages here

import pages.external.Accessability
import pages.external.Copyright
import pages.external.Disclaimer
import pages.external.Privacy

import spock.lang.*

@Title("Basic Navigational script to verify links and pages.")
class FlowSpecs extends GebReportingSpec {

    @Unroll
    def "Navigate Page from: #startPage, click Link: #clickLink, Assert Page: #assertPage"(){
        when: "I am on #startPage"
        to startPage
        and: "I click on #clickLink"
            $("a", id:"$clickLink").click()
        then: "I should see #assertPage"
        at assertPage

        where:
        startPage                 | clickLink                    || assertPage
        //Test Externally Linked Pages
        <application Page>        | "footer-about-copyright"     || Copyright
        <application Page>        | "footer-about-disclaimer"    || Disclaimer
        <application Page>        | "footer-about-privacy"       || Privacy
        <application Page>        | "footer-about-accessibility" || Accessability
    }
}
