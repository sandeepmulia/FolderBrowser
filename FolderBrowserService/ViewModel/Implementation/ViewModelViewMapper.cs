using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FolderBrowser.View;

namespace FolderBrowser.ViewModel.Implementation
{
    /// <summary>
    /// A simple ViewModel to View Mapper Helper class which is used while resolving of objects
    /// during invocation of ShowDialog
    /// </summary>
    public static class ViewModelViewMapper
    {
        private static Dictionary<Type, Type> viewViewModelMappings;

        static ViewModelViewMapper()
        {
            viewViewModelMappings = new Dictionary<Type, Type>()
            {
                {typeof(MainDialogViewModel),typeof(string)},
                {typeof(FolderBrowserDialogViewModel), typeof(FolderBrowserDialogWrapper)},
                {typeof(DisplayBrowserDialogViewModel), typeof(DisplayBrowser)},
                {typeof(DetailFileInfoViewModel), typeof(DetailsWindow)}
            };
        }

        public static Type GetViewModelDialogMapping(Type key)
        {
            return viewViewModelMappings.GetValue<Type, Type>(key);
        }
    }

    /// <summary>
    /// Classic Wrapper over the dictionary. This gives verbose information about the key
    /// which is not found.
    /// </summary>
    public static class Helper
    {
        public static V GetValue<K, V>(this Dictionary<K, V> dictionary, K key)
        {
            V result = default(V);

            if (dictionary.ContainsKey(key))
            {
                result = dictionary[key];
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Key {0} not found in dictionary", key));
            }
            return result;
        }

    }
}
