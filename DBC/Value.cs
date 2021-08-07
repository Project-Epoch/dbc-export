using System;

namespace dbc_export
{
    class Value
    {
        /// <summary>
        /// The field this value is for.
        /// </summary>
        /// <value></value>
        public Field Field { get; set; }
        
        /// <summary>
        /// Data we got from the DB. Object as this could be many types.
        /// </summary>
        /// <value></value>
        public object Data {get; set; }
    }
}