# LINQ

## Sources

- [YT: Every Single LINQ Extension Method With Examples | .NET & C# Essentials (2:24)](https://www.youtube.com/watch?v=7-P6Mxl5elg)
- [Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/linq)

## Theory

### Deferred Execution vs. Immediate Execution

#### Deferred Execution

- query is **not** executed when it is defined
- execution happens when results are enumerated, e.g. `foreach()` or `ToList()`
- allows query results to reflect the current state of the data source at the moment of execution, not at declaration
- enables chaining and composition of multiple queries before execution
- for example `Where`, `Select`, `OrderBy`

Example:

```csharp
var query = numbers.Where(n => n > 3);        // Not executed yet
foreach(var n in query) { /* ... */ }         // Executed here
```

##### Advantages

- **Performance:** No computation or memory usage until results are actually needed.
- **Fresh Results:** Each execution reflects the latest state of the source data.
- **Query Composition:** You can build complex queries by chaining multiple operators before execution.
- **Lower Resource Use:** Avoids unnecessary calculations if results are never enumerated.

##### Disadvantages

- **Unpredictable State:** Results can change unexpectedly if the source collection changes between query definition and execution.
- **Multiple Enumerations:** Each enumeration re-executes the query, which can be inefficient for expensive operations or slow data sources.
- **Delayed Exceptions:** Errors aren’t caught until execution, which can make debugging harder.

##### When to Use

- When working with data that might change and you always need the latest information.
- When queries are composed dynamically, and you may or may not enumerate.
- For in-memory collections where evaluating results on demand is efficient.

##### What Can Go Wrong

- Source collection changes or is disposed before enumeration, leading to exceptions or invalid results.
- Re-evaluating a computationally expensive query multiple times unintentionally.
- Side effects in query logic producing inconsistent or undesired results during multiple enumerations.

#### Immediate Execution

- query is executed as soon as it is called
- results are materialized immediately
- useful for caching results, debugging, or when further query changes are not needed
- for example `ToList()`, `ToArray()`, `Count()`, or `First()`

Example:

```csharp
var resultList = numbers.Where(n => n > 3).ToList(); // Executed here
```

##### Advantages

- **Stable Results:** The results reflect the state of the source at the moment of execution.
- **Performance for Reuse:** Enumerate the data once and reuse the results multiple times with no re-execution cost.
- **Early Exception Detection:** Exceptions are thrown when the query is materialized, making debugging easier.
- **Thread Safety:** Safer for multi-threaded scenarios since results are not dependent on the ongoing state of the collection.

##### Disadvantages

- **Higher Memory Use:** Materializing large result sets into lists/arrays can be memory intensive.
- **Slower Start:** Immediate cost of computation even if only part of the data is needed.
- **Less Flexible:** Cannot reflect changes to the underlying data after the results are created.

##### When to Use

- When you need to cache, reuse, or repeatedly access the results.
- When the source might change unexpectedly during enumeration, or when querying external resources (like databases).
- For debugging, since exceptions and errors are surfaced right away.
- When you want to ensure repeatable, unchanging results.

##### What Can Go Wrong

- Unnecessary processing of large data sets consuming memory needlessly.
- Prematurely collecting results when only partial data was needed.
- Failing to realize that updates to the underlying data are not reflected in the collected results.

#### Comparison Table

Aspect              | Deferred Execution            | Immediate Execution
---                 | ---                           | ---
Memory Usage        | Low (until enumerated)        | High (materializes results)
Freshness           | Always up to date             | State at execution time
Query Complexity    | Can compose and chain         | Usually single, concrete result
Exception Timing    | On enumeration                | On materialization
Multiple Use        | Recomputed each time          | Cached/reused
Source Data Changes | Reflected in query results    | Not reflected after execution

#### Best Practices

- Use **deferred execution** when:
  - You want efficient, up-to-date results.
  - The source doesn’t change during enumeration, or it’s acceptable for it to change.
- Use **immediate execution** when:
  - The source may mutate, be disposed, or is from an external resource (database, file, API).
  - You need to cache results, or want exception safety and debugging clarity.

**Common Pitfall:** Not realizing that deferred queries can give different results over time, or incur performance penalties from repeated enumerations. Conversely, immediate queries can waste resources if a large set is materialized without need.

Understanding these trade-offs lets you choose the right execution type for each LINQ use case, helping avoid subtle bugs and performance traps.

### LINQ Query Syntax and Method Syntax

**Query Syntax**: Looks similar to SQL and is more readable for those familiar with SQL

``` csharp
from x in collection where x > 2 select x
```

**Method Syntax**: Uses extension methods and lambda expressions

``` csharp
collection.Where(x => x > 2).Select(x => x)
```

Both syntaxes compile to the same underlying code.

### Standard Query Operators

LINQ provides a set of methods to manipulate data:

- Filtering: Where
- Projection: Select
- Sorting: OrderBy, OrderByDescending
- Grouping: GroupBy
- **Aggregations**: Count, Sum, Average, Min, Max
- **Set Operations**: Distinct, Union, Except, Intersect

The choice of operator affects whether execution is deferred or immediate.

### Streaming vs. Non-Streaming Execution

Streaming operators (e.g., Where, Select) yield each result as soon as it’s available, without processing the entire data set first.

Non-Streaming operators (e.g., OrderBy, GroupBy) must process the full data set before yielding any results.

### LINQ Data Sources

LINQ can work with different data sources: in-memory objects (LINQ to Objects), databases (LINQ to SQL, LINQ to Entities), XML, and more.

The behavior of queries (especially around execution) can vary based on the data source.

### Expression Trees and Lambda Expressions

LINQ uses lambda expressions (like x => x > 3) extensively for query definitions.

Some LINQ providers (like LINQ to Entities) translate these expressions into other query languages (e.g., SQL).

### Side Effects and Re-Evaluations

Deferred queries re-evaluate the source each time they’re iterated; if the data source changes, results can change.

For stable, repeatable results, consider immediate execution or buffering results.

### Exception Timing and Resource Management

Exceptions in queries may not occur until the query is actually executed (iteration or materialization), not when it is declared.

Properly manage resources (like database connections) that the query might consume.

### Quick reference

Query/Operator Example      | Type of Execution
---                         | ---
Where, Select, OrderBy      | Deferred
ToList(), ToArray()         | Immediate
Count(), First(), Single()  | Immediate
GroupBy, Join               | Non-Streaming
Take(), Skip()              | Streaming
