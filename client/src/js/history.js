import React from 'react';
import { Link } from 'react-router-dom';

import * as Api from './api';
import * as Constant from './constants';

// History Events
export const OWNER_ADDED = 'Owner %e was added.';
export const OWNER_MODIFIED = 'Owner %e was modified.';
export const OWNER_MODIFIED_STATUS = 'Owner %e status changed to "%e". Comment: %e';
export const OWNER_MODIFIED_NAME = 'Owner %e is now %e';
export const OWNER_EQUIPMENT_ADDED = 'Owner %e added equipment %e.';
export const OWNER_EQUIPMENT_VERIFIED = 'Owner %e verified equipment %e.';
export const OWNER_MODIFIED_POLICY = 'Owner %e modified policy.';
export const OWNER_DOCUMENT_ADDED = 'Owner %e added document %e.';
export const OWNER_DOCUMENTS_ADDED = 'Owner %e added documents.';
export const OWNER_DOCUMENT_DELETED = 'Owner %e removed document %e.';
export const OWNER_CONTACT_ADDED = 'Owner %e added contact %e.';
export const OWNER_CONTACT_UPDATED = 'Owner %e modified contact %e.';
export const OWNER_CONTACT_DELETED = 'Owner %e removed contact %e.';

export const PROJECT_ADDED = 'Project %e was added.';
export const PROJECT_MODIFIED = 'Project %e was modified.';
export const PROJECT_MODIFIED_STATUS = 'Project %e status changed to %e.';
export const PROJECT_MODIFIED_NAME = 'Project %e is now %e';
export const PROJECT_EQUIPMENT_ADDED = 'Project %e added equipment %e.';
export const PROJECT_CONTACT_ADDED = 'Project %e added contact %e.';
export const PROJECT_CONTACT_UPDATED = 'Project %e modified contact %e.';
export const PROJECT_CONTACT_DELETED = 'Project %e removed contact %e.';
export const PROJECT_DOCUMENT_ADDED = 'Project %e added document %e.';
export const PROJECT_DOCUMENTS_ADDED = 'Project %e added documents.';
export const PROJECT_DOCUMENT_DELETED = 'Project %e removed document %e.';
export const PROJECT_EQUIPMENT_RELEASED = 'Project %e released equipment %e.';
export const PROJECT_RENTAL_REQUEST_ADDED = 'Project %e added rental request %e.';

export const USER_ADDED = 'User %e was added.';
export const USER_MODIFIED = 'User %e was modified.';
export const USER_DELETED = 'User %e was deleted.';

export const EQUIPMENT_ADDED = 'Equipment %e was added.';
export const EQUIPMENT_MODIFIED = 'Equipment %e was modified.';
export const EQUIPMENT_STATUS_MODIFIED = 'Equipment %e status changed to "%e". Comment: %e';
export const EQUIPMENT_SENIORITY_MODIFIED = 'Equipment %e seniority was modified.';
export const EQUIPMENT_DOCUMENT_ADDED = 'Equipment %e added document %e.';
export const EQUIPMENT_DOCUMENTS_ADDED = 'Equipment %e added documents.';
export const EQUIPMENT_DOCUMENT_DELETED = 'Equipment %e removed document %e.';
export const EQUIPMENT_ATTACHMENT_ADDED = 'Equipment %e added attachment %e.';
export const EQUIPMENT_ATTACHMENT_UPDATED = 'Equipment %e modified attachment %e.';
export const EQUIPMENT_ATTACHMENT_DELETED = 'Equipment %e removed attachment %e.';

export const RENTAL_REQUEST_ADDED = 'Rental request %e was added.';
export const RENTAL_REQUEST_MODIFIED = 'Rental request was modified.';
export const RENTAL_REQUEST_DOCUMENT_ADDED = 'Rental request %e added document %e.';
export const RENTAL_REQUEST_DOCUMENTS_ADDED = 'Rental request %e added documents.';
export const RENTAL_REQUEST_DOCUMENT_DELETED = 'Rental request %e removed document %e.';
export const RENTAL_REQUEST_EQUIPMENT_HIRED = 'Rental request %e equipment %e was given a response of %e.';

// Helper to create an entity object
export function makeHistoryEntity(type, entity) {
  return {
    type: type,
    id: entity.id,
    description: entity.name,
    path: entity.path,
    url: entity.url,
  };
}

// Log a history event
export const log = (historyEntity, event, ...entities) => async (dispatch) => {
  // prepend the 'parent' entity
  entities.unshift(historyEntity);

  // Build the history text
  const historyText = JSON.stringify({
    // The event text, with entity placeholders
    text: event,
    // The array of entities
    entities: entities,
  });

  // Choose the correct API call.
  let addHistoryPromise = null;

  switch (historyEntity.type) {
    case Constant.HISTORY_OWNER:
      addHistoryPromise = Api.addOwnerHistory;
      break;

    case Constant.HISTORY_PROJECT:
      addHistoryPromise = Api.addProjectHistory;
      break;

    case Constant.HISTORY_EQUIPMENT:
      addHistoryPromise = Api.addEquipmentHistory;
      break;

    case Constant.HISTORY_REQUEST:
      addHistoryPromise = Api.addRentalRequestHistory;
      break;

    case Constant.HISTORY_USER:
    case Constant.HISTORY_ROLE:
      break;
    default:
      break;
  }

  if (addHistoryPromise) {
    return await dispatch(addHistoryPromise(historyEntity.id, { historyText }));
  }

  return null;
};

function buildLink(entity, closeFunc) {
  // Return a link if the entity has a path; just the description otherwise.
  return entity.path ? (
    //some entity.paths when application was using react-router did not have /url. Post react-router-dom adds the /.
    //Thus, rather than change the DB information we check to see if an additional / is required in the url.
    <Link onClick={closeFunc} to={entity.path[0] === '/' ? entity.path : `/${entity.path}`}>
      {entity.description}
    </Link>
  ) : (
    <span>{entity.description}</span>
  );
}

export function renderEvent(historyText, closeFunc) {
  try {
    // Unwrap the JSONed event
    var event = JSON.parse(historyText);

    // Parse the text and return it inside a <div>, replacing field placeholders with linked content.
    var tokens = event.text.split('%e');
    return (
      <div>
        {tokens.map((token, index) => {
          return (
            <span key={index}>
              {token}
              {index < tokens.length - 1 ? buildLink(event.entities[index], closeFunc || null) : null}
            </span>
          );
        })}
      </div>
    );
  } catch (err) {
    // Not JSON so just push out what's in there.
    return <span>{historyText}</span>;
  }
}

export const get = (historyEntity, offset, limit) => async (dispatch) => {
  // If not showing all, then just fetch the first 10 entries
  const params = {
    offset: offset || 0,
    limit: limit || null,
  };

  switch (historyEntity.type) {
    case Constant.HISTORY_OWNER:
      return await dispatch(Api.getOwnerHistory(historyEntity.id, params));

    case Constant.HISTORY_PROJECT:
      return await dispatch(Api.getProjectHistory(historyEntity.id, params));

    case Constant.HISTORY_EQUIPMENT:
      return await dispatch(Api.getEquipmentHistory(historyEntity.id, params));

    case Constant.HISTORY_REQUEST:
      return await dispatch(Api.getRentalRequestHistory(historyEntity.id, params));

    case Constant.HISTORY_USER:
    case Constant.HISTORY_ROLE:
      break;
    default:
      break;
  }

  return null;
};

// Logging
export const ownerAdded = (owner) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_ADDED));
};

export const ownerModified = (owner) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_MODIFIED));
};

export const ownerModifiedStatus = (owner, status, statusComment) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_MODIFIED_STATUS, { description: status }, { description: statusComment }));
};

export const ownerContactAdded = (owner, contact) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_CONTACT_ADDED, contact.historyEntity));
};

export const ownerContactUpdated = (owner, contact) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_CONTACT_UPDATED, contact.historyEntity));
};

export const ownerContactDeleted = (owner, contact) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_CONTACT_DELETED, contact.historyEntity));
};

export const ownerModifiedPolicy = (owner) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_MODIFIED_POLICY));
};

export const ownerEquipmentAdded = (owner, equipment) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_EQUIPMENT_ADDED, equipment.historyEntity));
};

export const ownerEquipmentVerified = (owner, equipment) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_EQUIPMENT_VERIFIED, equipment.historyEntity));
};

export const ownerDocumentAdded = (owner, document) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_DOCUMENT_ADDED, {
    description: document,
  }));
};

export const ownerDocumentsAdded = (owner) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_DOCUMENTS_ADDED));
};

export const ownerDocumentDeleted = (owner, document) => async (dispatch) => {
  return await dispatch(log(owner.historyEntity, OWNER_DOCUMENT_DELETED, document.historyEntity));
};

export const projectAdded = (project) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_ADDED));
};

export const projectModified = (project) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_MODIFIED));
};

export const projectModifiedStatus = (project) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_MODIFIED_STATUS, {
    description: project.status,
  }));
};

export const projectContactAdded = (project, contact) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_CONTACT_ADDED, contact.historyEntity));
};

export const projectContactUpdated = (project, contact) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_CONTACT_UPDATED, contact.historyEntity));
};

export const projectContactDeleted = (project, contact) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_CONTACT_DELETED, contact.historyEntity));
};

export const projectDocumentAdded = (project, document) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_DOCUMENT_ADDED, {
    description: document,
  }));
};

export const projectDocumentsAdded = (project) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_DOCUMENTS_ADDED));
};

export const projectDocumentDeleted = (project, document) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_DOCUMENT_DELETED, document.historyEntity));
};

export const projectEquipmentReleased = (project, equipment) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_EQUIPMENT_RELEASED, equipment.historyEntity));
};

export const projectRentalRequestAdded = (project, rentalRequest) => async (dispatch) => {
  return await dispatch(log(project.historyEntity, PROJECT_RENTAL_REQUEST_ADDED, rentalRequest.historyEntity));
};

export const equipmentAdded = (equipment) => async (dispatch) => {
  return await dispatch(log(equipment.historyEntity, EQUIPMENT_ADDED));
};

export const equipmentModified = (equipment) => async (dispatch) => {
  return await dispatch(log(equipment.historyEntity, EQUIPMENT_MODIFIED));
};

export const equipmentSeniorityModified = (equipment) => async (dispatch) => {
  return await dispatch(log(equipment.historyEntity, EQUIPMENT_SENIORITY_MODIFIED));
};

export const equipmentStatusModified = (equipment, status, statusComment) => async (dispatch) => {
  return await dispatch(log(
    equipment.historyEntity,
    EQUIPMENT_STATUS_MODIFIED,
    { description: status },
    { description: statusComment }
  ));
};

export const equipmentDocumentAdded = (equipment, document) => async (dispatch) => {
  return await dispatch(log(equipment.historyEntity, EQUIPMENT_DOCUMENT_ADDED, {
    description: document,
  }));
};

export const equipmentDocumentsAdded = (equipment) => async (dispatch) => {
  return await dispatch(log(equipment.historyEntity, EQUIPMENT_DOCUMENTS_ADDED));
};

export const equipmentDocumentDeleted = (equipment, document) => async (dispatch) => {
  return await dispatch(log(equipment.historyEntity, EQUIPMENT_DOCUMENT_DELETED, document.historyEntity));
};

export const equipmentAttachmentAdded = (equipment, attachment) => async (dispatch) => {
  return await dispatch(log(equipment.historyEntity, EQUIPMENT_ATTACHMENT_ADDED, {
    description: attachment,
  }));
};

export const equipmentAttachmentUpdated = (equipment, attachment) => async (dispatch) => {
  return await dispatch(log(equipment.historyEntity, EQUIPMENT_ATTACHMENT_UPDATED, {
    description: attachment,
  }));
};

export const equipmentAttachmentDeleted = (equipment, attachment) => async (dispatch) => {
  return await dispatch(log(equipment.historyEntity, EQUIPMENT_ATTACHMENT_DELETED, {
    description: attachment,
  }));
};

export const rentalRequestAdded = (rentalRequest) => async (dispatch) => {
  return await dispatch(log(rentalRequest.historyEntity, RENTAL_REQUEST_ADDED));
};

export const rentalRequestModified = (rentalRequest) => async (dispatch) => {
  return await dispatch(log(rentalRequest.historyEntity, RENTAL_REQUEST_MODIFIED));
};

export const rentalRequestDocumentAdded = (rentalRequest, document) => async (dispatch) => {
  return await dispatch(log(rentalRequest.historyEntity, RENTAL_REQUEST_DOCUMENT_ADDED, {
    description: document,
  }));
};

export const rentalRequestDocumentsAdded = (rentalRequest) => async (dispatch) => {
  return await dispatch(log(rentalRequest.historyEntity, RENTAL_REQUEST_DOCUMENTS_ADDED));
};

export const rentalRequestDocumentDeleted = (rentalRequest, document) => async (dispatch) => {
  return await dispatch(log(rentalRequest.historyEntity, RENTAL_REQUEST_DOCUMENT_DELETED, document.historyEntity));
};

export const rentalRequestEquipmentHired = (rentalRequest, equipment, offerResponse) => async (dispatch) => {
  return await dispatch(log(rentalRequest.historyEntity, RENTAL_REQUEST_EQUIPMENT_HIRED, equipment.historyEntity, {
    description: offerResponse,
  }));
};
