FROM image-registry.openshift-image-registry.svc:5000/e0cee6-tools/node:1 AS builder
LABEL maintainer="young-jin.chung@gov.bc.ca"

ENV NODE_ENV production

COPY . /src

RUN cd /src/Client && npm ci && \
  ./node_modules/.bin/gulp --production --commit=$OPENSHIFT_BUILD_COMMIT

FROM image-registry.openshift-image-registry.svc:5000/e0cee6-tools/dotnet-21-rhel7:2.1-41

COPY --from=builder /src/Client/dist /opt/app-root/dist

COPY --from=builder . /opt/app-root/

RUN $STI_SCRIPTS_PATH/assemble

CMD $STI_SCRIPTS_PATH/run