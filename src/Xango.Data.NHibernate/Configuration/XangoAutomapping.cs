using System;
using FluentNHibernate.Automapping;

namespace Xango.Data.NHibernate.Configuration
{
    public class XangoAutomapping : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return typeof(Entity).IsAssignableFrom(type);
        }
        
        public override bool IsComponent(Type type)
        {
            return (typeof(IComponent)).IsAssignableFrom(type);
        }
    }
}