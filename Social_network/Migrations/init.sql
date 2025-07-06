CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    birth_date DATE,
    gender VARCHAR(10),
    interests TEXT,
    city VARCHAR(100),
    email VARCHAR(200) UNIQUE,
    password_hash VARCHAR(500)
);