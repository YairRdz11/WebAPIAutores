﻿using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebAPIAutores.Entities;

namespace WebAPIAutores
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Autor> Autors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AutorBook> AutorsBooks { get; set; }
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<AutorBook>()
                .HasKey(x => new { x.AutorId, x.BookId });
        }
    }
}
