## Despliegue de MyBookyApp

Este proyecto ha sido desarrollado utilizando **C#**, **MongoDB** y **.Net Core SDK**. A continuación, se detallan los pasos necesarios para probarlo:

1. Ejecutar MongoDB y MongoDB Compass.
2. Instalar .NET Core desde [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download).
3. Verificar que la variable de entorno de .NET Core está configurada. Para ello, ejecutar `dotnet --version` desde la consola del sistema.
4. El proyecto está disponible en GitHub como **HB02-MongoDB-CSharp**. Para clonarlo, selecciona un directorio y ejecuta el siguiente comando en la terminal:

    ```bash
    git clone https://github.com/Francisco-David/HB02-MongoDB-CSharp.git
    ```

    Si ya tienes el proyecto descargado, continúa con el siguiente paso.
5. Una vez clonado el repositorio, navega hasta la carpeta **MyBookyApp**:

    ```bash
    cd HB02-MongoDB-CSharp/MyBookyApp
    ```

6. Verifica en el archivo **Booky.cs** que la dirección y el puerto de MongoClient coinciden con los de tu instancia de MongoDB. Por defecto, se utiliza `mongodb://localhost:27017`.
7. Compila y ejecuta el proyecto con los siguientes comandos:

    ```bash
    dotnet build ./MyBookyApp.csproj
    dotnet run ./MyBookyApp.csproj
    ```

¡Listo! Ahora MyBookyApp debería estar en funcionamiento.
