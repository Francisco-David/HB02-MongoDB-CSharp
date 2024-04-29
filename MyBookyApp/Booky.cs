using System;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Usuario
{
     [BsonId]
    public ObjectId IdUsuario { get; set; }
    public string Nombre { get; set; }
    public string CorreoElectronico { get; set; }
    public string Contrasena { get; set; }
    public List<Libro> LibrosFavoritos { get; set; }
    public List<Autor> AutoresSeguidos { get; set; }

    public Usuario()
    {
        Nombre = "";
        CorreoElectronico = "";
        Contrasena = "";
        LibrosFavoritos = new List<Libro>();
        AutoresSeguidos = new List<Autor>();
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

    public List<Autor> Autor { get; set; }

    public string Genero { get; set; }
    public int Likes { get; set; }
    public List<Comentario> Comentarios { get; set; }

    public Libro()
    {
        Titulo = "";
        Autor = new List<Autor>();
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

    public void AddLibro(Libro libro, List<ObjectId> autorIDs)
    {
        List<Autor> autoresDelLibro = new List<Autor>();
        foreach (ObjectId autorID in autorIDs)
        {
            var filter = Builders<Autor>.Filter.Eq(a => a.IdAutor, autorID);
            Autor autor = this.autores.Find(filter).FirstOrDefault();
            if (autor != null)
            {
                autoresDelLibro.Add(autor);
                autor.Libros.Add(libro.Titulo);
                var update = Builders<Autor>.Update.Set(a => a.Libros, autor.Libros);
                autores.UpdateOne(filter, update);
            }
        }
        libro.Autor = autoresDelLibro;
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
    
    public void AddFavoritos( ObjectId usuarioId, ObjectId libroId)
    {
        var filterUsuario = Builders<Usuario>.Filter.Eq(u => u.IdUsuario, usuarioId);
        var filterLibro = Builders<Libro>.Filter.Eq(x => x.IdLibro, libroId);

        Usuario user = usuarios.Find(filterUsuario).FirstOrDefault();
        Libro libro = libros.Find(filterLibro).FirstOrDefault();

        if (libro != null)
        {
            user.LibrosFavoritos.Add(libro);
            var update = Builders<Usuario>.Update.Set(u => u.LibrosFavoritos, user.LibrosFavoritos);
            usuarios.UpdateOne(filterUsuario, update);
        }
    }


    public void SeguirAutor(ObjectId usuarioID, ObjectId autorID)
    {
        var filterUpdate = Builders<Usuario>.Filter.Eq(u => u.IdUsuario, usuarioID);
        Usuario user = usuarios.Find(filterUpdate).FirstOrDefault();

        var filterFind = Builders<Autor>.Filter.Eq(a => a.IdAutor, autorID);
        Autor aut  = autores.Find(filterFind).FirstOrDefault();
        
        user.AutoresSeguidos.Add(aut);
        
        var update = Builders<Usuario>.Update.Set(u => u.AutoresSeguidos, user.AutoresSeguidos);
        usuarios.UpdateOne(filterUpdate, update);
    }

    public void PublicarComentario(ObjectId libroID, ObjectId comentarioID)
    {
        var filterUpdate = Builders<Libro>.Filter.Eq(l => l.IdLibro, libroID);
        Libro book = libros.Find(filterUpdate).FirstOrDefault();

        var filterFind = Builders<Comentario>.Filter.Eq(c => c.IdComentario, comentarioID);
        Comentario com  = comentarios.Find(filterFind).FirstOrDefault();
        
        book.Comentarios.Add(com);
        
        var update = Builders<Libro>.Update.Set(l => l.Comentarios, book.Comentarios);
        libros.UpdateOne(filterUpdate, update);
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
            IdUsuario = ObjectId.GenerateNewId(),
            Nombre = "Usuario1",
            CorreoElectronico = "usuario1@example.com",
            Contrasena = "contrasena1",
            LibrosFavoritos = new List<Libro>(),
            AutoresSeguidos = new List<Autor>() 
        };
        platform.AddUsuario(usuario1);
        platform.SeguirAutor(usuario1.IdUsuario, autor1.IdAutor);

         Comentario comentario1 = new Comentario
        {
            IdComentario = ObjectId.GenerateNewId(),
            Texto = "¡Excelente libro!",
            UsuarioId = usuario1.IdUsuario, 
            FechaPublicacion = DateTime.UtcNow
        };
        platform.AddComentario(comentario1);

        Libro libro1 = new Libro
        {
            IdLibro = ObjectId.GenerateNewId(),
            Titulo = "Cien años de soledad",
            Genero = "Realismo mágico",
            Likes = 100,
            Comentarios = new List<Comentario> { comentario1 } 
        };
        List<ObjectId> autores = new List<ObjectId>();
        autores.Add(autor1.IdAutor);
        platform.AddLibro(libro1, autores);

        platform.AddFavoritos(usuario1.IdUsuario, libro1.IdLibro);
        Comentario comentario2 = new Comentario
        {
            IdComentario = ObjectId.GenerateNewId(),
            Texto = "waos",
            UsuarioId = usuario1.IdUsuario, 
            FechaPublicacion = DateTime.UtcNow
        };
        platform.AddComentario(comentario2);
        platform.PublicarComentario(libro1.IdLibro, comentario2.IdComentario);
       
    }
}

