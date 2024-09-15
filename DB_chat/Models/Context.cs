using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DB_chat.Models
{
    // Строка подкоючения с сайта connectionstring
    // "Host=localHost;Username=postgres;Password=example;Database=chatv1"
    public class Context : DbContext
    {
        // своеобразные коллекции-таблицы
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public Context() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine).UseLazyLoadingProxies().UseNpgsql("Host=localHost;Username=postgres;Password=example;Database=ChatDB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                // установили первичный ключ
                entity.HasKey(e => e.Id).HasName("message_pkey");
                // название таблицы
                entity.ToTable("Message");
                // Процесс нименования полей
                entity.Property(x => x.Id).HasColumnName("Id");
                entity.Property(x => x.Text).HasColumnName("Text");
                entity.Property(x => x.FromUserId).HasColumnName("MessageFromUser");
                entity.Property(x => x.ToUserId).HasColumnName("MessageToUser");

                // создание свзяи многие ко многим 
                entity.HasOne(d => d.FromUser).
                WithMany(p => p.MessagesFromUser).
                HasForeignKey(e => e.FromUserId).
                HasConstraintName("message_from_user_id_fkey");

                entity.HasOne(d => d.ToUser).
                WithMany(p => p.MessagesToUser).
                HasForeignKey(e => e.ToUserId).
                HasConstraintName("message_to_user_id_fkey");

            });
            modelBuilder.Entity<User>(entity =>
            {
                // установили первичный ключ
                entity.HasKey(e => e.Id).HasName("users_pkey");
                // название таблицы
                entity.ToTable("users");
                // Процесс нименования полей
                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Name).HasMaxLength(255).
                HasColumnName("name");
            });

            base.OnModelCreating(modelBuilder);
            // dotnet tool install --global dotnet-ef :: что и зачем
            //Создает код для DbContext типов сущностей для базы данных.
        }
    }
}
