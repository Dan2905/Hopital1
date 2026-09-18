using System.Reflection;

var asm = Assembly.Load("Radzen.Blazor");

string[] wanted = { "DialogService", "ConfirmOptions", "DialogOptionsBase", "RadzenComponents", "RadzenDialog", "RadzenNotification", "RadzenTooltip", "RadzenContextMenu", "RadzenTemplateForm`1" };

foreach (var typeName in wanted)
{
    var type = asm.GetTypes().FirstOrDefault(t => t.Name == typeName || t.FullName == typeName);
    if (type is null)
    {
        Console.WriteLine($"### {typeName} : NOT FOUND");
        continue;
    }

    Console.WriteLine($"### {type.FullName} (base: {type.BaseType?.Name})");
    foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                        .Where(m => !m.IsSpecialName))
    {
        var g = m.IsGenericMethod ? "<" + string.Join(",", m.GetGenericArguments().Select(a => a.Name)) + ">" : string.Empty;
        var ps = string.Join(", ", m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}{(p.HasDefaultValue ? " = " + (p.DefaultValue ?? "null") : "")}"));
        Console.WriteLine($"  M: {m.ReturnType.Name} {m.Name}{g}({ps})");
    }
    foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
    {
        Console.WriteLine($"  P: {p.PropertyType.Name} {p.Name}");
    }
    Console.WriteLine();
}
