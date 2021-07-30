namespace dbc_export
{
    class Value<T>
    {
        public Field Field { get; set; }
        
        public T Data {get; set; }
    }
}