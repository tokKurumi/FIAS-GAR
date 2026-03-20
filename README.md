# FIAS GAR

Система для загрузки и поиска адресных данных ФИАС ГАР.

## Описание

Проект состоит из трех сервисов на базе ASP.NET Core 8:

- **ReaderApi** - загружает данные из XML архивов в PostgreSQL
- **SearchApi** - обеспечивает поиск по Elasticsearch
- **AppHost** - оркестрирует контейнеры (PostgreSQL, Elasticsearch, Logstash)

## Основные зависимости

- .NET 8.0
- PostgreSQL
- Elasticsearch 8.9.1
- Logstash 8.13.4

## Структура

```
src/
├── GAR.AppHost/          Хост приложение и оркестрация
├── GAR.Services.ReaderApi/   Сервис чтения и загрузки данных
├── GAR.Services.SearchApi/   Сервис поиска
├── GAR.ServiceDefaults/  Общие конфигурации
├── GAR.XmlReaderCopyHelper/  Утилита для работы с XML
├── elasticsearch/        Конфиг Elasticsearch
├── logstash/            Конфиг Logstash
└── sql/                 SQL скрипты
```

## Требования

- Docker и Docker Compose для контейнеризации
- .NET 8.0 SDK

## Запуск

```bash
dotnet run --project src/GAR.AppHost/GAR.AppHost.csproj
```

Проект автоматически запустит все необходимые контейнеры и сервисы.
