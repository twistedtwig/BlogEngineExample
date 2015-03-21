using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BlogEngine.Repository.Models;

namespace BlogEngine.Repository.Interfaces
{

    public interface IBlogRepository
    {
        TEntity Single<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class, new();
        IEnumerable<TEntity> All<TEntity>() where TEntity : class, new();
        IEnumerable<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class, new();
        bool Exists<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class, new();
        void Add<TEntity>(TEntity item) where TEntity : class, new();
        void Update<TEntity>(TEntity item) where TEntity : class, new();

        void Save<TEntity>(TEntity item, Expression<Func<TEntity, bool>> exp) where TEntity : class, new();
        void Delete<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class, new();

        void UpdateTagCount();
        void RemoveUnusedTags();
        IEnumerable<BlogEntryEntity> ListByTags(params string[] tags);
    }
}
