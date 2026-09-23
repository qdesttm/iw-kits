FROM nginx:alpine

WORKDIR /usr/share/nginx/html

COPY builds/Client/ .

COPY src/Client/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80