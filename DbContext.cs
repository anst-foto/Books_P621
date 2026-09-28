using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace Books;

public class DbContext
{
    private const string ConnectionString = "Data Source=books.db;";

    public static IEnumerable<Book>? GetAllBooks()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        const string sql = """
                           SELECT table_books.title AS title,
                                  GROUP_CONCAT(table_authors.full_name, ', ') AS authors
                           FROM table_authors_books 
                                    JOIN table_books
                                        ON table_authors_books.book_id = table_books.id
                                    JOIN table_authors 
                                        ON table_authors_books.author_id = table_authors.id 
                           GROUP BY table_books.id, table_books.title;
                           """;
        var command = new SqliteCommand(sql, connection);
        var reader = command.ExecuteReader();
        if (!reader.HasRows) return null;

        var books = new List<Book>();
        while (reader.Read())
        {
            var title = reader.GetString("title");
            var authors = reader.GetString("authors")
                .Split(", ")
                .ToList();

            var book = new Book()
            {
                Title = title,
                Authors = authors
            };

            books.Add(book);
        }
        
        connection.Close();
        
        return books;
    }
}