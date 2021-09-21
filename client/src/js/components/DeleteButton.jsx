// import PropTypes from 'prop-types';
// import React from 'react';
// import { Button, OverlayTrigger } from 'react-bootstrap';
// import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
// import _ from 'lodash';

// import Confirm from '../components/Confirm.jsx';

// class DeleteButton extends React.Component {
//   static propTypes = {
//     onConfirm: PropTypes.func.isRequired,
//     name: PropTypes.string,
//     hide: PropTypes.bool,
//   };

//   render() {
//     var props = _.omit(this.props, 'onConfirm', 'hide', 'name');

//     return (
//       <OverlayTrigger
//         placement="top"
//         trigger={'click'}
//         rootClose
//         overlay={<Confirm onConfirm={this.props.onConfirm} />}
//         transition={false} //removes findDOMNode warning for transitions
//         delay={100} //required so the onConfirm function is fired on the Confirm overlay before closing. If transition={true} this isn't needed.
//       >
//         {(overlayProps) => (
//           <Button
//             title={`Delete ${this.props.name}`}
//             size="sm"
//             className={this.props.hide ? 'd-none' : 'btn-custom'}
//             {...props}
//             {...overlayProps}
//           >
//             <FontAwesomeIcon icon="trash-alt" />
//           </Button>
//         )}
//       </OverlayTrigger>
//     );
//   }
// }

// export default DeleteButton;

import PropTypes from 'prop-types';
import React from 'react';
import { Button, OverlayTrigger } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import Confirm from '../components/Confirm.jsx';

DeleteButton.propTypes = {
  onConfirm: PropTypes.func.isRequired,
  name: PropTypes.string,
  hide: PropTypes.bool,
};

function DeleteButton({ onConfirm, hide, name, ...rest }) {
  return (
    <OverlayTrigger
      placement="top"
      trigger={'click'}
      rootClose
      overlay={<Confirm onConfirm={onConfirm} />}
      transition={false} //removes findDOMNode warning for transitions
      delay={100} //required so the onConfirm function is fired on the Confirm overlay before closing. If transition={true} this isn't needed.
    >
      {(overlayProps) => (
        <Button
          title={`Delete ${name}`}
          size="sm"
          className={hide ? 'd-none' : 'btn-custom'}
          {...rest}
          {...overlayProps}
        >
          <FontAwesomeIcon icon="trash-alt" />
        </Button>
      )}
    </OverlayTrigger>
  );
}

export default DeleteButton;
