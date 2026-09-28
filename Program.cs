using System;
using Books;

var books = DbContext.GetAllBooks();

if (books is null)
{
    Console.WriteLine("Нет книг");
    return;
}

foreach (var book in books)
{
    var authors = string.Join(", ", book.Authors);
    Console.WriteLine($"{book.Title}: {authors}");
}