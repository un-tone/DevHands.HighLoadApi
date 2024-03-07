using System.Data.Common;

namespace DevHands.HighLoadApi.Data;

public interface IDapperContext
{
    DbConnection Connect();
}