using System.Collections.Generic;
using System.IO;

namespace dbc_export
{
    class Header
    {
        private uint Magic;

        private uint RecordCount;

        private uint FieldCount;

        private uint RecordSize;

        private uint StringBlockSize;

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

        public uint GenerateStringOffsets(List<Entry> Entries)
        {
            uint offset = 1;

            

            return offset;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Magic);

            writer.Write(RecordCount);

            writer.Write(FieldCount);

            writer.Write((uint)1);
        }
    }
}