import React, { useMemo } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';

// Temporary adapter for class components while route props are migrated to hooks.
export const withRouter = (Component) => {
  const ComponentWithRouter = (props) => {
    const location = useLocation();
    const navigate = useNavigate();
    const params = useParams();
    const history = useMemo(
      () => ({
        go: navigate,
        goBack: () => navigate(-1),
        push: (to, state) => navigate(to, { state }),
        replace: (to, state) => navigate(to, { replace: true, state }),
      }),
      [navigate]
    );

    return <Component {...props} history={history} location={location} match={{ params }} navigate={navigate} />;
  };

  ComponentWithRouter.displayName = `withRouter(${Component.displayName || Component.name || 'Component'})`;
  return ComponentWithRouter;
};

