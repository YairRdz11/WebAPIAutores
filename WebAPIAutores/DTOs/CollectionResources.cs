namespace WebAPIAutores.DTOs
{
    public class CollectionResources<T> : Resource where T : Resource
    {
        public List<T> Values { get; set; }
    }
}
