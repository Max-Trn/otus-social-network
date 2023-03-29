<h1>Домашнее задание 2. Индексы</h1>
<h3>Без индекса</h3>
<b>Samples:1000</b>

| Number of Threads | 1       | 10       | 100       |
|-------------------|---------|----------|-----------|
| Throughput        | 6.2/sec | 10.7/sec | 10.25/sec |
| Latency           | 85      | 472      | 5895      |
|                   |         |          |           |

<h3>С индексом</h3>
<b>Samples:15000</b>

| Number of Threads | 1         | 10        | 100       |
|-------------------|-----------|-----------|-----------|
| Throughput        | 139.8/sec | 197.4/sec | 262.3/sec |
| Latency           | 2.4       | 15.6      | 173.5     |
|                   |           |           |           |


```
CREATE INDEX users_idx ON users USING btree (first_name text_pattern_ops, second_name text_pattern_ops)
```

<h3> Explain </h3>

```
BITMAP_INDEX_SCAN (bitmap index scan)	 index: users_idx; Rows: 523 Total Cost: 288.67 Startup Cost: 0.0	Parent Relationship = Outer;
Parallel Aware = false;
Async Capable = false;
Plan Width = 0;
Index Cond = (((first_name)::text ~>=~ 'Лев'::text) AND ((first_name)::text ~<~ 'Лег'::text) AND ((second_name)::text ~>=~ 'В'::text) AND ((second_name)::text ~<~ 'Г'::text));

```
ВTree индекс выбран т.к. с другим запросы не попадают в индекс, также Id не включен т.к. его включение не даст прироста в скорости
