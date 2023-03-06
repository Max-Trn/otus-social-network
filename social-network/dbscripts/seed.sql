CREATE TABLE users (
 id SERIAL PRIMARY KEY,
 biography VARCHAR (200) NOT NULL,
 password VARCHAR (50) NOT NULL,
 city VARCHAR (50) NOT NULL,
 first_name VARCHAR (50) NOT NULL,
 second_name VARCHAR (50) NOT NULL,
 age integer NOT NULL
);