# Biblioteca Digital Secretos Para Contar

## Katio: Proyecto .NET con C#

Este es un proyecto sencillo desarrollado en .NET con C#, diseñado para ser una biblioteca o repositorio de libros digital para la Fundación Secretos Para contar.

## integrantes

- Sara Castrillon
- Felipe Ochoa
- Samir Gil
- Maria Estefania

## Requisitos previos:
Asegúrate de tener lo siguiente instalado en tu sistema:

- .NET SDK (Versión 6.0 o superior)
- Un IDE o editor de texto compatible con C#, como Visual Studio o Visual Studio Code con la extensión de C# instalada.
- Git para clonar el repositorio.

## Clonar el repositorio:
Abre una terminal y ejecuta el siguiente comando para clonar el repositorio en tu máquina local:

´´´
git clone https://github.com/SamirJoseGil/katio_back.git
´´´

Luego, navega al directorio del proyecto:

´´´
cd katio_back
´´´

## Compilar el proyecto:
Para compilar el proyecto, ejecuta el siguiente comando en la terminal:
´´´
dotnet build
´´´

## Ejecutar el proyecto:
Una vez compilado, puedes ejecutar el proyecto con el siguiente comando:
´´´
dotnet watch --project Katio_net.API
´´´

Esto ejecutará la aplicación en la terminal. Se abrirá en el navegador en la URL especificada (por defecto es http://localhost:5000).

## Estructura del proyecto:

- /src: Contiene el código fuente del proyecto.
- /tests: Contiene las pruebas unitarias del proyecto (si aplicable).
- /README.md: Este archivo, que explica cómo configurar y ejecutar el proyecto.



## Historia de Usuario / Requisitos:

### Usuarios

- [ ] Crear un usuario, utilizar el registro.
- [ ] Login del usuario: debe regresar un token bearer. Al hacer login, debo poder usar el token para cuaquier otra acción.
- [ ] Todas mis acciones deben quedar bajo llave, con la sola excepción de: Login, Signup.
- [ ] Editar un usuario.
- [ ] Debo poder reiniciar mi clave, solo mi clave. Debo colocar la clave anterior, y dos veces la clave nueva.
- [ ] Listar todos mis usuarios.
- [ ] Listar todos mis usuarios por email, o identificación.
- [ ] Agregar un username. No todos los usuarios tienen un correo hábil. Ambos campos son distintos, pero puede repetir la información.
- [ ] Manejar los perfiles (Roles) del usuario.

### Libros

- [x] Crear un libro.
- [x] Editar un libro.
- [x] Buscar libro por nombre, de forma relativa.
- [x] Buscar libro por autor, de forma relativa, por nombre y apellido del autor.
- [x] Buscar libro por editorial.
- [x] Buscar libro por genero.
- [x] Buscar libro por fecha de publicación.
- [ ] Subir un libro en PDF a la biblioteca.
- [ ] Servir un libro en PDF al cliente.
- [ ] Agregar varios géneros a un libro.
- [ ] Los libros pueden tener varios autores.
- [ ] Agregar temas al libro.
- [ ] un libro puede tener varios temas.
- [x] No pueden haber dos versiones del mismo libro.
- [ ] Agregar libros relacionados a un libro principal

### Audiolibros

- [x] Crear un audiolibro.
- [x] Editar un audiolibro.
- [x] Buscar Audiolibro por nombre, de forma relativa.
- [x] Buscar Audiolibro por autor, de forma relativa, por nombre y apellido del autor.
- [x] Buscar Audiolibro por editorial.
- [x] Buscar Audiolibro por genero.
- [x] Buscar Audiolibro por fecha de publicación.
- [ ] Subir un audiolibro en MP3/OGG a la biblioteca.
- [ ] Servir un audiolibro en MP3/OGG al cliente.
- [ ] Buscar un audiolibro por narrador.
- [x] Buscar un audiolibro por longitud.
- [ ] Agregar varios géneros a un libro.
- [ ] Los libros pueden tener varios autores.
- [ ] Agregar temas al libro.
- [ ] un libro puede tener varios temas.
- [ ] No pueden haber dos versiones del mismo libro.

### Autores

- [x] Crear un Autor
- [x] Editar un Autor
- [x] Buscar un autor por nombre y apellido de forma relativa.
- [x] Buscar un autor por fecha de nacimiento
- [x] Buscar un autor por país de procedencia

### Narradores

- [x] Crear un narrador
- [x] Editar un narrador
- [x] Buscar narrador por nombre.
- [ ] Buscar narrador por perfil de voz.
- [ ] Buscar todos los audiolibros de un narradores por relación.

### Utils

- [ ] manejo correcto de errores.
- [ ] utilizar try catch donde sea necesario
- [ ] Hacer rollback donde sea necesario.
- [x] Usar el tipo correcto para mejorar la memoria.
- [x] no hacer llamados innecesarios.
- [x] Extraer funcionalidad repetida en su propia clase, o método.

### Admin / Estadísticas

- [ ] Ver mis usuarios, editarlos y desactivarlos.
- [ ] Asignar una clave de forma directa a un usuario a través de la edición
- [ ] El username y el email no son mutables.
- [ ] Agregar estadísticas al sitio.
- [ ] Cada vez que se descargue un libro, tener un contador que se encargue de llevar la cuenta.
- [ ] Cada vez que  se reproduzca un audilibro, tener un contador que se encargue de llevar la cuenta.
- [ ] Cada vez que se mire el perfil de un autor con sus libros, tener un contador que se encargue de llevar la cuenta.
- [ ] Cada vez que se descargue un libro, o se escuche un audiolibro, marcar el género en una tabla de contadores que lleve la cuenta.
- [ ] Última conexión al usuario.
- [ ] Llevar la cuenta de cuantos dias distintos se conecta un usuario. Diferente a la última conexión.
- [ ] Agregar logs al sistema.
- [ ] Basada en las conexiones y la cuenta de las mismas, emitir una estadística que diga cuales son los días más activos para el sistema.
- [ ] llevar la cuenta de intentos fallidos al hacer login. Al llegar a 10, bloquear el usuario. una vez se ingrese la clave correcta, reiniciar el contador a 0;
- [ ] Cambiar la longitud de duración del token a 24H.'
- [ ] Actualizar su sistema a la última versión de Java y Spring.

## BONUS TRACKS (Actividades Extra Curriculares)

- Crear un front end en react. Que sea capaz de implementar todos los dominios.

    - Crear pagina de signup
    - crear página de login

    - crear Modulo de libros
        - Buscar        
        - Descargar

    - Crear módulo de Autores
        - Buscar
        - listar

    - Crear módulo de Audiolibros
        - Buscar
        - Listar
