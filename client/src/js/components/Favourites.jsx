import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { connect } from 'react-redux';
import { Alert, Dropdown, ButtonGroup, Button, Col, Row } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import * as Api from '../api';
import * as Constant from '../constants';

import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import Authorize from '../components/Authorize.jsx';
import EditFavouritesDialog from '../views/dialogs/EditFavouritesDialog';

const Favourites = (props) => {
  const { id, className, title, type, favourites, data, onSelect, dispatch } = props;

  const [favouriteToEdit, setFavouriteToEdit] = useState({});
  const [showEditDialog, setShowEditDialog] = useState(false);
  const [open, setOpen] = useState(false);

  const addFavourite = () => {
    editFavourite({
      type: type,
      name: '',
      isDefault: false,
      value: JSON.stringify(data),
    });
  };

  const editFavourite = (favourite) => {
    setFavouriteToEdit(favourite);
    openDialog();
  };

  const favoriteSaved = (favourite) => {
    if (favourite.isDefault) {
      const oldDefault = _.find(favourites, (f) => f.isDefault);
      if (oldDefault && favourite.id !== oldDefault.id) {
        dispatch(
          Api.updateFavourite({
            ...oldDefault,
            isDefault: false,
          })
        );
      }
    }
    closeDialog();
  };

  const deleteFavourite = (favourite) => {
    dispatch(Api.deleteFavourite(favourite));
  };

  const selectFavourite = (favourite) => {
    toggle(false);
    onSelect(favourite);
  };

  const openDialog = () => {
    setShowEditDialog(true);
  };

  const closeDialog = () => {
    setShowEditDialog(false);
  };

  const toggle = (isOpen) => {
    setOpen(isOpen);
  };

  const dropdownTitle = title || 'Favourites';
  const dropdownClassName = `favourites ${className || ''}`;

  return (
    <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
      <Dropdown id={id} className={dropdownClassName} show={open} onToggle={toggle}>
        <Dropdown.Toggle className="btn-custom">{dropdownTitle}</Dropdown.Toggle>

        <Dropdown.Menu>
          <div className="favourites-button-bar">
            <Button onClick={addFavourite}>Favourite Current Selection</Button>
          </div>

          {Object.keys(favourites).length === 0 ? (
            <Alert variant="success" style={{ margin: '5px' }}>
              No favourites
            </Alert>
          ) : (
            <ul>
              {_.map(favourites, (favourite) => (
                <li key={favourite.id}>
                  <Row>
                    <Col md={1}>{favourite.isDefault ? <FontAwesomeIcon icon="star" /> : ''}</Col>
                    <Col md={8}>
                      <span className="favourite__item" onClick={() => selectFavourite(favourite)}>
                        {favourite.name}
                      </span>
                    </Col>
                    <Col md={3}>
                      <ButtonGroup>
                        <DeleteButton name="Favourite" onConfirm={() => deleteFavourite(favourite)} />
                        <EditButton name="Favourite" onClick={() => editFavourite(favourite)} />
                      </ButtonGroup>
                    </Col>
                  </Row>
                </li>
              ))}
            </ul>
          )}
        </Dropdown.Menu>

        {showEditDialog && (
          <EditFavouritesDialog
            show={showEditDialog}
            favourite={favouriteToEdit}
            onSave={favoriteSaved}
            onClose={closeDialog}
          />
        )}
      </Dropdown>
    </Authorize>
  );
};

Favourites.propTypes = {
  id: PropTypes.string,
  className: PropTypes.string,
  title: PropTypes.string,
  type: PropTypes.string.isRequired,
  favourites: PropTypes.object.isRequired,
  data: PropTypes.object.isRequired,
  onSelect: PropTypes.func.isRequired,
  dispatch: PropTypes.func.isRequired,
};

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(null, mapDispatchToProps)(Favourites);
