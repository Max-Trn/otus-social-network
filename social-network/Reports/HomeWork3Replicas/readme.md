<h1>Домашнее задание 3. Репликация</h1>
1.1 настроил в postgresql.conf на мастере на репликацию

```
ssl = off
wal_level = replica
max_wal_senders = 4 # expected slave num
```
1.2 Добавил новую replica базу в docker-compose 

```
  postgres_image_repl:
    image: postgres
    ports:
      - "5433:5433"
    restart: always
    volumes:
      - ./dbVolumes/pgslave:/var/lib/postgresql/data
    environment:
      POSTGRES_USER: "postgres"
      POSTGRES_PASSWORD: "password"
      POSTGRES_DB: "socialnetwork"
    networks:
      - social-network
```
1.3 Сделал бэкап мастера для реплики

1.3 Добавил standby.signal чтобы база включилась в режим реплики

1.4 Прописал в pg_hba.conf мастера адрес реплики

1.5 Прописал в postgresql.conf у реплики путь до мастера

1.6 Запустил и проверил репликацию

2. Перевел read запросы на реплику в сервисе. Код в данном коммите

3. Провел нагрузочный тест до репликации и после. Убедился, что после репликации нагрузка мастера упала и запросы на чтение идут в реплику

4. Добавил новую replica базу без standby.signal

5.1  Выставил у мастера wal_level = logical
5.2  Создал публикацию на мастере

```
GRANT CONNECT ON DATABASE postgres TO replicator;
GRANT SELECT ON ALL TABLES IN SCHEMA public TO replicator;
create publication pg_pub for ALL TABLES
```
5.3 Добавил подписку у новой реплики

```
CREATE SUBSCRIPTION pg_sub CONNECTION 'host=postgres_image port=5432 user=replicator password=pass dbname=socialnetwork' PUBLICATION pg_pub
```

5.4 Убедился, что репликация работает, а добавление элементов с одинаковым айди вызывает конфликты

7. Настроил кворумную репликацию с помощью

```
synchronous_standby_names = 'ANY 1 (pgslave2, pgslave)' 
```

8. Создал нагрузку на запись

9. В процессе записи убил мастер

10. Записано 11к записей

11. Запромоутил slave2 командой select * from pg_promote() и настроил реплику slave на slave2

12. Потерь данных не обнаружено


