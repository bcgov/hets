import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { Row } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { VERSION_PATHNAME, PERMISSION_VERSION } from '../constants';

import Authorize from '../components/Authorize';

const Footer = () => {
  const currentUser = useSelector((state) => state.user);

  return (
    <footer id="footer">
      <div id="footerWrapper">
        <div id="footerAdminSection">
          <div id="footerAdminLinksContainer" className="container">
            <Row id="footerAdminLinks">
              <ul className="inline">
                <li>
                  <Link to={'/'}>Home</Link>
                </li>
                <li>
                  <a href="http://www2.gov.bc.ca/gov/content/about-gov-bc-ca">About gov.bc.ca</a>
                </li>
                <li>
                  <a href="http://www2.gov.bc.ca/gov/content/home/disclaimer">Disclaimer</a>
                </li>
                <li>
                  <a href="http://www2.gov.bc.ca/gov/content/home/privacy">Privacy</a>
                </li>
                <li>
                  <a href="http://www2.gov.bc.ca/gov/content/home/accessibility">Accessibility</a>
                </li>
                <li>
                  <a href="http://www2.gov.bc.ca/gov/content/home/copyright">Copyright</a>
                </li>
                <li>
                  <a href="http://www2.gov.bc.ca/gov/content/home/contact-us">Contact Us</a>
                </li>
                <Authorize requires={PERMISSION_VERSION}>
                  <li className="float-right" style={{ border: 0 }}>
                    <Link to={VERSION_PATHNAME}>Version</Link>
                  </li>
                </Authorize>
              </ul>
            </Row>
          </div>
        </div>
      </div>
    </footer>
  );
};

Footer.propTypes = {
  currentUser: PropTypes.object,
};

export default Footer;
