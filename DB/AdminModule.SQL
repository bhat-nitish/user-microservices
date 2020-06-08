				
create database admin_service_data;
				
create table Users(USER_ID int auto_increment,
                USER_NAME varchar(512) not null,
                EMAIL_ADDRESS varchar(320) not null ,
                DATE_OF_BIRTH datetime not null,
                CREATED_ON  datetime not null,
                UPDATED_ON datetime not null,
                CREATED_BY varchar(512) not null,
                UPDATED_BY varchar(512) not null,
                primary key(USER_ID))
				
create table IntegrationEvents(EVENT_ID int auto_increment,
                EVENT_TYPE varchar(100) not null,
                EVENT_NAME varchar(10000) not null ,
                EVENT_STATUS varchar(100) not null ,
                CREATED_ON  datetime not null,
                UPDATED_ON datetime not null,
                CREATED_BY varchar(512) not null,
                UPDATED_BY varchar(512) not null,
                primary key(EVENT_ID))	

create table UserNotifications(NOTIFICATION_ID int auto_increment,
                EMAIL varchar(320) not null,
                NOTIFICATION varchar(10000) not null ,
                NOTIFICATION_STATUS varchar(100) not null ,
                CREATED_ON  datetime not null,
                UPDATED_ON datetime not null,
                CREATED_BY varchar(512) not null,
                UPDATED_BY varchar(512) not null,
                primary key(NOTIFICATION_ID))				