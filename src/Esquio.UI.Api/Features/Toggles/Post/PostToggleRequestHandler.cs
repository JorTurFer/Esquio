﻿using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles.Post
{
    public class DetailsToggleRequestHandler : IRequestHandler<PostToggleRequest, int>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsToggleRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }

        public async Task<int> Handle(PostToggleRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Include(t => t.Toggles)
                .Where(t => t.Id == request.FeatureId)
                .SingleOrDefaultAsync(cancellationToken);

            var isExistingType = feature.Toggles
                .Any(t => t.Type == request.ToggleType);

            if (!isExistingType)
            {
                var toggle = new ToggleEntity(feature.Id, request.ToggleType);

                foreach (var item in request.Parameters)
                {
                    toggle.Parameters.Add(new ParameterEntity(toggle.Id, item.Key, item.Value));
                }

                feature.Toggles.Add(toggle);
                await _storeDbContext.SaveChangesAsync();

                return toggle.Id;
            }
            else
            {
                throw new InvalidOperationException($"Toggle with type {request.ToggleType} already exist on this feature.");
            }
        }
    }
}
