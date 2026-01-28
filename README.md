# Tienda de Flores - Proyecto .NET + Angular

Pequeño proyecto de aprendizaje full-stack que simula una tienda online de flores.  
Incluye un **backend en ASP.NET Core** (API REST) y un **[frontend en Angular](https://github.com/flaviovich/tienda-flores-angular)** para mostrar productos, gestionar un carrito de compra básico (almacenado en sesión) y realizar operaciones CRUD simples sobre ítems del carrito.

## Tecnologías utilizadas

### Backend
- **.NET 8**
- **ASP.NET Core Web API**
- **C#**
- Almacenamiento del carrito: **Session** de ASP.NET Core (sin base de datos persistente por ahora)
- Inyección de dependencias nativa
- DTOs para transferencia de datos
- Logging básico con ILogger

## Cómo ejecutar el proyecto

1. Clonar el repositorio
   ```bash
   git clone https://github.com/flaviovich/tienda-flores-csharp.git
   cd tienda-flores-csharp
   ```

2. Backend (API .NET)

   ```bash
   cd backend/ServidorTiendaDotNet     # o la carpeta donde esté el .sln / .csproj principal
   dotnet restore
   dotnet run
   ```

   La API normalmente queda escuchando en:  
   `https://localhost:7123` o `http://localhost:5123`  
   (revisa el puerto en la consola o en `launchSettings.json`)

## Notas importantes

- El carrito se guarda en **sesión HTTP** → se pierde al cerrar el navegador o expirar la sesión.
- Proyecto pensado para practicar: DTOs, controladores, servicios, modelos, comunicación http, manejo de estado simple en Angular.

¡Disfruta aprendiendo!  
Cualquier mejora o sugerencia es bienvenida → pull requests abiertos.
