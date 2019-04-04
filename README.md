# LiteDB-Wrapper
A simpler way to use [LiteDB](https://github.com/mbdavid/LiteDB)

# How-To

## [NuGet](https://www.nuget.org/packages/LiteDB.Wrapper/)

Add as reference
```c#
using LiteDB.Wrapper;
```

Create a collection reference
```c#
CollectionReference<YourModel> reference = new  CollectionReference<YourModel>("mydatabase.db", "my_collection");
```

Available methods
```c#
Insert(T obj)
Insert(IList<T> objList)
Update(T obj)
Update(IList<T> objList)
Remove(Guid id)
Remove(IList<Guid> idList)
```

Retrieving paged list
```c#
(IList<WrapperModel> list, long rows) = reference.GetPaged(new PageOptions(0, 10), new SortOptions(SortOptions.Order.DSC, "_id"));
```
*The **GetPaged** method returned a ```Tuple<IList<T>, long>```*

***Note*** : As per ``` LiteDB ``` specs, you must decorate your model classes with ```BsonField``` attributes.

# Contributors
- [kuromukira](https://www.twitter.com/norgelera)

Install the following to get started

**IDE**
1. [Visual Studio Code](https://code.visualstudio.com/) 
2. [Visual Studio Community](https://visualstudio.microsoft.com/downloads/)

**Exntesions**
1. [C# Language Extension for VSCode](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)

**Frameworks**
1. [.NET](https://www.microsoft.com/net/download)


Do you want to contribute? Send me an email or DM me in [twitter](https://www.twitter.com/norgelera).