**Чтобы запустить проект:**
1. Поднять контейнеры
```docker compose up -d```
2. Зайти через браузер на
```localhost:5179```

**Обьяснение:**
- Клиент 1 отправляет message и сохранает в базу postgres
- Клинет 2 получет message в реальном времени 
- Клиент 3 получет все сообщения из базы postgres

**Замечание**
- При первом запуске выполняется init.sql (поэтому нужно подождать несколько секунд)
