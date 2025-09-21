using System.Reflection;

var assembly = Assembly.LoadFrom("C:\\Users\\Ouse\\Documents\\2d-demo\\.godot\\mono\\temp\\bin\\Debug\\2DDemo.dll");

if(assembly == null)
{
    Console.WriteLine("加载失败!");
    return;
}
Console.WriteLine("加载到了程序集:" + assembly?.FullName);

var types = assembly?.GetTypes();
if(types == null)
{
    Console.WriteLine("Type为null");
    return;
}

foreach (var type in types)
{
    // 检查字段
    foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    {
        if (typeof(Microsoft.Extensions.Logging.ILogger).IsAssignableFrom(field.FieldType) ||
            typeof(Microsoft.Extensions.Logging.ILoggerFactory).IsAssignableFrom(field.FieldType))
        {
            Console.WriteLine($"Logger field: {type.FullName}.{field.Name}");
        }
    }

    // 检查属性
    foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    {
        if (typeof(Microsoft.Extensions.Logging.ILogger).IsAssignableFrom(prop.PropertyType) ||
            typeof(Microsoft.Extensions.Logging.ILoggerFactory).IsAssignableFrom(prop.PropertyType))
        {
            Console.WriteLine($"Logger property: {type.FullName}.{prop.Name}");
        }
    }

    // 检查基类/接口
    if (typeof(Microsoft.Extensions.Logging.ILogger).IsAssignableFrom(type) ||
        typeof(Microsoft.Extensions.Logging.ILoggerFactory).IsAssignableFrom(type))
    {
        Console.WriteLine($"Logger-related type: {type.FullName}");
    }
}
Console.WriteLine("End");


