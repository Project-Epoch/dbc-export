using System;

namespace dbc_export
{
    /// <summary>
    /// A definition of a Field within a DBC file.
    /// </summary>
    class Field
    {
        /// <summary>
        /// The name of the field.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// The type of the field eg byte, int, float
        /// </summary>
        /// <value></value>
        public string Type { get; set; }

        /// <summary>
        /// Whether we need to automatically generate an array of fields with this name. 
        /// Eg CharStartOutfit ItemID.
        /// Defaults to false.
        /// </summary>
        /// <value></value>
        public bool Array { get; set; } = false;

        /// <summary>
        /// If Array is true this is the number of elements we'll need to generate.
        /// </summary>
        /// <value></value>
        public int Size { get; set; } = 1;

        public Type GetParsedType()
        {
            switch (Type.ToLower())
            {
                case "sbyte":
                    return typeof(sbyte);
                case "byte":
                    return typeof(byte);
                case "int32":
                case "int":
                    return typeof(int);
                case "uint32":
                case "uint":
                    return typeof(uint);
                case "int64":
                case "long":
                    return typeof(long);
                case "uint64":
                case "ulong":
                    return typeof(ulong);
                case "single":
                case "float":
                    return typeof(float);
                case "boolean":
                case "bool":
                    return typeof(bool);
                case "string":
                    return typeof(string);
                case "int16":
                case "short":
                    return typeof(short);
                case "uint16":
                case "ushort":
                    return typeof(ushort);
                default:
                    throw new Exception("Could not find matching data type.");
            }
        }
    }
}