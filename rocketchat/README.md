# Rocket.Chat Integrations

The following section describes the HETS environments and resources which are integrated with Rocket.Chat to provide feeds, notifications, and alerts.

Rocket.Chat instance(s):

- https://chat.developer.gov.bc.ca/

## Backups

Notifications and alerts from backup-containers.

Channel:

- moti-db-backups

Integration Script:

- https://github.com/bcgov/hets/rocketchat/rocketchat-script

Environments:

- [Hired Equipment Tracking System (dev)](https://console.apps.silver.devops.gov.bc.ca/k8s/cluster/projects/e0cee6-dev)
- [Hired Equipment Tracking System (test)](https://console.apps.silver.devops.gov.bc.ca/k8s/cluster/projects/e0cee6-test)
- [Hired Equipment Tracking System (prod)](https://console.apps.silver.devops.gov.bc.ca/k8s/cluster/projects/e0cee6-prod)

### Implementation (As of 20 Aug 2021)

- Works with [BCDevOps Backup Container](https://github.com/BCDevOps/backup-container)
- Create channel in Rocketchat
- Go to Administration - Integrations - Incoming - +New
- Fill out Rocketchat form. The rocketchat-script.js goes into this form under script (Be sure to enable two sliders - At top and by the script box)
- Once saved take note of the Webhook URL
- Create an openshift secret and reference the Webhook URL.
- Webhook URL secret will be used by [backup-deploy-config.yaml](https://github.com/bcgov/hets/blob/1.9.3/openshift/backup-deploy-config.yaml)
