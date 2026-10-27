// <copyright file="OpenApiExtensions.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace API.Extensions
{
    using API.Examples;
    using Microsoft.AspNetCore.OpenApi;

    /// <summary>
    /// Коллекция методов-расширений для настройки <c>OpenAPI</c>.
    /// </summary>
    public static class OpenApiExtensions
    {
        /// <summary>
        /// Добавляет трансформеры для создания примеров.
        /// </summary>
        /// <param name="options"> Опции настройки <c>OpenAPI</c>. </param>
        /// <returns> Обновлённые опции настройки <c>OpenAPI</c>. </returns>
        public static OpenApiOptions AddAllSchemaTransformers(this OpenApiOptions options)
        {
            // Перечисляем все трансформеры в нужном порядке
            _ = options.AddSchemaTransformer<WeatherForecastExampleTransformer>();

            return options;
        }
    }
}
