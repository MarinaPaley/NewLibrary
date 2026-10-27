// <copyright file="BaseExampleTransformer.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace API.Examples
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Nodes;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.OpenApi;
    using Microsoft.OpenApi;

    /// <summary>
    /// Базовый генератор примеров для использования <typeparamref name="TModel"/>.
    /// </summary>
    /// <typeparam name="TModel"> Целевой тип, для использования которого нужны примеры. </typeparam>
    public abstract class BaseExampleTransformer<TModel> : IOpenApiSchemaTransformer
    {
        /// <inheritdoc/>
        public Task TransformAsync(
            OpenApiSchema schema,
            OpenApiSchemaTransformerContext context,
            CancellationToken cancellationToken)
        {
            // context.JsonTypeInfo.Type позволяет понять, для какого C# класса сейчас строится схема
            if (context.JsonTypeInfo.Type == typeof(TModel))
            {
                schema.Examples = this.GetExamples()
                    .Select(this.Serialize)
                    .ToList();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает перечисление объектов-примеров.
        /// </summary>
        /// <returns> Перечисление объектов-примеров. </returns>
        protected abstract IEnumerable<TModel> GetExamples();

        /// <summary>
        /// Сериализует (отображает) <typeparamref name="TModel"/> в <see cref="JsonNode"/>.
        /// </summary>
        /// <param name="model"> Объект-пример. </param>
        /// <returns> Сериализованное представление (<c>JSON</c>). </returns>
        protected abstract JsonNode Serialize(TModel model);
    }
}
