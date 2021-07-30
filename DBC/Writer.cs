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
            uint size = 0;

            foreach (Entry entry in entries)
            {
                size += entry.CalculateSize();
            }

            using (var fileStream = new FileStream(outputPath, FileMode.Create))
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                /** Write Dummy Header */
                Header header = new Header(1, 1, 1, 1);
                header.Write(binaryWriter);

                // Write each record
                foreach (Entry entry in entries)
                {
                    foreach (object value in entry.Values)
                    {
                        Value obj = (Value) value;

                        binaryWriter.Write(obj.Data.);
                    }
                }

                // write real header. Todo

                // Finish Up
                memoryStream.Position = 0;
				memoryStream.CopyTo(fileStream);
            }

            Console.WriteLine("Finished Writing to {0}", outputPath);
        }
    }
}