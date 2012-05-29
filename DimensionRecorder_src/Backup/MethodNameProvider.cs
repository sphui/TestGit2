// (C) Jan Boettcher, <http://www.ib-boettcher.de>
// Licensed under the EUPL V.1.1
using System;
using System.Collections.Generic;
using System.Text;

namespace DimensionRecorder
{
    /// <summary>
    /// Utility
    /// </summary>
    public class MethodNameProvider
    {
        internal delegate void Callback();
        internal delegate int CallbackInt();
        /// <summary>
        /// Get the name of the given method as a string
        /// </summary>
        /// <param name="cb">method group (void f())</param>
        /// <returns></returns>
        internal static string getNameVoid(Callback cb)
        {
            return cb.Method.Name;
        }
        /// <summary>
        /// Get the name of the given method as a string
        /// </summary>
        /// <param name="cb">method group (int f())</param>
        /// <returns></returns>
        internal static string getNameInt(CallbackInt cb)
        {
            return cb.Method.Name;
        }

    }
}
