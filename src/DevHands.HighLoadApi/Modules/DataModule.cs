using System.Collections.Immutable;
using DevHands.HighLoadApi.Containers;
using DevHands.HighLoadApi.Definitions;
using DevHands.HighLoadApi.Helpers;

namespace DevHands.HighLoadApi.Modules;

public static class DataModule
{
    public static ImmutableList<DataItem> GenerateDataItems() =>
        Enumerable.Range(Defaults.DataItemsStartId, Defaults.DataItemsCount)
            .Select(x => (long)x)
            .Select(CreateDataItem)
            .ToImmutableList();

    public static DataItem CreateDataItem(long id) => new(
        Id: id,
        Name: $"Name {id}",
        Description: $"Description {id}",
        Completed: RandomHelper.GenerateBool(),
        Created: DateTime.UtcNow);
}