using System.Collections.Generic;
using System.IO;

namespace dbc_export
{
    class Header
    {
        private uint Magic;

        public uint RecordCount { get; set; }

        public uint FieldCount { get; set; }

        public uint RecordSize { get; set; }

        public uint StringBlockSize { get; set; }

        public uint StringBlockOffset { get; set; }

        public Header(uint RecordCount, uint FieldCount, uint RecordSize, uint StringBlockSize)
        {
            this.RecordCount = RecordCount;
            this.FieldCount = FieldCount;
            this.RecordSize = RecordSize;
            this.StringBlockSize = StringBlockSize;
            this.Magic =  1128416343; // Magic is always 'WDBC' https://wowdev.wiki/DBC
        }

        public uint GetHeaderLength()
        {
            return sizeof(uint) * 5;
        }

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