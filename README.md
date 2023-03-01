# Alten.Challenge

## Run the project

1. Download the repository
2. Make sure you have docker and docker-compose installed
3. Run it through the docker-compose like below:
  
  docker compose up --scale bookingapi=4 -d --build
  
- The project has tests to run
- The project has two files to import into Postman to test
  - collection (Alten.postman_collection.json)
  - environment (Alten.postman_environment.json)
  
- When running through Postman, make sure you set the correct "url" in the environment and you can call the operations.

Example of url value ==> http://localhost:8090

So, after the application is up and running, you can just navigate to:

http://localhost:8090


Last but not least, you need to set a new entry into your hosts files like below:

127.0.0.1 bookingapi.local

This is the url/name configured for the Nginx server that was used in the project.

The purpose to use the Nginx was to emulate a load balancer.

In the real life, you would use an orchestrator like Kubernetes, deploy your application to cloud an scale it.

Thanks!
