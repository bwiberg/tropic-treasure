// ----------------------------------------------------------------------------
// LICENSE
// This is free and unencumbered software released into the public domain.
//
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
//
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
// For more information, please refer to <http://unlicense.org>
// ----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// Tuple structs for use in .NET Not-Quite-3.5 (e.g. Unity3D).
//
// Used Chapter 3 in http://functional-programming.net/ as a starting point.
//
// Note: .NET 4.0 Tuples are immutable classes so they're *slightly* different.
// ----------------------------------------------------------------------------

namespace Utility {
    /// <summary>
    /// Utility class that simplifies cration of tuples by using
    /// method calls instead of constructor calls
    /// </summary>
    public static class Tuple {
        /// <summary>
        /// Creates a new tuple value with the specified elements. The method
        /// can be used without specifying the generic parameters, because C#
        /// compiler can usually infer the actual types.
        /// </summary>
        /// <param name="item1">First element of the tuple</param>
        /// <param name="second">Second element of the tuple</param>
        /// <returns>A newly created tuple</returns>
        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 second) {
            return new Tuple<T1, T2>(item1, second);
        }

        /// <summary>
        /// Creates a new tuple value with the specified elements. The method
        /// can be used without specifying the generic parameters, because C#
        /// compiler can usually infer the actual types.
        /// </summary>
        /// <param name="item1">First element of the tuple</param>
        /// <param name="second">Second element of the tuple</param>
        /// <param name="third">Third element of the tuple</param>
        /// <returns>A newly created tuple</returns>
        public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 second, T3 third) {
            return new Tuple<T1, T2, T3>(item1, second, third);
        }

        /// <summary>
        /// Creates a new tuple value with the specified elements. The method
        /// can be used without specifying the generic parameters, because C#
        /// compiler can usually infer the actual types.
        /// </summary>
        /// <param name="item1">First element of the tuple</param>
        /// <param name="second">Second element of the tuple</param>
        /// <param name="third">Third element of the tuple</param>
        /// <param name="fourth">Fourth element of the tuple</param>
        /// <returns>A newly created tuple</returns>
        public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 second, T3 third, T4 fourth) {
            return new Tuple<T1, T2, T3, T4>(item1, second, third, fourth);
        }


        /// <summary>
        /// Extension method that provides a concise utility for unpacking
        /// tuple components into specific out parameters.
        /// </summary>
        /// <param name="tuple">the tuple to unpack from</param>
        /// <param name="ref1">the out parameter that will be assigned tuple.Item1</param>
        /// <param name="ref2">the out parameter that will be assigned tuple.Item2</param>
        public static void Unpack<T1, T2>(this Tuple<T1, T2> tuple, out T1 ref1, out T2 ref2) {
            ref1 = tuple.Item1;
            ref2 = tuple.Item2;
        }

        /// <summary>
        /// Extension method that provides a concise utility for unpacking
        /// tuple components into specific out parameters.
        /// </summary>
        /// <param name="tuple">the tuple to unpack from</param>
        /// <param name="ref1">the out parameter that will be assigned tuple.Item1</param>
        /// <param name="ref2">the out parameter that will be assigned tuple.Item2</param>
        /// <param name="ref3">the out parameter that will be assigned tuple.Item3</param>
        public static void Unpack<T1, T2, T3>(this Tuple<T1, T2, T3> tuple, out T1 ref1, out T2 ref2, T3 ref3) {
            ref1 = tuple.Item1;
            ref2 = tuple.Item2;
            ref3 = tuple.Item3;
        }

        /// <summary>
        /// Extension method that provides a concise utility for unpacking
        /// tuple components into specific out parameters.
        /// </summary>
        /// <param name="tuple">the tuple to unpack from</param>
        /// <param name="ref1">the out parameter that will be assigned tuple.Item1</param>
        /// <param name="ref2">the out parameter that will be assigned tuple.Item2</param>
        /// <param name="ref3">the out parameter that will be assigned tuple.Item3</param>
        /// <param name="ref4">the out parameter that will be assigned tuple.Item4</param>
        public static void Unpack<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple, out T1 ref1, out T2 ref2, T3 ref3,
            T4 ref4) {
            ref1 = tuple.Item1;
            ref2 = tuple.Item2;
            ref3 = tuple.Item3;
            ref4 = tuple.Item4;
        }
    }
}