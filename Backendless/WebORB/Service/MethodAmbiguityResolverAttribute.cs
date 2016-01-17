using System;

namespace Weborb.Service
    {
    [AttributeUsage(AttributeTargets.Class)]
    class MethodAmbiguityResolverAttribute : Attribute
        {
        private Type resolverType;

        public Type ResolverType
            {
            get { return resolverType; }
            set { resolverType = value; }
            }
        }
    }
