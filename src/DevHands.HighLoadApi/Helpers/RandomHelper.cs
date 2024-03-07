using DevHands.HighLoadApi.Definitions;

namespace DevHands.HighLoadApi.Helpers;

public static class RandomHelper
{
    public static int GenerateItemId() => GenerateInRange(Defaults.DataItemsStartId, Defaults.DataItemsCount);

    public static int GenerateInRange(int start, int count) => Random.Shared.Next(start, start + count - 1);

    public static bool GenerateBool() => Random.Shared.Next() % 2 == 0;
}