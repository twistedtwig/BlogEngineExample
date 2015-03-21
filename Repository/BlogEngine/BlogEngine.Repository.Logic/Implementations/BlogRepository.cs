﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlogEngine.Repository.Interfaces;
using BlogEngine.Repository.Models;
using RefactorThis.GraphDiff;

namespace BlogEngine.Repository.Implementations
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogEngineContext _context = new BlogEngineContext();

        /// <summary>
        /// This is a bit messy and not ideal.  Will be cleaned up by implementing auto mapper and project To.
        /// It currently ensures all entities are returned with all their child properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IQueryable<T> Setup<T>() where T : class, new()
        {
            string type = typeof(T).Name;
            switch (type)
            {
                case "BlogEntryEntity":
                    return _context.Set<T>().Include("Tags").AsQueryable();

                case "TagCountEntity":
                    return _context.Set<T>().Include("Tag").AsQueryable();

                case "TagEntity":
                default:
                    return _context.Set<T>().AsQueryable();
            }
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class, new()
        {
            return Setup<TEntity>().Single(exp);
        }

        public IEnumerable<TEntity> All<TEntity>() where TEntity : class, new()
        {
            return Setup<TEntity>().ToList();
        }

        public IEnumerable<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class, new()
        {
            if (exp == null) throw new ArgumentNullException("exp");
            return Setup<TEntity>().Where(exp).ToList();
        }

        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class, new()
        {
            return Setup<TEntity>().Any(exp);
        }

        public void Add<TEntity>(TEntity item) where TEntity : class, new()
        {
            if (item == null) throw new ArgumentNullException("item");

            _context.Set<TEntity>().Add(item);
            _context.SaveChanges();
        }

        public void Update<TEntity>(TEntity item) where TEntity : class, new()
        {
            if (item == null) throw new ArgumentNullException("item");

            _context.Entry(item).CurrentValues.SetValues(item);
            _context.SaveChanges();
        }

        /// <summary>
        /// The save will either add an item or update it if the expression matches a single item.
        /// If the expression is null then will always add new.
        /// If the epxression doesn't match anything then it will add.
        /// If it matches one item it will update that item.
        /// Will throw ArgumentNullException if no item passed in.
        /// Will throw if the expression returns more than one item.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        /// <param name="exp"></param>
        public void Save<TEntity>(TEntity item, Expression<Func<TEntity, bool>> exp) where TEntity : class, new()
        {
            if (item == null) throw new ArgumentNullException("item");

            //if no expression can only add
            if (exp == null)
            {
                Add(item);
            }
            else
            {
                //try and find it, if exists, update otherwise add.
                var entity = Setup<TEntity>().SingleOrDefault(exp);
                if (entity == null)
                {
                    Add(item);
                }
                else
                {
                    var entityType = typeof(TEntity);
                    if (entityType == typeof(BlogEntryEntity))
                    {
                        var blogEntry = item as BlogEntryEntity;
                        _context.UpdateGraph(blogEntry, m => m.OwnedCollection(i => i.Tags));

                    }
                    else
                    {
                        _context.Entry(entity).CurrentValues.SetValues(item);
                    }
                    _context.SaveChanges();

                }
            }
        }

        /// <summary>
        /// Expects the expression to match a single item only. Will throw if this is not meet.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp"></param>
        public void Delete<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class, new()
        {
            var item = Setup<TEntity>().Single(exp);
            if (item == null) throw new ArgumentNullException("item");

            _context.Set<TEntity>().Remove(item);
            _context.SaveChanges();
        }

        /// <summary>
        /// Counts all tags used by blog entries and replaces / updates the tag counts.
        /// </summary>
        public void UpdateTagCount()
        {
            var tags = Setup<BlogEntryEntity>().SelectMany(e => e.Tags).ToArray();

            var dictionary = new Dictionary<TagEntity, int>();
            foreach (var tag in tags)
            {
                if (!dictionary.ContainsKey(tag))
                {
                    dictionary.Add(tag, 0);
                }

                dictionary[tag] = ++dictionary[tag];
            }

            var items = _context.Set<TagCountEntity>().ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                _context.Set<TagCountEntity>().Remove(items[i]);
            }

            var list = dictionary.Select(kvp => new TagCountEntity
            {
                Tag = kvp.Key,
                Count = kvp.Value
            }).ToList();

            _context.Set<TagCountEntity>().AddRange(list);
            _context.SaveChanges();
        }

        /// <summary>
        /// Searches for any tags that are not being used by any  blog entry and deletes them.
        /// </summary>
        public void RemoveUnusedTags()
        {
            var usedTagNames = Setup<BlogEntryEntity>().SelectMany(e => e.Tags).Select(t => t.Name).Distinct().ToArray();

            var allTags = Setup<TagEntity>().ToArray();

            var unused = allTags.Where(tag => usedTagNames.All(t => t != tag.Name)).ToList();

            foreach (var tag in unused)
            {
                _context.Set<TagEntity>().Remove(tag);
            }

            _context.SaveChanges();
        }


        public IEnumerable<BlogEntryEntity> ListByTags(params string[] tags)
        {
            return Setup<BlogEntryEntity>()
                .Where(b => tags.All(t => b.Tags.Select(bt => bt.Name).Contains(t)))
                .ToArray();
        }
    }
}