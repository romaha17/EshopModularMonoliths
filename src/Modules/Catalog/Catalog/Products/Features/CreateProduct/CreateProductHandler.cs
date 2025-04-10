namespace Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(ProductDto Product)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    class CreateProductHandler(CatalogDbContext dbContext)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            // create Product entity from command object
            // save to database
            // return result

            var product = CreateNewPRoduct(command.Product);

            dbContext.Add(product);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }

        private Product CreateNewPRoduct(ProductDto productDto)
        {
            var product = Product.Create(
                Guid.NewGuid(),
                productDto.Name,
                productDto.Category,
                productDto.Description,
                productDto.ImageFile,
                productDto.Price);
            return product;
        }
    }
}
