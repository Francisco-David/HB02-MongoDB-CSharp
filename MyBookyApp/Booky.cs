using System;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Bson;

public class Usuario
{
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
    public ObjectId IdAutor { get; set; }
    public string Nombre { get; set; }
    public string Nacionalidad { get; set; }
    public List<ObjectId> LibrosPublicados { get; set; }
    public Autor()
    {
        Nombre = "";
        Nacionalidad = "";
        LibrosPublicados = new List<ObjectId>();
    }
}

public class Libro
{
    public ObjectId IdLibro { get; set; }
    public string Titulo { get; set; }
    public List<ObjectId> Autores { get; set; }
    public string Genero { get; set; }
    public int Likes { get; set; }
    public List<ObjectId> Comentarios { get; set; }
    public Libro()
    {
        Titulo = "";
        Autores = new List<ObjectId>();
        Genero = "";
        Likes = 0;
        Comentarios = new List<ObjectId>();
    }
}

public class Comentario
{
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
            LibrosPublicados = new List<ObjectId>()
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

        Libro libro1 = new Libro
        {
            IdLibro = ObjectId.GenerateNewId(),
            Titulo = "Cien años de soledad",
            Autores = new List<ObjectId> { autor1.IdAutor }, // Usar el ObjectId del autor creado previamente
            Genero = "Realismo mágico",
            Likes = 100,
            Comentarios = new List<ObjectId>()
        };
        platform.AddLibro(libro1);

        Comentario comentario1 = new Comentario
        {
            IdComentario = ObjectId.GenerateNewId(),
            Texto = "¡Excelente libro!",
            UsuarioId = usuario1.IdUser, // Usar el ObjectId del usuario creado previamente
            FechaPublicacion = DateTime.UtcNow
        };
        platform.AddComentario(comentario1);
    }
}

