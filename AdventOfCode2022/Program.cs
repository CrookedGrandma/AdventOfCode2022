using AdventOfCode2022;
using System.Reflection;

var type = Assembly.GetExecutingAssembly()
    .DefinedTypes
    .Where(t => t.BaseType!.Name == "Base")
    .Where(t => t.Name != "_Test")
    .OrderBy(t => int.Parse(t.Name[1..]))
    .Last();

var solver = (Base?)Activator.CreateInstance(type);
solver?.Run();
