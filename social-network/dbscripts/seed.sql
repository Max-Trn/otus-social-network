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
  text VARCHAR (1000) NOT NULL
);

CREATE TABLE dialog (
     user_id integer NOT NULL, 
     friend_id integer NOT NULL, 
     text VARCHAR (1000),
     shard_key integer NOT NULL
)
PARTITION BY HASH (shard_key);

--shards
CREATE EXTENSION postgres_fdw;
CREATE SERVER shard_0 FOREIGN DATA WRAPPER postgres_fdw OPTIONS (dbname 'socialnetwork', host 'postgres_shard0', port '5435');
CREATE SERVER shard_1 FOREIGN DATA WRAPPER postgres_fdw OPTIONS (dbname 'socialnetwork', host 'postgres_shard1', port '5436');
CREATE USER MAPPING FOR POSTGRES SERVER shard_0 OPTIONS (user 'postgres', password 'password');
CREATE USER MAPPING FOR POSTGRES SERVER shard_1 OPTIONS (user 'postgres', password 'password');
CREATE FOREIGN TABLE dialog_0 PARTITION OF dialog FOR VALUES WITH (MODULUS 2, REMAINDER 0) SERVER shard_0;
CREATE FOREIGN TABLE dialog_1 PARTITION OF dialog FOR VALUES WITH (MODULUS 2, REMAINDER 1) SERVER shard_1;
