// (C) Jan Boettcher, <http://www.ib-boettcher.de>
// Licensed under the EUPL V.1.1
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DimensionRecorder
{
    public class COMInfoProvider
    {
        /// <summary>
        /// Gives back the types a com object
        /// (thanks to Fernando Felman, http://fernandof.wordpress.com/2008/02/05/how-to-check-the-type-of-a-com-object-system__comobject-with-visual-c-net/)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="hintType">A type within the Assembly to be scanned for types (any type, just to get the assembly)</param>
        /// <returns></returns>
        public static Type[] isAsignableTo(object obj, Type hintType)
        {
            List<Type> asignableTypes=new List<Type>();
            Assembly assy = Assembly.GetAssembly(hintType);

            IntPtr iUnknown = Marshal.GetIUnknownForObject(obj);
            
            //First search the given assy itself
            Type[] types = assy.GetTypes();
            foreach (Type type in types)
            {
                // get the iid of the current type
                Guid iid = type.GUID;
                if (!type.IsInterface || iid == Guid.Empty)
                {
                    // com interop type must be an interface with valid iid
                    continue;
                }

                // query supportability of current interface on object
                IntPtr ipointer = IntPtr.Zero;
                Marshal.QueryInterface(iUnknown, ref iid, out ipointer);

                if (ipointer != IntPtr.Zero)
                {
                    asignableTypes.Add(type);
                }
            }

            //... the search all assies referenced by the above assy
            AssemblyName[] assyNames = assy.GetReferencedAssemblies();
            foreach (AssemblyName assyName in assyNames) {
                Assembly refAssy = Assembly.ReflectionOnlyLoad(assyName.FullName);
                Type[] expTypes = refAssy.GetExportedTypes();
                foreach (Type type in expTypes)
                {
                    //if (type.IsInstanceOfType(obj)) asignableTypes.Add(type);
                    // get the iid of the current type
                    Guid iid = type.GUID;
                    if (!type.IsInterface || iid == Guid.Empty)
                    {
                        // com interop type must be an interface with valid iid
                        continue;
                    }

                    // query supportability of current interface on object
                    IntPtr ipointer = IntPtr.Zero;
                    Marshal.QueryInterface(iUnknown, ref iid, out ipointer);

                    if (ipointer != IntPtr.Zero)
                    {
                        asignableTypes.Add(type);
                    }
                }
            }
            
            return asignableTypes.ToArray();           
        }
        /// <summary>
        /// Gives back the type-names a com object
        /// (thanks to Fernando Felman, http://fernandof.wordpress.com/2008/02/05/how-to-check-the-type-of-a-com-object-system__comobject-with-visual-c-net/)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="hintType">A type within the Assembly to be scanned for types (any type, just to get the assembly)</param>
        /// <returns></returns>
        public static string[] isAsignableToNames(object obj, Type hintType)
        {
            Type[] types = COMInfoProvider.isAsignableTo(obj,hintType);
            string[] names = new string[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                names[i] = types[i].FullName;
            }
            return names;
        }

    }
}
