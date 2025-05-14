using HandyControl.Controls;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using WpfSample.WpfSqlite.Models;

namespace WpfSample.WpfSqlite.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<SubTaskEntity> SubTasks { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<TaskTagEntity> TaskTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 配置SQLite数据库连接字符串
            optionsBuilder.UseSqlite("Data Source=todo.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 可以在这里配置模型关系等
            //modelBuilder.Entity<TodoItem>()
            //    .Property(t => t.Title)
            //    .IsRequired()
            //    .HasMaxLength(100);

            modelBuilder.Entity<TaskEntity>()
                .HasKey(t => t.TaskId);

            // 配置全局查询过滤器（排除已删除的任务）
            modelBuilder.Entity<TaskEntity>()
                .HasQueryFilter(t => !t.IsDeleted);

            // 配置索引
            modelBuilder.Entity<TaskEntity>()
                .HasIndex(t => new { t.DueDate, t.Status });

            modelBuilder.Entity<TaskEntity>()
                .HasIndex(t => t.Priority);

            modelBuilder.Entity<TaskEntity>()
                .HasIndex(t => t.IsDeleted);

            // 配置级联删除
            modelBuilder.Entity<TaskEntity>()
                .HasMany(t => t.SubTaskEntitys)
                .WithOne(st => st.TaskEntity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskEntity>()
                .HasMany(t => t.TaskTagEntitys)
                .WithOne(tt => tt.TaskEntity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TagEntity>()
                .HasMany(t => t.TaskTagEntitys)
                .WithOne(tt => tt.TagEntity)
                .OnDelete(DeleteBehavior.Cascade);


            // 配置复合主键
            modelBuilder.Entity<TaskTagEntity>()
                .HasKey(tt => new { tt.TaskId, tt.TagId });
        }
    }
}
