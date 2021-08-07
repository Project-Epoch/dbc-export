using System.Collections.Generic;
using System.IO;

namespace dbc_export
{
    class Header
    {
        /// <summary>
        /// The "magic" WDBC value that is mandatory for a DBC file.
        /// </summary>
        private uint Magic;

        /// <summary>
        /// Total number of rows.
        /// </summary>
        /// <value></value>
        public uint RecordCount { get; set; }

        /// <summary>
        /// Number of fields per row.
        /// </summary>
        /// <value></value>
        public uint FieldCount { get; set; }

        /// <summary>
        /// Total bytes per row.
        /// </summary>
        /// <value></value>
        public uint RecordSize { get; set; }

        /// <summary>
        /// Size of the string block. Dynamically expanded when writing.
        /// </summary>
        /// <value></value>
        public uint StringBlockSize { get; set; }

        /// <summary>
        /// Where our string block begins.
        /// </summary>
        /// <value></value>
        public uint StringBlockOffset { get; set; }

        /// <summary>
        /// Build a DBC header.
        /// </summary>
        /// <param name="RecordCount"></param>
        /// <param name="FieldCount"></param>
        /// <param name="RecordSize"></param>
        /// <param name="StringBlockSize"></param>
        public Header(uint RecordCount, uint FieldCount, uint RecordSize, uint StringBlockSize)
        {
            this.RecordCount = RecordCount;
            this.FieldCount = FieldCount;
            this.RecordSize = RecordSize;
            this.StringBlockSize = StringBlockSize;
            this.Magic =  1128416343; // Magic is always 'WDBC' https://wowdev.wiki/DBC
        }

        /// <summary>
        /// Helper for getting the constant size of a DBC header.
        /// </summary>
        /// <returns></returns>
        public uint GetHeaderLength()
        {
            return sizeof(uint) * 5;
        }

        /// <summary>
        /// Given a binary stream it will output our header file in the right 20 byte format.
        /// </summary>
        /// <param name="writer"></param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Magic);

            writer.Write(RecordCount);

            writer.Write(FieldCount);

            writer.Write(RecordSize);

            writer.Write(StringBlockSize);
        }
    }
}