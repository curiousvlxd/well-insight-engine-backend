using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Dapper;

namespace WellInsightEngine.Infrastructure.Persistence;

public static class SqlColumnMapper
{
    public static void Register<T>()
    {
        SqlMapper.SetTypeMap(
            typeof(T),
            new CustomPropertyTypeMap(
                typeof(T),
                (type, columnName) =>
                    type.GetProperties()
                        .FirstOrDefault(p =>
                            p.GetCustomAttribute<ColumnAttribute>()?.Name == columnName
                        )!
            )
        );
    }
}