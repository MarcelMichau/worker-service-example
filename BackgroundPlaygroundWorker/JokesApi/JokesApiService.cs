﻿using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlaygroundWorker.JokesApi;

internal sealed class JokesApiService
{
    private readonly HttpClient _client;

    public JokesApiService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<JokesApiResponse> GetRandomJoke(CancellationToken cancellationToken)
    {
        return await _client.GetFromJsonAsync<JokesApiResponse>("jokes/random", cancellationToken);
    }
}