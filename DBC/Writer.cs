using System;
using System.Collections.Generic;
using System.IO;

namespace dbc_export
{
    class Writer
    {
        List<Entry> entries;
        Definition definition;

        public Writer(Definition definition, List<Entry> entries)
        {
            this.definition = definition;
            this.entries = entries;
        }

        public void Run()
        {
            string outputPath = String.Format("{0}/built/{1}.dbc", Directory.GetCurrentDirectory(), definition.Name);

            Directory.CreateDirectory(String.Format("{0}/built", Directory.GetCurrentDirectory()));

            using (var fileStream = new FileStream(outputPath, FileMode.Create))
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                /** Write Dummy Header */
                // Header header = new Header(1, 1, 1, 1);

                Header header = new Header(0, 77, 296, 1);
                header.Write(binaryWriter);

                uint fieldCount = (uint) entries[0].Values.Count;
                uint recordCount = (uint) entries.Count;
                uint recordSize = entries[0].CalculateSize();
                uint stringBlockOffset = header.GetHeaderLength() + recordCount * recordSize;
                uint stringBlockSize = 1;

                // Write each record
                foreach (Entry entry in entries)
                {
                    foreach (object value in entry.Values)
                    {
                        Value obj = (Value) value;

                        switch (obj.Field.Type.ToLower())
                        {
                            case "sbyte":
                                binaryWriter.Write((sbyte) obj.Data);
                                break;
                            case "byte":
                                binaryWriter.Write((byte) obj.Data);
                                break;
                            case "int32":
                            case "int":
                                binaryWriter.Write((int) obj.Data);
                                break;
                            case "uint32":
                            case "uint":
                                binaryWriter.Write((uint) obj.Data);
                                break;
                            case "int64":
                            case "long":
                                binaryWriter.Write((long) obj.Data);
                                break;
                            case "uint64":
                            case "ulong":
                                binaryWriter.Write((ulong) obj.Data);
                                break;
                            case "single":
                            case "float":
                                binaryWriter.Write((float) obj.Data);
                                break;
                            case "boolean":
                            case "bool":
                                binaryWriter.Write((bool) obj.Data);
                                break;
                            case "string":
                                binaryWriter.Write((string) obj.Data);
                                break;
                            case "int16":
                            case "short":
                                binaryWriter.Write((short) obj.Data);
                                break;
                            case "uint16":
                            case "ushort":
                                binaryWriter.Write((ushort) obj.Data);
                                break;
                        }
                    }
                }

                // Write final header
                header = new Header(recordCount, fieldCount, recordSize, stringBlockSize);
                binaryWriter.Seek(0, SeekOrigin.Begin);
                header.Write(binaryWriter);

                // Finish Up
                memoryStream.Position = 0;
				memoryStream.CopyTo(fileStream);

                fileStream.Close();
            }

            Console.WriteLine("Finished Writing to {0}", outputPath);
        }
    }
}