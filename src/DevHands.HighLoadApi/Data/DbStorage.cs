using System.Collections.Immutable;
using Dapper;
using DevHands.HighLoadApi.Containers;

namespace DevHands.HighLoadApi.Data;

public class DbStorage(IDapperContext dapperContext) : IDataStorage
{
    public async Task CreateItems(ImmutableList<DataItem> items)
    {
        await using var connection = dapperContext.Connect();
        await connection.ExecuteAsync("DELETE FROM [dh].[Items]");

        foreach (var item in items)
        {
            await connection.ExecuteAsync(
                @"insert into [dh].[Items] ([Id], [Name], [Description], [Completed], [Created]) 
values (@Id, @Name, @Description, @Completed, @Created)",
                param: item);
        }
    }

    public async Task<DataItem?> GetItem(long id)
    {
        await using var connection = dapperContext.Connect();
        return await connection.QueryFirstOrDefaultAsync<DataItem>(
                "select [Id], [Name], [Description], [Completed], [Created] from [dh].[Items] where [Id] = @Id",
                param: new { Id = id })
            .ConfigureAwait(false);
    }
}