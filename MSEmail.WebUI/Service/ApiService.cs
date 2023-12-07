using MsEmail.Domain.Entities;

using MSEmail.WebUI.Models;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MSEmail.WebUI.Service;

public class ApiService
{
    private Uri baseUri = new("https://localhost:7146/api");

    private readonly HttpClient _client;

    public ApiService()
    {
        _client = new HttpClient();
        _client.BaseAddress = baseUri;
    }

    public void AddHeader(string key, string value)
    {
        _client.DefaultRequestHeaders.Add(key, value);
    }

    public void AddAuthorizationHeader(string value)
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", value);
    }


    public async Task<T> PostAsync<T, U>(string url, U data)
    {
        var _data = JsonSerializer.Serialize(data);
        StringContent content = new StringContent(
            _data,
            Encoding.UTF8,
            "application/json");

        using HttpResponseMessage response = _client.PostAsync(
                _client.BaseAddress + url,
                content)
                .Result;

        if (response.IsSuccessStatusCode)
            return  response.Content.ReadFromJsonAsync<T>().Result;

        throw new Exception(response.ReasonPhrase);
    }

    public HttpResponseMessage Get(string url)
    {
        return _client.GetAsync(
                _client.BaseAddress + url)
            .Result;
    }

    public async Task<T> GetAsync<T>(string url)
    {

        using (HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + url))
        {
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<T>(data);
            }

            throw new Exception(response.ReasonPhrase);
        }
    }
}