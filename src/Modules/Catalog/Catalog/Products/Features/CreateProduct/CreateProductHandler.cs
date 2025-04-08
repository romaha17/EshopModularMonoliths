﻿using MediatR;

namespace Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand
        (string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : IRequest<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Business logic to create a product
            throw new NotImplementedException();
        }
    }
}
