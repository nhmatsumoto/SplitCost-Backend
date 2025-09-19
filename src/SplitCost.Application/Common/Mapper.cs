using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace SplitCost.Application.Common;

public static class Mapper
{
    private static readonly ConcurrentDictionary<string, Delegate> _mapCache = new();

    public static TDestination Map<TSource, TDestination>(TSource source)
        where TDestination : new()
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var key = $"{typeof(TSource).FullName}->{typeof(TDestination).FullName}";

        if (!_mapCache.TryGetValue(key, out var del))
        {
            del = CreateMapFunc<TSource, TDestination>();
            _mapCache[key] = del;
        }

        var func = (Func<TSource, TDestination>)del;
        return func(source);
    }

    private static Func<TSource, TDestination> CreateMapFunc<TSource, TDestination>()
        where TDestination : new()
    {
        var sourceParam = Expression.Parameter(typeof(TSource), "src");
        var bindings = new List<MemberBinding>();

        var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite);

        foreach (var destProp in destProperties)
        {
            var sourceProp = sourceProperties.FirstOrDefault(p => p.Name == destProp.Name && p.PropertyType == destProp.PropertyType);
            if (sourceProp != null)
            {
                var bind = Expression.Bind(destProp, Expression.Property(sourceParam, sourceProp));
                bindings.Add(bind);
            }
        }

        var body = Expression.MemberInit(Expression.New(typeof(TDestination)), bindings);
        var lambda = Expression.Lambda<Func<TSource, TDestination>>(body, sourceParam);
        return lambda.Compile();
    }

    public static IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> sourceList)
        where TDestination : new()
    {
        if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));
        var mapFunc = CreateMapFunc<TSource, TDestination>();
        return sourceList.Select(item => mapFunc(item)).ToList();
    }
}