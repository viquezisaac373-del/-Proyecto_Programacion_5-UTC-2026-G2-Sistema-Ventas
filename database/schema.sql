CREATE DATABASE IF NOT EXISTS facturadb;
USE facturadb;

CREATE TABLE clientes (
    id INT PRIMARY KEY,
    nombre VARCHAR(100),
    correo VARCHAR(100),
    telefono VARCHAR(20)
);

CREATE TABLE productos (
    codigo VARCHAR(10) PRIMARY KEY,
    nombre VARCHAR(100),
    precio DECIMAL(10,2),
    stock INT,
    descuento DECIMAL(5,2)
);

CREATE TABLE ventas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATETIME,
    cliente_id INT,
    FOREIGN KEY (cliente_id) REFERENCES clientes(id)
);

CREATE TABLE venta_detalle (
    id INT AUTO_INCREMENT PRIMARY KEY,
    venta_id INT,
    producto_codigo VARCHAR(10),
    cantidad INT,
    precio DECIMAL(10,2),
    FOREIGN KEY (venta_id) REFERENCES ventas(id),
    FOREIGN KEY (producto_codigo) REFERENCES productos(codigo)
);