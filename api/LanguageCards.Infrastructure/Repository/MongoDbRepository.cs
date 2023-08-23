using System.Linq.Expressions;
using LanguageCards.Domain.Abstractions;
using MongoDb.Options;
using MongoDB.Driver;

namespace MongoDb.Repository
{
    internal class MongoDbRepository<TDocument> : IRepository<TDocument> where TDocument : BaseEntity
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoDbRepository(MongoDbOptions settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName());
        }

        protected string GetCollectionName()
        {
            return typeof(TDocument).Name;
        }

        public IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync(cancellationToken);
        }

        public TDocument FindById(Guid id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return _collection.Find(filter).SingleOrDefault();
        }

        public async Task<TDocument> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);
        }

        public void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }

        public async Task InsertOneAsync(TDocument document, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }

        public async Task InsertManyAsync(ICollection<TDocument> documents, CancellationToken cancellationToken)
        {
            await _collection.InsertManyAsync(documents, cancellationToken: cancellationToken);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public async Task ReplaceOneAsync(TDocument document, CancellationToken cancellationToken)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document, cancellationToken: cancellationToken);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression, cancellationToken: cancellationToken);
        }

        public void DeleteById(Guid id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            _collection.FindOneAndDelete(filter);
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken)
        {
            await _collection.DeleteManyAsync(filterExpression, cancellationToken);
        }
    }
}
