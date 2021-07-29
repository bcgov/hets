import PropTypes from 'prop-types';
import React from 'react';
import { Alert, Dropdown, ButtonGroup, Button } from 'react-bootstrap';
import { FormGroup, FormText, FormLabel } from 'react-bootstrap';
import { Col, Row } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import * as Api from '../api';
import * as Constant from '../constants';

import CheckboxControl from '../components/CheckboxControl.jsx';
import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import FormDialog from '../components/FormDialog.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import Authorize from '../components/Authorize.jsx';

import { isBlank } from '../utils/string';

class EditFavouritesDialog extends React.Component {
  static propTypes = {
    favourite: PropTypes.object.isRequired,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      name: props.favourite.name || '',
      isDefault: props.favourite.isDefault || false,
      nameError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.name !== this.props.favourite.name) {
      return true;
    }
    if (this.state.isDefault !== this.props.favourite.isDefault) {
      return true;
    }

    return false;
  };

  isValid = () => {
    if (isBlank(this.state.name)) {
      this.setState({ nameError: 'Name is required' });
      return false;
    }
    return true;
  };

  onSubmit = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const favourite = {
          ...this.props.favourite,
          name: this.state.name,
          isDefault: this.state.isDefault,
        };

        const promise = favourite.id ? Api.updateFavourite(favourite) : Api.addFavourite(favourite);
        promise.finally(() => {
          this.setState({ isSaving: false });
        });

        this.props.onSave(favourite);
      }

      this.props.onClose();
    }
  };

  render() {
    const { isSaving, name, nameError, isDefault } = this.state;
    const { show, onClose } = this.props;

    return (
      <FormDialog
        id="edit-favourite"
        title="Favourite"
        size="sm"
        show={show}
        isSaving={isSaving}
        onClose={onClose}
        onSubmit={this.onSubmit}
      >
        <FormGroup controlId="name">
          <FormLabel>
            Name <sup>*</sup>
          </FormLabel>
          <FormInputControl
            type="text"
            readOnly={isSaving}
            defaultValue={name}
            updateState={this.updateState}
            autoFocus
            isInvalid={nameError}
          />
          <FormText>{nameError}</FormText>
        </FormGroup>
        <CheckboxControl id="isDefault" checked={isDefault} updateState={this.updateState} label="Default" />
      </FormDialog>
    );
  }
}

class Favourites extends React.Component {
  static propTypes = {
    id: PropTypes.string,
    className: PropTypes.string,
    title: PropTypes.string,
    type: PropTypes.string.isRequired,
    favourites: PropTypes.object.isRequired,
    data: PropTypes.object.isRequired,
    onSelect: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      favouriteToEdit: {},
      showEditDialog: false,
      open: false,
    };
  }

  addFavourite = () => {
    this.editFavourite({
      type: this.props.type,
      name: '',
      isDefault: false,
      value: JSON.stringify(this.props.data),
    });
  };

  editFavourite = (favourite) => {
    this.setState({ favouriteToEdit: favourite });
    this.openDialog();
  };

  favoriteSaved = (favourite) => {
    // Make sure there's only one default
    if (favourite.isDefault) {
      var oldDefault = _.find(this.props.favourites, (f) => f.isDefault);
      if (oldDefault && favourite.id !== oldDefault.id) {
        Api.updateFavourite({
          ...oldDefault,
          isDefault: false,
        });
      }
    }

    this.closeDialog();
  };

  deleteFavourite = (favourite) => {
    Api.deleteFavourite(favourite);
  };

  selectFavourite = (favourite) => {
    this.toggle(false);
    this.props.onSelect(favourite);
  };

  openDialog = () => {
    this.setState({ showEditDialog: true });
  };

  closeDialog = () => {
    this.setState({ showEditDialog: false });
  };

  toggle = (open) => {
    this.setState({ open: open });
  };

  render() {
    var title = this.props.title || 'Favourites';
    var className = `favourites ${this.props.className || ''} `;

    return (
      <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
        <Dropdown id={this.props.id} className={className} title={title} open={this.state.open} onToggle={this.toggle}>
          <Dropdown.Toggle className="btn-custom">{title}</Dropdown.Toggle>

          <Dropdown.Menu>
            <div className="favourites-button-bar">
              <Button onClick={this.addFavourite}>Favourite Current Selection</Button>
            </div>
            {(() => {
              if (Object.keys(this.props.favourites).length === 0) {
                return (
                  <Alert variant="success" style={{ margin: '5px' }}>
                    No favourites
                  </Alert>
                );
              }

              return (
                <ul>
                  {_.map(this.props.favourites, (favourite) => {
                    return (
                      <li key={favourite.id}>
                        <Row>
                          <Col md={1}>{favourite.isDefault ? <FontAwesomeIcon icon="star" /> : ''}</Col>
                          <Col md={8}>
                            <span className="favourite__item" onClick={this.selectFavourite.bind(this, favourite)}>
                              {favourite.name}
                            </span>
                          </Col>
                          <Col md={3}>
                            <ButtonGroup>
                              <DeleteButton name="Favourite" onConfirm={this.deleteFavourite.bind(this, favourite)} />
                              <EditButton name="Favourite" onClick={this.editFavourite.bind(this, favourite)} />
                            </ButtonGroup>
                          </Col>
                        </Row>
                      </li>
                    );
                  })}
                </ul>
              );
            })()}
          </Dropdown.Menu>

          {this.state.showEditDialog ? (
            <EditFavouritesDialog
              show={this.state.showEditDialog}
              favourite={this.state.favouriteToEdit}
              onSave={this.favoriteSaved}
              onClose={this.closeDialog}
            />
          ) : null}
        </Dropdown>
      </Authorize>
    );
  }
}

export default Favourites;
