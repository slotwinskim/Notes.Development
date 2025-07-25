#region Filtering

IEnumerable<int> numbers = [1, 2, 3, 4, 5];
numbers.Where(n => n > 3).Dump();

IEnumerable<object> objects = [1, "two", 3.0, 4, "five"];
// it also converts objects to specified type
objects.OfType<int>().Dump();
objects.OfType<string>().Dump();

#endregion

#region Partitioning

#endregion