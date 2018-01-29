package extensions

trait ReactJSAware {
 
    boolean isReactReady() {
/*         if ($("script", src:"https://s3.ca-central-1.amazonaws.com/stens-angular-test/react-test-support.js").size() == 0) {
            browser.driver.executeScript("document.body.appendChild(document.createElement(\'script\')).src=\'https://s3.ca-central-1.amazonaws.com/stens-angular-test/react-test-support.js'")
            js.exec("document.addEventListener('DOMContentLoaded', function(event) { window.MYAPP.APP_READY = true; });")
        }   */
        
        //js.exec('window.MYAPP.waitForReact();');
        waitFor {
            //js.MYAPP.APP_READY == true
            js.exec("return !!(typeof window !== 'undefined' && window.document && window.document.createElement) && document.readyState=='complete';") == true
        }
         
    }
}

