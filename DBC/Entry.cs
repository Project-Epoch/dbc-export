using System.Collections.Generic;

namespace dbc_export
{
    class Entry
    {
		/// <summary>
		/// The values (columns) for this row.
		/// </summary>
		/// <value></value>
        public List<Value> Values { get; set; }

		/// <summary>
		/// Based on the data types within an entry figure out it's size in bytes.
		/// </summary>
		/// <returns></returns>
        public uint CalculateSize()
        {
            uint size = 0;

            foreach (Value value in Values)
            {
                switch (value.Field.Type)
                {
                    case "sbyte":
							size += sizeof(sbyte);
							break;
						case "byte":
							size += sizeof(byte);
							break;
						case "int32":
						case "int":
                            size += sizeof(int);
							break;
						case "uint32":
						case "uint":
                            size += sizeof(uint);
							break;
						case "int64":
						case "long":
                            size += sizeof(long);
							break;
						case "uint64":
						case "ulong":
                            size += sizeof(ulong);
							break;
						case "single":
						case "float":
                            size += sizeof(float);
							break;
						case "boolean":
						case "bool":
                            size += sizeof(bool);
							break;
						case "string":
                            size += sizeof(int);
							break;
						case "int16":
						case "short":
							size += sizeof(short);
							break;
						case "uint16":
						case "ushort":
							size += sizeof(ushort);
							break;
						case "loc":
							// todo
							break;
                }
            }

            return size;
        }
    }
}