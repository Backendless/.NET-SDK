using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Weborb.Service
{
    public class AssembliesRegistry
    {
        #region Assembly Full Name Manipulation API Methods
        public static String GetAssemblyNameFromFullName(String fullAssemblyName)
        {
            String[] fullNameParts = getAssemblyFullNameParts(fullAssemblyName);
            return fullNameParts[0];
        }
        public static String GetAssemblyVersionFromFullName(String fullAssemblyName)
        {
            String[] fullNameParts = getAssemblyFullNameParts(fullAssemblyName);
            return fullNameParts[1].Replace("Version=", "");
        }
        public static String GetAssemblyCultureFromFullName(String fullAssemblyName)
        {
            String[] fullNameParts = getAssemblyFullNameParts(fullAssemblyName);
            return fullNameParts[2].Replace("Culture=", "");
        }
        public static String GetAssemblyTokenFromFullName(String fullAssemblyName)
        {
            String[] fullNameParts = getAssemblyFullNameParts(fullAssemblyName);
            return fullNameParts[3].Replace("PublicKeyToken=", "");
        }

        private static String[] getAssemblyFullNameParts(String fullAssemblyName)
        {
            String[] assemblyNameParts = fullAssemblyName.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            List<String> returnParts = new List<string>();

            foreach (String s in assemblyNameParts)
                returnParts.Add(s.Trim());

            return returnParts.ToArray();
        }

        private static int getVersionAsInteger(string version)
        {
            version = version.Replace(".", "");

            return Convert.ToInt32(version);
        }
        #endregion

        public static Assembly[] RemoveDuplicateAssemblies(Assembly[] assembliesArray)
        {
            Dictionary<String, List<Assembly>> assemblyGrouping = new Dictionary<string, List<Assembly>>();

            foreach (Assembly assembly in assembliesArray)
            {
                String assemblyName = AssembliesRegistry.GetAssemblyNameFromFullName(assembly.FullName);

                if (!assemblyGrouping.ContainsKey(assemblyName))
                    assemblyGrouping.Add(assemblyName, new List<Assembly>());

                assemblyGrouping[assemblyName].Add(assembly);
            }

            IEnumerator<KeyValuePair<String, List<Assembly>>> iter = assemblyGrouping.GetEnumerator();

            List<Assembly> assemblies = new List<Assembly>();

            while (iter.MoveNext())
            {
                if (iter.Current.Value.Count == 1)
                {
                    assemblies.Add(iter.Current.Value[0]);
                    continue;
                }

                Assembly assemblyToAdd = null;
                foreach (Assembly ass in iter.Current.Value)
                {
                    if (assemblyToAdd == null)
                    {
                        assemblyToAdd = ass;
                        continue;
                    }

                    if (getVersionAsInteger(GetAssemblyVersionFromFullName(ass.FullName)) >=
                        getVersionAsInteger(GetAssemblyVersionFromFullName(assemblyToAdd.FullName)))
                    {
                        assemblyToAdd = ass;
                        continue;
                    }
                }

                assemblies.Add(assemblyToAdd);
            }

            return assemblies.ToArray();
        }
    }
}
