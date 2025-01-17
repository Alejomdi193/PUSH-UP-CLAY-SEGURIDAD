# Proyecto API con .NET y MySQL - BackUpSeguridad

## Descripción

**PushUpClaySeguridad** es un proyecto backend desarrollado en .NET diseñado para gestionar y controlar la seguridad a través de APIs. Este proyecto organiza entidades clave como usuarios, roles, permisos, y otros elementos necesarios para manejar la autenticación, la autorización y las operaciones relacionadas con la seguridad de aplicaciones o servicios.

## Características

- **Autenticación**: Implementación de mecanismos de autenticación mediante tokens.
- **Autorización**: Gestión de roles y permisos para controlar el acceso a diferentes recursos.
- **API REST**: Desarrollo de endpoints para interactuar con las entidades y manejar operaciones CRUD.
- **Estructura Modular**: División clara entre capas de dominio, interfaces y persistencia para un mantenimiento eficiente.


- **Interfaces**: Define las abstracciones y contratos para las operaciones del sistema.

- **Persistencia**: Implementa la lógica de acceso a datos para interactuar con la base de datos.

- **APIs**: Expone endpoints REST para la interacción con las entidades.

## Instalación y Uso

### Prerrequisitos
- **.NET SDK**: Asegúrate de tener instalado el SDK de .NET adecuado.
- **Base de datos**: Configura la conexión a tu base de datos en el archivo de configuración del proyecto.
- **Visual Studio o VS Code**: IDE recomendado para trabajar con el proyecto.

## Configuración Inicial

### Requisitos Previos

1. .NET SDK instalado en el sistema.
2. Base de datos configurada (compatible con EF Core).

### Pasos para Ejecutar el Proyecto

1. Clona el repositorio del proyecto.
   ```bash
   git clone <https://github.com/Alejomdi193/BackendSeguridad.git>
   cd <./Persistencia>
   ```

2. Aplica las migraciones para configurar la base de datos:
   ```bash
   dotnet ef database update --project ./Persistencia --startup-project ./Api
   ```

3. Inicia el servidor:
   ```bash
   dotnet run --project ./Api
   ```

4. Accede a los endpoints de la API a través de tu navegador o herramienta de prueba como Postman.

