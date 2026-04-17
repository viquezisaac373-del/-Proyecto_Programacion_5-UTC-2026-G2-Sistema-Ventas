Sistema Completo de Ventas

Requisitos:
.NET 6+
MySQL Server

Instalación:
Ejecutar el archivo facturadb.sql
en MySQL para crear la base de datos
automáticamente. El mismo está ubicado en:
Sistema_Completo_De_Ventas/database/facturadb.sql
Abrir la solución en Visual Studio
Ejecutar el proyecto
Usuario por defecto:

-Usuario: admin
-Contraseña: 1234

Funcionalidades:
CRUD de clientes
CRUD de productos
Registro de ventas
Reportes
Sistema de usuarios

Arquitectura:
UI (WinForms)
BLL (Lógica de negocio)
DAL (Acceso a datos)
DTO (Transferencia de datos)

Estructura del proyecto:
Sistema_Completo_De_Ventas (UI y punto de entrada)
SistemaVentas.BLL (Lógica de negocio)
SistemaVentas.DAL (Acceso a datos)
SistemaVentas.DTO (Objetos de transferencia)