using System;

namespace Nulah.PhantomIndex.Core
{
    public static class PageViewResolver
    {
        public static (Type PageView, string[] PageViewParameters) ResolvePageViewFromAssembly(this Type parentAssemblyType, string pageViewLocation)
        {
            var callingNamespace = parentAssemblyType.Namespace;
            var pathFragments = ResolvePageViewLocation(pageViewLocation);
            string[] pageviewParameters = null;

            if (pathFragments.Length > 0)
            {
                pageviewParameters = GetFragmentParamters(pathFragments[^1]);
                pathFragments[pathFragments.Length - 1] = ClearFragmentParameters(pathFragments[pathFragments.Length - 1]);
            }

            var typeName = $"{callingNamespace}.Pages.{string.Join(".", pathFragments)}";
            var view = parentAssemblyType.Assembly.GetType(typeName);

            return (view, pageviewParameters);
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