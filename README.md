# user-microservices

A demo microservices application with Angular , .NET Core , RabbitMq and MySql

# Tech Stack

1. Angular 8
2. .NET Core 3.1
3. MySQL
4. RabbitMq 

# Setting up

1. Install Node Angular CLI.
2. Install .NET Core 3.1 Runtime ( sdk in case of development).
3. Install MySql.
4. Create a mysql database. ( create database user_service_data and admin_service_data).
5. The /db folder has tables. Run the table queries first to create necessary tables.
6. The password and other db details are not stored in the project for security reasons. They are read from environment variables. If you have trouble setting environment variables, update launchsettings.json file.

Below are the environment variables used.

##
"users_repo_password": "Password",
"users_repo_database": "admin_service_data",
"users_repo_username": "root",
"users_repo_host": "localhost"

"users_repo_password": "Password",
"users_repo_database": "user_service_data",
"users_repo_username": "root",
"users_repo_host": "localhost"

Note : The admin and users repository is different . They can be different credentials. In the demo application, the keys are the same but the values in lauchsettings will be different.

7. The application needs RabbitMq for inter service communication. running docker desktop would be the easiest way to set up. Install docker and run the below to start the rabbitmq container.

## Running RabbitMq container

1. docker run -d --hostname my-rabbit --name some-rabbit -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password rabbitmq:3-management
2. docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

This will start the RabbitMq client with guest and guest as username and password.

# Building the application

The API can be built and run via

1.dotnet build and run, 
2.docker build and run or 
3.Using visual studio by opening the solution file.

The user service will run on port 5000 and the admin service will run on 5001. The UI runs on localhost:4200. 

The UI can be run by navigating to the UI folder and runnig npm install follwed by ng serve.

Note : Currently, the application depends on RabbitMq for communication. Please run the RabbitMq container before the API starts. Ideally this would be handled with docker compose or using container orchestration by handling dependencies.

Also, There might be issues with running multiple projects in visual studio. Once the solution is run, navigate to localhost:5000 and localhost:5001 on chrome just to enable debugging ( debugger points might not work sometimes with multi project solutions).

# Feature

The application consists of a user microservice and an admin microservice. The database and api for both microservices are different and they communicate via Messages.

## The user microservice performs the following functions

1. Add a new user (with validations).
2. View all users. 
3. Search , Sort and Pagination features.
4. On User Addition, Modification and Deletion, Raise Events which can be read by other microservices.
5. The user microservices uses a user_queue in RabbitMq.
6. Handle UserNotified event.
7. Cors Enabled.
8. Operations in non blocking mode and include transactions.


## The admin microservice performs the following functions

1. View all users.
2. Notify all users.
3. Handle User Addition, Modification and Deletion events. 
4. Raise Notify user event.
5. Cors enabled.
6. Operations in non blocking mode and include transactions.
7. Handle events asynchronously.

# Note 

1. This is a demo application and does not involve stability patterns such as Circuit-Breaker etc. But this can be easily implemented.
2. RabbitMq is a dependency, but can be easily handled with docker compose. The API already is containerized.
