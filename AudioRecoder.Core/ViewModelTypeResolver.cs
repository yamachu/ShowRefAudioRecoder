using System;
using System.Reflection;

namespace AudioRecoder.Core
{
    public class ViewModelTypeResolver
    {
        private static readonly Assembly LocalAssembly = typeof(ViewModelTypeResolver).GetTypeInfo().Assembly;
        public static Type Resolve(Type viewType)
        {
            if (viewType == null) throw new ArgumentNullException(nameof(viewType));

            var vmTypeName = $"{viewType.Namespace.Replace("Forms", "Core").Replace("Views", "ViewModels")}.{viewType.Name}ViewModel";
            return LocalAssembly.GetType(vmTypeName);
        }
    }
}
