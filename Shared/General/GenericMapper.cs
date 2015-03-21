using System;
using System.Collections.Generic;
using System.Linq;

namespace General
{
    public class GenericMapper<TEntity, TModel>
    {
        private const string DomainModelsNamespace = "Domain.Models";
        private const string RepositoryModelsNamespace = "Repository.Models";

        static GenericMapper()
        {            
            lock (typeof(AutoMapper.Mapper))
            {
                AutoMapper.Mapper.CreateMap<TEntity, TModel>().ReverseMap();
            }
        }

        public static void MapSubTypes()
        {
            lock (typeof(AutoMapper.Mapper))
            {
                var allProps = typeof(TEntity).GetProperties().ToList();
                allProps.AddRange(typeof(TModel).GetProperties().ToList());
                var allPropsArray = allProps.ToArray();
                var entityTypes = new List<Type>();
                var modelTypes = new List<Type>();

                foreach (var prop in allPropsArray)
                {
                    var type = prop.PropertyType.UnderlyingSystemType;
                    var isSpecial = prop.IsSpecialName;
                    CheckTypeForMapping(type, isSpecial, entityTypes, modelTypes);
                }

                foreach (var entitySubType in entityTypes)
                {
                    foreach (var modelSubType in modelTypes)
                    {
                        if (modelSubType.Name == entitySubType.Name.Replace("Dto", "").Replace("Entity", ""))
                        {
                            AutoMapper.Mapper.CreateMap(entitySubType, modelSubType);
                            AutoMapper.Mapper.CreateMap(modelSubType, entitySubType);
                        }
                    }
                }
            }
        }

        private static void CheckTypeForMapping(Type type, bool isSpecial, List<Type> entityTypes, List<Type> modelTypes)
        {
            var typeName = type.FullName;
            var isEntityType = typeName.Contains(RepositoryModelsNamespace);
            var isModelType = typeName.Contains(DomainModelsNamespace);

            if (isSpecial) return;

            if (type.IsGenericType)
            {
                var typeArguments = type.GetGenericArguments();
                foreach (var typeArgument in typeArguments)
                {
                    CheckTypeForMapping(typeArgument, false, entityTypes, modelTypes);
                }
            }
            else
            {
                if (isEntityType)
                {
                    entityTypes.Add(type);
                }
                if (isModelType)
                {
                    modelTypes.Add(type);
                }
            }
        }
        

        public static TEntity ToEntity(TModel model)
        {
            if (model == null)
                return default(TEntity);

            return AutoMapper.Mapper.Map<TEntity>(model);
        }

        public static TModel ToModel(TEntity entity)
        {
            if (entity == null)
                return default(TModel);

            return AutoMapper.Mapper.Map<TModel>(entity);
        }

        public static TEntity ToEntityWithSubTypes(TModel model)
        {
            if (model == null)
                return default(TEntity);

            MapSubTypes();
            return ToEntity(model);
        }

        public static TModel ToModelWithSubTypes(TEntity entity)
        {
            if (entity == null)
                return default(TModel);

            MapSubTypes();
            return ToModel(entity);
        }
    }
}


