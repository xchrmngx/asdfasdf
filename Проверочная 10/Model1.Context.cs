﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Проверочная_10
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class КнижкиEntities3 : DbContext
    {
        public КнижкиEntities3()
            : base("name=КнижкиEntities3")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Автор_> Автор_ { get; set; }
        public virtual DbSet<Вид_> Вид_ { get; set; }
        public virtual DbSet<Заказы> Заказы { get; set; }
        public virtual DbSet<Книги_> Книги_ { get; set; }
    }
}
