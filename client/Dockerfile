FROM image-registry.openshift-image-registry.svc:5000/e0cee6-tools/node:1 AS builder
LABEL maintainer="young-jin.chung@gov.bc.ca"

COPY . /src

RUN cd /src && npm ci && \
  ./node_modules/.bin/gulp --production --commit=$OPENSHIFT_BUILD_COMMIT

FROM image-registry.openshift-image-registry.svc:5000/e0cee6-tools/nginx-116-rhel8:1

COPY --from=builder /src/dist/. /tmp/src
COPY --from=builder /src/nginx-start/. /tmp/src/nginx-start
COPY --from=builder /src/nginx.conf.tmpl /tmp/src/nginx.conf.tmpl

RUN $STI_SCRIPTS_PATH/assemble

CMD $STI_SCRIPTS_PATH/run