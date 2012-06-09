using MongoDB.Driver;

namespace Import
{
    public interface IMongoEntity
    {
        
        Oid GetOid();

        Document GetAsDocument();
        void UpdateFromDocument(Document document);
    }
}