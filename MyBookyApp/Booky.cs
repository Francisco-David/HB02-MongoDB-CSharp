using System;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Usuario
{
     [BsonId]
    public ObjectId IdUser { get; set; }
    public string Nombre { get; set; }
    public string CorreoElectronico { get; set; }
    public string Contrasena { get; set; }
    public List<ObjectId> LibrosFavoritos { get; set; }
    public List<ObjectId> autoresSeguidos { get; set; }

    public Usuario()
    {
        Nombre = "";
        CorreoElectronico = "";
        Contrasena = "";
        LibrosFavoritos = new List<ObjectId>();
        autoresSeguidos = new List<ObjectId>();
    }
}

public class Autor
{
     [BsonId]
    public ObjectId IdAutor { get; set; }
    public string Nombre { get; set; }
    public string Nacionalidad { get; set; }
    public List<string> Libros { get; set; }
    public Autor()
    {
        Nombre = "";
        Nacionalidad = "";
        Libros = new List<string>(); 
       
    }
}

public class Libro
{
     [BsonId]
    public ObjectId IdLibro { get; set; }
    public string Titulo { get; set; }

     public Autor Autor { get; set; }

    public string Genero { get; set; }
    public int Likes { get; set; }
    public List<Comentario> Comentarios { get; set; }

    public Libro()
    {
        Titulo = "";
         Autor = new Autor();
        Genero = "";
        Likes = 0;
        Comentarios = new List<Comentario>();
    }
}

public class Comentario
{
     [BsonId]
    public ObjectId IdComentario { get; set; }
    public string Texto { get; set; }
    public ObjectId UsuarioId { get; set; }
    public DateTime FechaPublicacion { get; set; }

    public Comentario()
    {
        Texto = "";
        UsuarioId = ObjectId.Empty;
        FechaPublicacion = DateTime.UtcNow;
    }
}


public class Booky
{
    private IMongoCollection<Autor> autores;
    private IMongoCollection<Libro> libros;
    private IMongoCollection<Usuario> usuarios;
    private IMongoCollection<Comentario> comentarios;

    public Booky()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("BookyDB");

        autores = database.GetCollection<Autor>("autores");
        libros = database.GetCollection<Libro>("libros");
        usuarios = database.GetCollection<Usuario>("usuarios");
        comentarios = database.GetCollection<Comentario>("comentarios");
    }

    public void AddAutor(Autor autor)
    {
        autores.InsertOne(autor);
    }

    public void AddLibro(Libro libro)
    {
        var filter = Builders<Autor>.Filter.Eq(a => a.IdAutor, libro.Autor.IdAutor);
        Autor autor = autores.Find(filter).FirstOrDefault();
        autor.Libros.Add(libro.Titulo);
        var update = Builders<Autor>.Update.Set(a => a.Libros, autor.Libros);
        autores.UpdateOne(filter, update);

        libros.InsertOne(libro);
    }

    public void AddUsuario(Usuario usuario)
    {
        usuarios.InsertOne(usuario);
    }

    public void AddComentario(Comentario comentario)
    {
        comentarios.InsertOne(comentario);
    }
    
    public void AddFavoritos( )
    {
    //añadir el libro a la lista de libros favoritos del usuario
        
    }

    public void SeguirAutor( )
    {
    //añadir el autor a la lista de autores seguidos del usuario
    }

    public void PublicarComentario( )
    {
    //añadir el comentario a la lista de comentarios del libro
    }
  

}


public class Programa
{
    public static void Main(string[] args)
    {
        Booky platform = new Booky();

        Autor autor1 = new Autor
        {
            IdAutor = ObjectId.GenerateNewId(),
            Nombre = "Gabriel García Márquez",
            Nacionalidad = "Colombia",
            Libros = new List<string>()
            
        };
        platform.AddAutor(autor1);

        Usuario usuario1 = new Usuario
        {
            IdUser = ObjectId.GenerateNewId(),
            Nombre = "Usuario1",
            CorreoElectronico = "usuario1@example.com",
            Contrasena = "contrasena1",
            LibrosFavoritos = new List<ObjectId>(),
            autoresSeguidos = new List<ObjectId> { autor1.IdAutor } // Usar el ObjectId del autor creado previamente
        };
        platform.AddUsuario(usuario1);

         Comentario comentario1 = new Comentario
        {
            IdComentario = ObjectId.GenerateNewId(),
            Texto = "¡Excelente libro!",
            UsuarioId = usuario1.IdUser, // Usar el ObjectId del usuario creado previamente
            FechaPublicacion = DateTime.UtcNow
        };
        platform.AddComentario(comentario1);

        Libro libro1 = new Libro
        {
            IdLibro = ObjectId.GenerateNewId(),
            Titulo = "Cien años de soledad",
            Autor =  autor1,// Usar el ObjectId del autor creado previamente
            Genero = "Realismo mágico",
            Likes = 100,
            Comentarios = new List<Comentario> { comentario1 } // Usar el ObjectId del comentario creado previamente
        };
        platform.AddLibro(libro1);
       
       
    }
}

