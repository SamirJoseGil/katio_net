# **Documentación Técnica del Backend**

## **Introducción**

Este documento describe los pasos para ejecutar el backend del proyecto, incluyendo los requisitos previos, configuración del entorno y comandos necesarios para su ejecución.

---

## **Requisitos Previos**

Asegúrate de tener los siguientes elementos instalados y configurados en tu entorno:

1. **.NET SDK**  
    Versión recomendada: `.NET 7.0` o superior.  
    [Descargar aquí](https://dotnet.microsoft.com/download)
    
2. **IDE recomendado**
    
    - Visual Studio 2022 (con la carga de trabajo de desarrollo .NET instalada)
    - Visual Studio Code (con extensión C#)
    - Extension .Net Install Tool
    - C# Dev Kit
3. **Base de datos**  
     - **PostgreSQL**
     La base de datos con la que desplegamos es PostgreSQL usamos su ultima versión al momento 17.2 (https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
    - **Beekeeper**(Opcional)
	  El manejo de la base de datos lo hicimos con Beekeeper Studio (https://www.beekeeperstudio.io/get)
    
5. **Herramientas adicionales**
    
    - PowerShell o Terminal de comandos integrada(cmd).
    - Git para control de versiones y para copiar el repositorio.

---

## **Configuración del Proyecto**

1. **Clonar el repositorio**  
    En GitHub, clonar el repositorio utilizando el siguiente comando:
    
     ```bash
    git clone https://github.com/SamirJoseGil/katio_back.git
	```
	
    Despues dirigirse a la carpeta raiz
    
     ``` bash
    cd katio_net
	```
    
1. **Configurar la Base de Datos**
    
    - Edita el archivo `appsettings.json` para configurar la cadena de conexión a la base de datos:
    
    Copiar código y agregar tu usuario y contraseña de PostgreSQL
	```json
	{   "ConnectionStrings": {     "DefaultConnection": "Server=localhost;Database=DatabaseName;User Id=username;Password=password;"   } }
   
	``` 
	
	
1. **Restaurar paquetes NuGet**  
    Ejecuta el siguiente comando para restaurar las dependencias del proyecto:
    
    ```bash
    dotnet restore
	```
	
1. **Aplicar migraciones de la base de datos**  
    Necesitas añadir las migraciones, para eso necesitas dirigirte a la carpeta .API
    
    ```bash
    cd katio_net.API
	```
    
    Despues ejecutar este codigo
    
    ```bash
    dotnet ef migrations add InitialCreate --project ../Katio_net.Data dotnet ef       database update
	```
	
---

## **Ejecución del Proyecto**

1. **Iniciar el servidor**  
    Usa el siguiente comando para ejecutar el proyecto:
  
    ```bash
    dotnet run
	```
	
    o usar el siguiente codigo que te llevara al Swagger UI
       
    ```bash
    dotnet watch --project katio_net.API
    ```
    
1. **Acceso al backend**  
    El backend estará disponible en la siguiente URL por defecto:
    
    `http://localhost:5125`
    
    
3. **Endpoints disponibles**  
    En el proyecto manejamos los metodos **CRUD**.
    De forma local los endpoints se verian de la siguiente forma: 
	Ruta por defecto
    `Http://localhost:(Puerto)/api/(Controlador)`
    - C (`Crear`) -> este es un metodo **POST**
	    Para acceder al metodo de crear debes usar la ruta por defecto `/api/(Controlador)/Create(Controlador)`
	
	- R(`Leer`) -> este es un metodo **GET**
		Para acceder al metodo de leer debes usar la ruta por defecto
		pero este tiene otros puntos dependiendo de lo que se desee buscar y dependiendo del controlador, siempre se acompaña de **GetBy** junto con lo que deseas.
		- En los libros `Name, Id, ISBN10, ISBN13, Edition, Published, Edition, DeweyIndex `
		 `/api/(Controlador)/GetBookBy(Controlador)`
		
		- En los Audiolibros `Id, Name, ISBN10, ISBN13, Published, Edition, Genre, LenghtInSeconds, Narrator, NarratorName, NarratorLastName, NarratorFullName, NarratorGenre`
		 `/api/(Controlador)/GetAudioBookBy(Controlador)`
		 
		- En los Autores `Id, Name, LastName, Country, BirthDate`
		 `/api/(Controlador)/GetAuthorBy(Controlador)`
		
		- En los Generos `Name, Description`
		  `/api/(Controlador)/GetGenresBy(Controlador)`
		
		- En los Narradores `Name, LastName, Id`
		 `/api/(Controlador)/GetNarratorBy(Controlador)`
	
	- U(`Actualizar`) ->este es un metodo **PUT**  
		Para acceder al metodo Actualizar debes usar la ruta por defecto
		`/api/(Controlador)/Update(Controlador)`
	
	- D(`Eliminar`) -> este es un metodo **DELETE** -> este encuentra el id y elimina
		Para acceder al metodo Eliminar debes usar la ruta por defecto
		`/api/(Controlador)/Delete(Controlador)`
    - `GET /api/books` - Obtiene todos los libros.
    - `POST /api/books` - Crea un nuevo libro.
    - `GET /api/books/{id}` - Obtiene un libro por ID.

---

## **Comandos Útiles**

- **Compilar el proyecto sin ejecutarlo:**
	Esto te ayudara para ver que se esta ejecutando y si hay algun error.
    
    ```bash
    dotnet build
	```
	
- **Para limpiar la construccion del proyecto:**
- 
	```bash
    dotnet clean
	```
	
- **Publicar el proyecto para producción:**
    
    ```
    dotnet publish -c Release -o ./published
	```
	

---

## **Pruebas Unitarias**

Todas las pruebas unitarias se encuentran en una carpeta **katio_net.Test** las cuales pueden ser ejecutadas despues de tener las extenciones de **C# dev kit** en donde enontraras pruebas para cada servicio, solo debes ir a tu visual studio code y correrlas, recuerda que estas si fallan debes revisar cambios hechos.

---

## **Solución de Problemas**

- **Error de conexión a la base de datos:**  
    Verifica que la cadena de conexión en `appsettings.json` sea correcta y que el servidor de la base de datos esté en ejecución.
    
- **Paquetes faltantes:**  
    Asegúrate de haber ejecutado `dotnet restore`.
    
- **Problemas con migraciones:**  
    Revisa las migraciones pendientes y verifica que Entity Framework esté configurado correctamente.


### **Technical Documentation of the Backend**

## **Introduction**

This document outlines the steps required to run the backend of the project, including prerequisites, environment setup, and the necessary commands for execution.

---

## **Prerequisites**

Ensure you have the following tools installed and configured in your environment:

1. **.NET SDK**  
    Recommended version: `.NET 7.0` or higher.  
    [Download here](https://dotnet.microsoft.com/download)
    
2. **Recommended IDEs**
    
    - Visual Studio 2022 (with the .NET development workload installed)
    - Visual Studio Code (with C# extension)
    - .Net Install Tool extension
    - C# Dev Kit
3. **Database**
    
    - **PostgreSQL**  
        The project uses PostgreSQL, specifically version 17.2.  
        [Download here](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
        
    - **Beekeeper** (Optional)  
        For database management, we used Beekeeper Studio.  
        [Download here](https://www.beekeeperstudio.io/get)
        
4. **Additional Tools**
    
    - PowerShell or integrated command terminal (cmd).
    - Git for version control and repository cloning.

---

## **Project Setup**

1. **Clone the Repository**  
    Clone the repository from GitHub using the following command:
    
    ```bash
    git clone https://github.com/SamirJoseGil/katio_back.git
	```
	
    
    Then, navigate to the root folder:
    
    ``` bash
    cd katio_net
	```
    
2. **Configure the Database**  
    Edit the `appsettings.json` file to set up the database connection string:
    
    ```json
{   "ConnectionStrings": {     "DefaultConnection": "Server=localhost;Database=DatabaseName;User Id=username;Password=password;"   } }
   
	``` 
	
1. **Restore NuGet Packages**  
    Run the following command to restore project dependencies:
    
    ```bash
    dotnet restore
	```
	
    
4. **Apply Database Migrations**  
    Navigate to the `.API` folder:
    
    ```bash
    cd katio_net.API
	```
    
    
    Run these commands to add and apply migrations:
    
    ```bash
    dotnet ef migrations add InitialCreate --project ../Katio_net.Data dotnet ef       database update
    
	```
	

---

## **Running the Project**

1. **Start the Server**  
    Use this command to run the project:
    
    ```bash
    dotnet run
	```
    
    Alternatively, to access the Swagger UI:
    
    ```bash
    dotnet watch --project katio_net.API
    ```
       
2. **Backend Access**  
    The backend will be available at the default URL:
       
    `http://localhost:5125`
    
3. **Available Endpoints**  
    The project includes **CRUD** methods. Locally, the endpoints follow this pattern:
    `http://localhost:(Port)/api/(Controller)`
    
    - **C (Create)** -> **POST**  
        Example: `/api/(Controller)/Create`
        
    - **R (Read)** -> **GET**  
        Example: `/api/(Controller)/GetByProperty`
        
        Supported queries include:
        
        - **Books**: `Name, Id, ISBN10, ISBN13, Edition, Published, DeweyIndex`
        - **Audiobooks**: `Id, Name, ISBN10, Genre, NarratorName`
        - **Authors**: `Id, Name, LastName, Country`
        - **Genres**: `Name, Description`
        - **Narrators**: `Name, LastName, Id`
    - **U (Update)** -> **PUT**  
        Example: `/api/(Controller)/Update`
        
    - **D (Delete)** -> **DELETE**  
        Example: `/api/(Controller)/Delete`
        
    
    Examples for the **Books** controller:
    
    - `GET /api/books` - Retrieve all books.
    - `POST /api/books` - Create a new book.
    - `GET /api/books/{id}` - Retrieve a book by ID.

---

## **Useful Commands**

- **Build the project without running it:**  
    This checks for errors in the build process:
    
    ```bash
    dotnet build
	```
    
- **Clean the project build:**
    
    ```bash
    dotnet clean
	```
	
- **Publish the project for production:**
    
    ```
    dotnet publish -c Release -o ./published
	```
	

---

## **Unit Tests**

All unit tests are located in the **katio_net.Test** folder and can be executed after installing the **C# Dev Kit** extensions. You will find tests for each service. Simply open Visual Studio Code and run them. If any tests fail, make sure to review any changes made.

---

## **Troubleshooting**

- **Database connection error:**  
    Ensure the connection string in `appsettings.json` is correct and the database server is running.
    
- **Missing packages:**  
    Make sure to run `dotnet restore`.
    
- **Migration issues:**  
    Check for pending migrations and verify that Entity Framework is properly configured.
