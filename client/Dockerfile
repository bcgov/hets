FROM node:24-alpine@sha256:8a32c8cf649a49915a66e81e7a609223c708d0adabf288ee3f4077d765542f85 AS build
WORKDIR /src
COPY package.json package-lock.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM nginx:1.30.6-alpine@sha256:0e52bd054126cdc040b1276c0af347aeb02a41b9cfe305d6094767ac30e97949 AS runtime
RUN apk add --no-cache jq \
    && rm /etc/nginx/conf.d/default.conf
COPY --from=build /src/build/ /usr/share/nginx/html/
COPY nginx.conf.tmpl /opt/hets/nginx.conf.tmpl
COPY container-entrypoint.sh /usr/local/bin/hets-client-start
RUN chgrp -R 0 /usr/share/nginx/html /etc/nginx \
    && chmod -R g=u /usr/share/nginx/html /etc/nginx \
    && chmod 755 /usr/local/bin/hets-client-start
USER 1001:0
EXPOSE 8080
ENTRYPOINT ["/usr/local/bin/hets-client-start"]
