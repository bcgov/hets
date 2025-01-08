import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

const Main = ({ location }) => {
  const [path, setPath] = useState(location.pathname);

  useEffect(() => {
    setPath(location.pathname);
  }, [location.pathname]);

  return (
    <div id="not-found-screen">
      <h1>
        Not found <span>:(</span>
      </h1>

      <p>
        Sorry, but the page you were trying to view ({path}) does not exist. You can try going to the{' '}
        <Link to={'/'}>home</Link> page.
      </p>
    </div>
  );
};

Main.propTypes = {
  location: PropTypes.object,
};

export default Main;
