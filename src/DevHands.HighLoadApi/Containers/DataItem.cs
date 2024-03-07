namespace DevHands.HighLoadApi.Containers;

public record DataItem(
    long Id,
    string Name,
    string? Description,
    bool Completed,
    DateTime Created
);