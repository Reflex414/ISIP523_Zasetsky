using System;
using System.Collections.Generic;
using System.Linq;

public enum Genre
{
    Fantasy,
    ScienceFiction,
    Mystery,
    Romance,
    Thriller,
    Biography,
    History
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Genre Genre { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Название: {Title}, Автор: {Author}, Жанр: {Genre}, Год: {Year}, Цена: {Price:C}";
    }
}

