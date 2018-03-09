import React from 'react';

import * as Api from './api';


// History Entity Types
export const OWNER = 'Owner';
export const PROJECT = 'Project';
export const EQUIPMENT = 'Equipment';
export const REQUEST = 'Request';
export const USER = 'User';
export const ROLE = 'Role';
export const CONTACT = 'Contact';
export const DOCUMENT = 'Document';

// History Events
export const OWNER_ADDED = 'Owner %e was added.';
export const OWNER_MODIFIED = 'Owner %e was modified.';
export const OWNER_MODIFIED_STATUS = 'Owner %e status is %e.';
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
export const PROJECT_MODIFIED_STATUS = 'Project %e status is %e.';
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
export const EQUIPMENT_STATUS_MODIFIED = 'Equipment %e status is %e.';
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
export function log(historyEntity, event, ...entities) {
  // prepend the 'parent' entity
  entities.unshift(historyEntity);

  // Build the history text
  var historyText = JSON.stringify({
    // The event text, with entity placeholders
    text: event,
    // The array of entities
    entities: entities,
  });

  // Choose the correct API call.
  var addHistoryPromise = null;

  switch (historyEntity.type) {
    case OWNER:
      addHistoryPromise = Api.addOwnerHistory;
      break;

    case PROJECT:
      addHistoryPromise = Api.addProjectHistory;
      break;

    case EQUIPMENT:
      addHistoryPromise = Api.addEquipmentHistory;
      break;

    case REQUEST:
      addHistoryPromise = Api.addRentalRequestHistory;
      break;

    case USER:
    case ROLE:
      break;
  }

  if (addHistoryPromise) {
    return addHistoryPromise(historyEntity.id, {
      historyText: historyText,
    });
  }

  return null;
}

function buildLink(entity, closeFunc) {
  // Return a link if the entity has a path; just the description otherwise.
  return entity.path ? <a onClick={ closeFunc } href={ entity.url }>{ entity.description }</a> : <span>{ entity.description }</span>;
}

export function renderEvent(historyText, closeFunc) {
  try {
    // Unwrap the JSONed event
    var event = JSON.parse(historyText);

    // Parse the text and return it inside a <div>, replacing field placeholders with linked content.
    var tokens = event.text.split('%e');
    return <div>
      {
        tokens.map((token, index) => {
          return <span key={ index }>
            { token }
            { index < tokens.length - 1 ? buildLink(event.entities[index], closeFunc || null) : null }
          </span>;
        })
      }
    </div>;
  } catch (err) {
    // Not JSON so just push out what's in there.
    return <span>{ historyText }</span>;
  }
}

export function get(historyEntity, offset, limit) {
  // If not showing all, then just fetch the first 10 entries
  var params = {
    offset: offset || 0,
    limit: limit || null,
  };

  switch (historyEntity.type) {
    case OWNER:
      return Api.getOwnerHistory(historyEntity.id, params);

    case PROJECT:
      return Api.getProjectHistory(historyEntity.id, params);

    case EQUIPMENT:
      return Api.getEquipmentHistory(historyEntity.id, params);

    case REQUEST:
      return Api.getRentalRequestHistory(historyEntity.id, params);

    case USER:
    case ROLE:
      break;
  }

  return null;
}

// Logging 
export function ownerAdded(owner) {
  return log(owner.historyEntity, OWNER_ADDED);
}

export function ownerModified(owner) {
  return log(owner.historyEntity, OWNER_MODIFIED);
}

export function ownerModifiedStatus(owner, status) {
  return log(owner.historyEntity, OWNER_MODIFIED_STATUS, { description: status });
}

export function ownerContactAdded(owner, contact) {
  return log(owner.historyEntity, OWNER_CONTACT_ADDED, contact.historyEntity);
}

export function ownerContactUpdated(owner, contact) {
  return log(owner.historyEntity, OWNER_CONTACT_UPDATED, contact.historyEntity);
}

export function ownerContactDeleted(owner, contact) {
  return log(owner.historyEntity, OWNER_CONTACT_DELETED, contact.historyEntity);
}

export function ownerModifiedPolicy(owner) {
  return log(owner.historyEntity, OWNER_MODIFIED_POLICY);
}

export function ownerEquipmentAdded(owner, equipment) {
  return log(owner.historyEntity, OWNER_EQUIPMENT_ADDED, equipment.historyEntity);
}

export function ownerEquipmentVerified(owner, equipment) {
  return log(owner.historyEntity, OWNER_EQUIPMENT_VERIFIED, equipment.historyEntity);
}

export function ownerDocumentAdded(owner, document) {
  return log(owner.historyEntity, OWNER_DOCUMENT_ADDED, { description: document });
}

export function ownerDocumentsAdded(owner) {
  return log(owner.historyEntity, OWNER_DOCUMENTS_ADDED);
}

export function ownerDocumentDeleted(owner, document) {
  return log(owner.historyEntity, OWNER_DOCUMENT_DELETED, document.historyEntity);
}

export function projectAdded(project) {
  return log(project.historyEntity, PROJECT_ADDED);
}

export function projectModified(project) {
  return log(project.historyEntity, PROJECT_MODIFIED);
}

export function projectContactAdded(project, contact) {
  return log(project.historyEntity, PROJECT_CONTACT_ADDED, contact.historyEntity);
}

export function projectContactUpdated(project, contact) {
  return log(project.historyEntity, PROJECT_CONTACT_UPDATED, contact.historyEntity);
}

export function projectContactDeleted(project, contact) {
  return log(project.historyEntity, PROJECT_CONTACT_DELETED, contact.historyEntity);
}

export function projectDocumentAdded(project, document) {
  return log(project.historyEntity, PROJECT_DOCUMENT_ADDED, { description: document });
}

export function projectDocumentsAdded(project) {
  return log(project.historyEntity, PROJECT_DOCUMENTS_ADDED);
}

export function projectDocumentDeleted(project, document) {
  return log(project.historyEntity, PROJECT_DOCUMENT_DELETED, document.historyEntity);
}

export function projectEquipmentReleased(project, equipment) {
  return log(project.historyEntity, PROJECT_EQUIPMENT_RELEASED, equipment.historyEntity);
}

export function projectRentalRequestAdded(project, rentalRequest) {
  return log(project.historyEntity, PROJECT_RENTAL_REQUEST_ADDED, rentalRequest.historyEntity);
}

export function equipmentAdded(equipment) {
  return log(equipment.historyEntity, EQUIPMENT_ADDED);
}

export function equipmentModified(equipment) {
  return log(equipment.historyEntity, EQUIPMENT_MODIFIED);
}

export function equipmentSeniorityModified(equipment) {
  return log(equipment.historyEntity, EQUIPMENT_SENIORITY_MODIFIED);
}

export function equipmentStatusModified(equipment, status) {
  return log(equipment.historyEntity, EQUIPMENT_STATUS_MODIFIED, { description: status });
}

export function equipmentDocumentAdded(equipment, document) {
  return log(equipment.historyEntity, EQUIPMENT_DOCUMENT_ADDED, { description: document });
}

export function equipmentDocumentsAdded(equipment) {
  return log(equipment.historyEntity, EQUIPMENT_DOCUMENTS_ADDED);
}

export function equipmentDocumentDeleted(equipment, document) {
  return log(equipment.historyEntity, EQUIPMENT_DOCUMENT_DELETED, document.historyEntity);
}

export function equipmentAttachmentAdded(equipment, attachment) {
  return log(equipment.historyEntity, EQUIPMENT_ATTACHMENT_ADDED, { description: attachment });
}

export function equipmentAttachmentUpdated(equipment, attachment) {
  return log(equipment.historyEntity, EQUIPMENT_ATTACHMENT_UPDATED, { description: attachment });
}

export function equipmentAttachmentDeleted(equipment, attachment) {
  return log(equipment.historyEntity, EQUIPMENT_ATTACHMENT_DELETED, { description: attachment });
}

export function rentalRequestAdded(rentalRequest) {
  return log(rentalRequest.historyEntity, RENTAL_REQUEST_ADDED);
}

export function rentalRequestModified(rentalRequest) {
  return log(rentalRequest.historyEntity, RENTAL_REQUEST_MODIFIED);
}

export function rentalRequestDocumentAdded(rentalRequest, document) {
  return log(rentalRequest.historyEntity, RENTAL_REQUEST_DOCUMENT_ADDED, { description: document });
}

export function rentalRequestDocumentsAdded(rentalRequest) {
  return log(rentalRequest.historyEntity, RENTAL_REQUEST_DOCUMENTS_ADDED);
}

export function rentalRequestDocumentDeleted(rentalRequest, document) {
  return log(rentalRequest.historyEntity, RENTAL_REQUEST_DOCUMENT_DELETED, document.historyEntity);
}

export function rentalRequestEquipmentHired(rentalRequest, equipment, offerResponse) {
  console.log(equipment);
  return log(rentalRequest.historyEntity, RENTAL_REQUEST_EQUIPMENT_HIRED, equipment.historyEntity, { description: offerResponse });
}