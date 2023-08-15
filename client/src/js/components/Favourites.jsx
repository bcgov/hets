import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Alert, Dropdown, ButtonGroup, Button } from 'react-bootstrap';
import { Col, Row } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import * as Api from '../api';
import * as Constant from '../constants';

import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import Authorize from '../components/Authorize.jsx';
import EditFavouritesDialog from '../views/dialogs/EditFavouritesDialog';

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
        this.props.dispatch(Api.updateFavourite({
          ...oldDefault,
          isDefault: false,
        }));
      }
    }

    this.closeDialog();
  };

  deleteFavourite = (favourite) => {
    this.props.dispatch(Api.deleteFavourite(favourite));
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

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(null, mapDispatchToProps)(Favourites);
