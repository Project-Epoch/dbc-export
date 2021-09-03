using System.Collections.Generic;

namespace dbc_export
{
    /// <summary>
    /// The actual definition of a DBC file. What it is, where we get it from, the fields it has.
    /// </summary>
    class Definition
    {
        /// <summary>
        /// The name of the DBC that we are generating.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// The Database Table that we should grab this data from.
        /// </summary>
        /// <value></value>
        public string Table { get; set; }

        /// <summary>
        /// All of the Fields that this DBC uses.
        /// </summary>
        /// <value></value>
        public List<Field> Fields { get; set; }

        /// <summary>
        /// The column we should order by when querying
        /// </summary>
        /// <value></value>
        public string OrderBy { get; set; } = "none";

        /// <summary>
        /// The direction to sort the above column.
        /// </summary>
        /// <value></value>
        public string OrderDirection { get; set; } = "none";
    }
}