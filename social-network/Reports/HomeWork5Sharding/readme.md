<h1>Домашнее задание 5. Шардирование</h1>

В проекте реализовано шардирование через партиционирование во внешние базы данных следущим кодом

```
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
```

ключом шардирование выбрана сумма id отправителя и получателя, остаток от деления определяет, в какой ходим шард


<h2>Запуск<h2>

в папке с проектом social-network

docker-compose up -d

в папке с проектом social-network

docker-compose user-posts-async-api

 - http://localhost:7285/swagger/index.html
   зарегистрировать 1 пользователя
   авторизоваться с токеном пользователя 1


- отправить сообщения ручкой dialog/send
- 
- получить сообщения ручкой dialog/send