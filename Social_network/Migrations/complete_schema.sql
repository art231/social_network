-- Complete database schema for Social Network E-Commerce

-- Users table (from init.sql)
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255),
    email VARCHAR(200) UNIQUE NOT NULL,
    password_hash VARCHAR(500) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Roles table (from init.sql)
CREATE TABLE IF NOT EXISTS roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL
);

-- User roles table (from init.sql)
CREATE TABLE IF NOT EXISTS user_roles (
    user_id INTEGER REFERENCES users(id) ON DELETE CASCADE,
    role_id INTEGER REFERENCES roles(id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);

-- Insert default roles (from init.sql)
INSERT INTO roles (name) VALUES 
('user'),
('admin')
ON CONFLICT (name) DO NOTHING;

-- Categories table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    parent_id INTEGER REFERENCES categories(id) ON DELETE SET NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Products table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS products (
    id SERIAL PRIMARY KEY,
    sku VARCHAR(100) UNIQUE NOT NULL,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10,2) NOT NULL,
    currency VARCHAR(3) DEFAULT 'USD',
    category_id INTEGER NOT NULL REFERENCES categories(id) ON DELETE CASCADE,
    active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Product images table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS product_images (
    id SERIAL PRIMARY KEY,
    product_id INTEGER NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    url TEXT NOT NULL,
    sort_order INTEGER DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Inventory table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS inventory (
    product_id INTEGER PRIMARY KEY REFERENCES products(id) ON DELETE CASCADE,
    quantity INTEGER NOT NULL DEFAULT 0,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Carts table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS carts (
    id SERIAL PRIMARY KEY,
    user_id INTEGER REFERENCES users(id) ON DELETE SET NULL,
    session_token VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Cart items table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS cart_items (
    id SERIAL PRIMARY KEY,
    cart_id INTEGER NOT NULL REFERENCES carts(id) ON DELETE CASCADE,
    product_id INTEGER NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    quantity INTEGER NOT NULL DEFAULT 1,
    price_snapshot DECIMAL(10,2) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Addresses table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS addresses (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    text TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Orders table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS orders (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    total DECIMAL(10,2) NOT NULL,
    status VARCHAR(50) DEFAULT 'pending',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    address_id INTEGER NOT NULL REFERENCES addresses(id),
    payment_id INTEGER
);

-- Order items table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS order_items (
    id SERIAL PRIMARY KEY,
    order_id INTEGER NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    product_id INTEGER NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    quantity INTEGER NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Payments table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS payments (
    id SERIAL PRIMARY KEY,
    order_id INTEGER NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    provider VARCHAR(100) NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    status VARCHAR(50) DEFAULT 'pending',
    external_id VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Audit logs table (from create_tables.sql)
CREATE TABLE IF NOT EXISTS audit_logs (
    id SERIAL PRIMARY KEY,
    entity VARCHAR(100) NOT NULL,
    action VARCHAR(50) NOT NULL,
    payload JSONB,
    actor VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create indexes for better performance (from create_tables.sql)
CREATE INDEX IF NOT EXISTS idx_products_category_active ON products(category_id, active);
CREATE INDEX IF NOT EXISTS idx_products_title_description ON products USING gin(to_tsvector('english', title || ' ' || description));
CREATE INDEX IF NOT EXISTS idx_inventory_product_id ON inventory(product_id);
CREATE INDEX IF NOT EXISTS idx_cart_items_cart_id ON cart_items(cart_id);
CREATE INDEX IF NOT EXISTS idx_orders_user_id ON orders(user_id);
CREATE INDEX IF NOT EXISTS idx_order_items_order_id ON order_items(order_id);
CREATE INDEX IF NOT EXISTS idx_payments_order_id ON payments(order_id);
CREATE INDEX IF NOT EXISTS idx_audit_logs_created_at ON audit_logs(created_at);

-- Add foreign key constraint for orders.payment_id (from create_tables.sql)
ALTER TABLE orders ADD CONSTRAINT fk_orders_payment_id FOREIGN KEY (payment_id) REFERENCES payments(id);

-- Insert sample categories (from create_tables.sql)
INSERT INTO categories (name, parent_id) VALUES
('Electronics', NULL),
('Computers', 1),
('Smartphones', 1),
('Laptops', 2),
('Desktops', 2),
('Clothing', NULL),
('Men''s Clothing', 6),
('Women''s Clothing', 6),
('Shoes', NULL),
('Books', NULL)
ON CONFLICT DO NOTHING;

-- Insert sample products (from create_tables.sql)
INSERT INTO products (sku, title, description, price, category_id) VALUES
('LAP001', 'MacBook Pro 16"', 'Apple MacBook Pro 16-inch with M3 Pro chip', 2499.99, 4),
('LAP002', 'Dell XPS 15', 'Dell XPS 15 laptop with Intel i7 processor', 1799.99, 4),
('PHN001', 'iPhone 15 Pro', 'Apple iPhone 15 Pro with 128GB storage', 999.99, 3),
('PHN002', 'Samsung Galaxy S24', 'Samsung Galaxy S24 with 256GB storage', 849.99, 3),
('DSK001', 'Gaming Desktop', 'High-performance gaming desktop with RTX 4080', 2999.99, 5),
('TSH001', 'Cotton T-Shirt', 'Premium cotton t-shirt for men', 29.99, 7),
('TSH002', 'Women''s Blouse', 'Elegant women''s blouse', 39.99, 8),
('SHO001', 'Running Shoes', 'Comfortable running shoes for all terrains', 89.99, 9),
('BOK001', 'Clean Code', 'Clean Code: A Handbook of Agile Software Craftsmanship', 39.99, 10),
('BOK002', 'Design Patterns', 'Design Patterns: Elements of Reusable Object-Oriented Software', 49.99, 10)
ON CONFLICT (sku) DO NOTHING;

-- Insert sample inventory (from create_tables.sql)
INSERT INTO inventory (product_id, quantity) VALUES
(1, 10),
(2, 15),
(3, 25),
(4, 20),
(5, 5),
(6, 100),
(7, 80),
(8, 50),
(9, 30),
(10, 25)
ON CONFLICT (product_id) DO NOTHING;

-- Insert sample product images (from create_tables.sql)
INSERT INTO product_images (product_id, url, sort_order) VALUES
(1, '/images/macbook-pro-1.jpg', 1),
(1, '/images/macbook-pro-2.jpg', 2),
(2, '/images/dell-xps-1.jpg', 1),
(3, '/images/iphone-15-pro-1.jpg', 1),
(4, '/images/galaxy-s24-1.jpg', 1),
(5, '/images/gaming-desktop-1.jpg', 1),
(6, '/images/tshirt-1.jpg', 1),
(7, '/images/blouse-1.jpg', 1),
(8, '/images/shoes-1.jpg', 1),
(9, '/images/clean-code-1.jpg', 1),
(10, '/images/design-patterns-1.jpg', 1)
ON CONFLICT DO NOTHING;
