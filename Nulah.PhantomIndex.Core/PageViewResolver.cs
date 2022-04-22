using System;
using System.Reflection;
using System.Windows.Controls;

namespace Nulah.PhantomIndex.Core
{
    public static class PageViewResolver
    {
        private static Type[] _defaultPageViewParameterConstructorTypes = new Type[] { typeof(string) };

        public static (Type PageView, string[] PageViewParameters) ResolvePageViewFromAssembly(Type parentAssemblyType, string pageViewLocation)
        {
            var callingNamespace = parentAssemblyType.Namespace;
            var pathFragments = ResolvePageViewLocation(pageViewLocation);
            string[] pageviewParameters = null;

            if (pathFragments.Length > 0)
            {
                pageviewParameters = GetFragmentParamters(pathFragments[^1]);
                pathFragments[pathFragments.Length - 1] = ClearFragmentParameters(pathFragments[pathFragments.Length - 1]);
            }

            var typeName = $"{callingNamespace}.{string.Join(".", pathFragments)}";
            var view = parentAssemblyType.Assembly.GetType(typeName);

            return (view, pageviewParameters);
        }

        public static (Type PageView, string[] PageViewParameters) ResolvePageViewFromAssembly(Assembly executingAssembly, string pageViewLocation)
        {
            var pathFragments = ResolvePageViewLocation(pageViewLocation);
            string[] pageviewParameters = null;

            if (pathFragments.Length > 0)
            {
                pageviewParameters = GetFragmentParamters(pathFragments[^1]);
                pathFragments[pathFragments.Length - 1] = ClearFragmentParameters(pathFragments[pathFragments.Length - 1]);
            }

            var typeName = $"{executingAssembly.GetName().Name}.{string.Join(".", pathFragments)}";
            var view = executingAssembly.GetType(typeName);

            return (view, pageviewParameters);
        }

        public static T GetActivatedPageViewByParameters<T>(Type pageViewType, string[] pageViewParameters)
        {
            if (pageViewParameters == null || pageViewParameters.Length == 0)
            {
                return (T)Activator.CreateInstance(pageViewType);
            }
            else if (pageViewParameters.Length == 1)
            {
                if (pageViewType.GetConstructor(_defaultPageViewParameterConstructorTypes) != null)
                {
                    return (T)Activator.CreateInstance(pageViewType, pageViewParameters);
                }

                return default(T);
            }
            else
            {
                throw new ArgumentException("Too many page view parameters supplied - multiple parameter constructor resolution not yet supported");
            }
        }

        private static string[] ResolvePageViewLocation(string pageViewLocation)
        {
            var fragments = pageViewLocation.Split('/');


            return fragments;
        }

        private static string ClearFragmentParameters(string paramaterisedFragment)
        {
            if (paramaterisedFragment.IndexOf(':') is int paramIndex && paramIndex > 0)
            {
                return paramaterisedFragment.Substring(0, paramIndex);
            }

            return paramaterisedFragment;
        }

        private static string[] GetFragmentParamters(string paramaterisedFragment)
        {
            if (paramaterisedFragment.IndexOf(':') is int paramIndex && paramIndex > 0)
            {
                return paramaterisedFragment.Substring(paramIndex + 1, paramaterisedFragment.Length - paramIndex - 1).Split(':');
            }

            return null;
        }
    }
}