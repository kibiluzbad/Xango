using System;
using FluentNHibernate.Automapping;

namespace Xango.Data.NHibernate.Configuration
{
    public class XangoAutomapping : DefaultAutomappingConfiguration
    {
           public override bool ShouldMap(Type type)
        {
            var result = IsAssignableToGenericType(type, typeof(Entity<>));
            return result;
        }
        
        public override bool IsComponent(Type type)
        {
            return (typeof(IComponent)).IsAssignableFrom(type);
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
                if (it.IsGenericType)
                    if (it.GetGenericTypeDefinition() == genericType) return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == genericType ||
                IsAssignableToGenericType(baseType, genericType);
        }
    }
}