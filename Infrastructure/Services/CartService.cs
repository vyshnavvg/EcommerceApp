using System;
using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class CartService : ICartService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _datebase;
    public CartService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _datebase = _redis.GetDatabase();
    }
    public async Task<bool> DeleteCartAsync(string key)
    {
        return await _datebase.KeyDeleteAsync(key);
    }

    public async Task<ShoppingCart?> GetCartAsync(string key)
    {
        var data = await _datebase.StringGetAsync(key);

        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data!); // ! is to override error
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
    {
        var created = await _datebase.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));

        if(!created) return null;

        return await GetCartAsync(cart.Id);
    }
}
