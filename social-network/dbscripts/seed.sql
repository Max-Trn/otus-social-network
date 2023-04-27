CREATE TABLE users (
 id SERIAL PRIMARY KEY,
 biography VARCHAR (200) NOT NULL,
 password VARCHAR (200) NOT NULL,
 city VARCHAR (50) NOT NULL,
 first_name VARCHAR (50) NOT NULL,
 second_name VARCHAR (50) NOT NULL,
 age integer NOT NULL
);

CREATE TABLE friend (
  id SERIAL PRIMARY KEY,
  user_id integer,
  friend_id integer
);

CREATE TABLE posts (
  id SERIAL PRIMARY KEY,
  user_id integer,
  text VARCHAR (1000) NOT NULL,
);