using Common.Enums.DependencyInjection;

namespace Common.Attributes.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DependencyInjectionAttribute : Attribute
    {
        public DependencyInjectionTypeEnum? DependencyType { get; private set; }

        public DependencyInjectionAttribute(Type? interfaceType = null, DependencyInjectionTypeEnum dependencyType = DependencyInjectionTypeEnum.Scope)
        {
            DependencyType = dependencyType;
        }
    }
}
