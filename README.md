# LiteDB-Wrapper
A simpler way to use [LiteDB](https://github.com/mbdavid/LiteDB)

![Azure DevOps builds](https://img.shields.io/azure-devops/build/norgelera/277d6eba-8304-42f5-8471-77737cf8ec7f/8.svg)
![Nuget](https://img.shields.io/nuget/dt/LiteDB.Wrapper)
![Nuget](https://img.shields.io/nuget/v/LiteDB.Wrapper)

# How-To

## [NuGet](https://www.nuget.org/packages/LiteDB.Wrapper/)

Add as reference
```c#
using LiteDB.Wrapper;
using LiteDB.Wrapper.Interface;
```

Create a collection reference
```c#
ICollectionRef<YourModel> reference = new  CollectionReference<YourModel>("mydatabase.db", "my_collection");
```

Available methods
```c#
Insert(T)
Insert(IList<T>)
Update(T)
Update(IList<T>)
Remove(Guid)
Remove(IList<Guid>)
Commit()
Get(Guid)
GetPaged(PageOptions, SortOptions)
```

Here's an example usage
```c#
ICollectionRef<Model> reference = new CollectionReference<Model>(litedbloc, "insert_collection");
reference.Insert(new Model
{
	_ID = Guid.NewGuid(),
	Word = "Sample Word",
	Number = 99
});
await reference.Commit();
```
You have to invoke ```Commit()``` at the end to save your changes to ```LiteDB```.

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
