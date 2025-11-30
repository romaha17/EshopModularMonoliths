namespace Catalog.Products.Features.GetProductById
{
    class GetProductByIdHandler(CatalogDbContext dbContext)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            // get products by id using dbContext
            // return result

            var product = await dbContext.Products
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(query.Id);
            }

            var productDto = product.Adapt<ProductDto>();

            return new GetProductByIdResult(productDto);
        }
    }
}
