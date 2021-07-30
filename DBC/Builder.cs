using MySqlConnector;
using System;
using System.Collections.Generic;

namespace dbc_export
{
    class Builder
    {
        private Definition definition;

        private MySqlConnection connection;

        public Builder(MySqlConnection connection, Definition definition)
        {
            this.connection = connection;
            this.definition = definition;
        }

        public void Run()
        {
            if (! TableExists())
            {
                Console.WriteLine(String.Format("Could not find matching table for {0} ({1})! Skipping.", definition.Name, definition.Table));

                return;
            }

            Console.WriteLine(String.Format("Beginning Export of {0}", definition.Name));

            GenerateExtraFields();

            List<Entry> entries;

            try
            {
                entries = GetFromDatabase();
            }
            catch (DataNotFoundException)
            {
                Console.WriteLine(String.Format("No Data found for {0} in table {1}", definition.Name, definition.Table));

                return;
            }
            
            Writer writer = new Writer(definition, entries);
            writer.Run();
        }

        private void GenerateExtraFields()
        {
            List<Field> newFields = new List<Field>();

            foreach (Field field in definition.Fields)
            {
                if (! field.Array)
                {
                    // Not an array or localisation, just add as a singular field.
                    if (field.Type.ToLower() != "loc")
                    {
                        newFields.Add(field);

                        continue;
                    }
                }

                // Not localisation but is an array. Add array fields. 
                if (field.Type.ToLower() != "loc")
                {
                    for (int i = 0; i < field.Size; i++)
                    {
                        newFields.Add(new Field {
                            Name = String.Format("{0}_{1}", field.Name, i + 1),
                            Type = field.Type,
                            Index = field.Index,
                            Autogenerate = field.Autogenerate,
                            Array = false,
                            Size = 1,
                        });
                    }

                    continue;
                }
                
                // Must be localisation now.
                Array languages = Enum.GetValues(typeof(Languages));

                for (int i = 0; i < languages.Length; i++)
                {
                    var blah = String.Format("{0}_{1}", field.Name, languages.GetValue(i).ToString());

                    newFields.Add(new Field {
                        Name = String.Format("{0}_{1}", field.Name, languages.GetValue(i).ToString()),
                        Type = field.Type,
                        Index = field.Index,
                        Autogenerate = field.Autogenerate,
                        Array = false,
                        Size = 1,
                    });
                }

                // Mask field
                newFields.Add(new Field {
                    Name = String.Format("{0}_Mask", field.Name),
                    Type = field.Type,
                    Index = field.Index,
                    Autogenerate = field.Autogenerate,
                    Array = false,
                    Size = 1,
                    InQuery = false,
                });
            }

            definition.Fields = newFields;
        }

        private List<Entry> GetFromDatabase()
        {
            string fields = "";

            int columnCount = definition.Fields.Count;

            foreach (Field field in definition.Fields)
            {
                fields = fields + String.Format("{0}, ", field.Name);
            }

            fields = fields.Substring(0, fields.Length - 2); // Hacky. Removes the final comma and space.

            MySqlCommand query = new MySqlCommand(String.Format("SELECT {0} FROM {1}", fields, definition.Table), connection);

            MySqlDataReader result = query.ExecuteReader();

            if (! result.HasRows) 
            {
                result.Close();

                throw new DataNotFoundException();
            }

            List<Entry> entries = new List<Entry>();

            int rows = 0;
            while (result.Read())
            {
                Entry entry = new Entry();

                for (int i = 0; i < columnCount; i++)
                {
                    Console.WriteLine(result[i].GetType().ToString());

                    entry.Values.Add(new Value {
                        Field = definition.Fields[i],
                        Data = result[i],
                    });
                }

                entries.Add(entry);

                rows++;
            }

            Console.WriteLine("Retrieved {0} Rows for {1}", rows, definition.Name);

            result.Close();

            return entries;
        }

        private bool TableExists()
        {
            MySqlCommand query = new MySqlCommand(string.Format("SHOW TABLES LIKE '{0}'", definition.Table), connection);

            MySqlDataReader result = query.ExecuteReader();

            bool found = result.HasRows;

            result.Close();

            return found;
        }
    }
}