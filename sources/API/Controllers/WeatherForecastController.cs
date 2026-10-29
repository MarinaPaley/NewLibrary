// <copyright file="WeatherForecastController.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для работы с прогнозом погоды.
    /// </summary>
    [ApiController]
    [Route("api/weather-forecasts/")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
        ];

        /// <summary>
        /// Получает прогноз погоды.
        /// </summary>
        /// <returns>
        /// <c>200</c> и сериализованная в JSON коллекция прогнозов по дням.
        /// </returns>
        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK)]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            })
            .ToArray();
        }
    }
}
