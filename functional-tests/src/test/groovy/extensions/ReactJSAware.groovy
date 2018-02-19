package extensions

trait ReactJSAware {
 
    boolean isReactReady() {
        waitFor {
            js.exec("return !!(typeof window !== 'undefined' && window.document && window.document.createElement) && document.readyState=='complete' && document.title=='MOTI Hired Equipment Tracking System';") && $("footer",id:"footer").displayed
        }
         
    }
}

