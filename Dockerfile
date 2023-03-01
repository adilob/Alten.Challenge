FROM nginx
EXPOSE 8080
RUN rm /etc/nginx/nginx.conf
COPY nginx.conf /etc/nginx/nginx.conf