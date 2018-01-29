Cerberus Route
======================

This is a legacy route.  The external application URLs that route traffic through SiteMinder and on to the application, route through the `https://cerberus-tran-hets*` routes.  Right now the `https://frontend-tran-hets-*` and `https://cerberus-tran-hets*` routes both route through to the `frontend` service.

SiteMinder's dependency on this route should be broken, and once it is the route and related configuration can be deleted.

For now it's here for backward compatibility.