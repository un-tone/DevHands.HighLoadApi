using System.Collections.Immutable;
using DevHands.HighLoadApi.Containers;

namespace DevHands.HighLoadApi.Data;

public interface IDataStorage
{
    Task CreateItems(ImmutableList<DataItem> items);

    Task<DataItem?> GetItem(long id);
}