# Social Network E-Commerce API

ASP.NET Core Web API для социальной сети с функционалом электронной коммерции, построенная по принципам Clean Architecture.

## Архитектура

- **Presentation Layer**: Controllers
- **Application Layer**: Services (бизнес-логика)
- **Domain Layer**: Entities (модели данных)
- **Infrastructure Layer**: Repositories (доступ к данным)

## Технологии

- .NET 8
- ASP.NET Core Web API
- PostgreSQL
- Dapper (ORM)
- JWT аутентификация
- Swagger/OpenAPI документация
- Docker

## Сущности

- User (пользователи)
- Role (роли)
- Category (категории товаров)
- Product (товары)
- ProductImage (изображения товаров)
- Inventory (складские остатки)
- Cart (корзины)
- CartItem (элементы корзины)
- Order (заказы)
- OrderItem (элементы заказа)
- Payment (платежи)
- Address (адреса)
- AuditLog (логи аудита)

## API Endpoints

### Аутентификация

- `POST /api/auth/register` - регистрация пользователя
- `POST /api/auth/login` - вход в систему
- `POST /api/auth/refresh` - обновление токена
- `POST /api/auth/logout` - выход из системы

### Каталог

- `GET /api/categories` - получение всех категорий
- `GET /api/products` - получение товаров с фильтрацией
- `GET /api/products/{id}` - получение товара по ID

### Пользователи

- `GET /api/users/me` - получение информации о текущем пользователе
- `PUT /api/users/me` - обновление информации о пользователе
- `GET /api/users/me/orders` - получение заказов пользователя

### Корзина

- `GET /api/cart` - получение текущей корзины
- `POST /api/cart/items` - добавление товара в корзину
- `PUT /api/cart/items/{id}` - изменение количества товара
- `DELETE /api/cart/items/{id}` - удаление товара из корзины

### Заказы

- `POST /api/orders` - создание заказа из корзины
- `GET /api/orders/{id}` - получение информации о заказе

### Платежи

- `POST /api/payments/create` - создание платежа
- `POST /api/payments/webhook` - webhook для callback от платежного провайдера

## Запуск приложения

### Требования

- .NET 8 SDK
- PostgreSQL
- Docker (опционально)

### Локальный запуск

1. Настройте строку подключения к БД в `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=social_network;Username=postgres;Password=password"
  }
}
```

2. Выполните миграции:
```sql
\i Social_network/Migrations/create_tables.sql
```

3. Запустите приложение:
```bash
dotnet run
```

4. Откройте Swagger UI: https://localhost:64148/swagger

### Docker запуск

```bash
docker-compose up -d
```

## Примеры запросов

### Регистрация пользователя
```bash
POST /api/auth/register
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "password123",
  "birthDate": "1990-01-01",
  "gender": "male",
  "interests": "programming, music",
  "city": "Moscow"
}
```

### Получение товаров с фильтрацией
```bash
GET /api/products?categoryId=4&q=macbook&priceFrom=2000&priceTo=3000&page=1&size=10&sort=price&desc=true
```

## Безопасность

- JWT аутентификация
- HTTPS в продакшене
- Валидация входных данных
- CORS политики
- Защита от SQL-инъекций через параметризованные запросы

## Мониторинг и логирование

- Structured logging через Serilog (планируется)
- Метрики Prometheus (планируется)
- Аудит действий пользователей
