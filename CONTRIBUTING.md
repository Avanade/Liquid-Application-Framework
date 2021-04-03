# How to contribute

One of the easiest ways to contribute is to participate in discussions on GitHub issues. You can also contribute by submitting pull requests with code changes.

## General feedback and discussions?

Start a discussion on the [repository issue tracker](https://github.com/Avanade/Liquid-Application-Framework/issues).

## Bugs and feature requests?

For non-security related bugs, log a new issue in the GitHub repository.

## Reporting security issues and bugs

Security issues and bugs should be reported privately, via email, to a repository admin. You should receive a response within 24 hours.

## Contributing code and content

We accept fixes and features! Here are some resources to help you get started on how to contribute code or new content.

* ["Help wanted" issues](https://github.com/Avanade/Liquid-Application-Framework/labels/help%20wanted) - these issues are up for grabs. Comment on an issue if you want to create a fix.
* ["Good first issue" issues](https://github.com/Avanade/Liquid-Application-Framework/labels/good%20first%20issue) - we think these are a good for newcomers.

### Identifying the scale

If you would like to contribute to one of our repositories, first identify the scale of what you would like to contribute. If it is small (grammar/spelling or a bug fix) feel free to start working on a fix. If you are submitting a feature or substantial code contribution, please discuss it with the team and ensure it follows the product roadmap. You might also read these two blogs posts on contributing code: [Open Source Contribution Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html) by Miguel de Icaza and [Don't "Push" Your Pull Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/) by Ilya Grigorik. All code submissions will be rigorously reviewed and tested by the team, and only those that meet an extremely high bar for both quality and design/roadmap appropriateness will be merged into the source.

### Submitting a pull request

If you don't know what a pull request is read this article: https://help.github.com/articles/using-pull-requests. **Make sure the repository can build and all tests pass.** Familiarize yourself with the project workflow and our coding conventions. The coding, style, and general engineering guidelines are below on the topic [Engineering guidelines](#Engineering-guidelines).

### Feedback

Your pull request will now go through **extensive checks** by the subject matter experts on our team. Please be patient. Update your pull request according to feedback until it is approved by one of the ASP.NET team members. After that, one of our team members may adjust the branch you merge into based on the expected release schedule.

# Engineering Guidelines

## General

We try to hold our code to the higher standards. Every pull request must and will be scrutinized in order to maintain our standard. Every pull request should improve on quality, therefore any PR that decreases the quality will not be approved.

We follow coding best practices to make sure the codebase is clean and newcomers and seniors alike will understand the code. This document provides a guideline that should make most of our practices clear, but gray areas may arise and we might make a judgment call on your code, so, when in doubt, question ahead. Issues are a wonderful tool that should be used by every contributor to help us drive the project.

All the rules here are mandatory; however, we do not claim to hold all the answers - you can raise a question over any rule any time (through issues) and we'll discuss it. 

Finally, this is a new project and we are still learning how to work on a Open Source project. Please bear with us while we learn how to do it best.

## External dependencies

This refers to dependencies on projects (i.e. NuGet packages) outside of the repo. Every dependency we add enlarges the package, and might have copyright issues that we may need to address.

Therefore, adding or updating any external dependency requires approval. This can be discussed preferrabily on the issue related to the PR or on the PR itself.

## Code reviews and checkins

To help ensure that only the highest quality code makes its way into the project, please submit all your code changes to GitHub as PRs. This includes runtime code changes, unit test updates, and updates to official samples. For example, sending a PR for just an update to a unit test might seem like a waste of time but the unit tests are just as important as the product code and as such, reviewing changes to them is also just as important. This also helps create visibility for your changes so that others can observe what is going on.

The advantages are numerous: improving code quality, more visibility on changes and their potential impact, avoiding duplication of effort, and creating general awareness of progress being made in various areas.

To commit the PR to the repo either use preferably GitHub's "Squash and Merge" button on the main PR page, or do a typical push that you would use with Git (e.g. local pull, rebase, merge, push).

## Branching Strategy

We are using [Trunk-Based branching strategy](https://trunkbaseddevelopment.com/).

## Assembly naming pattern

The general naming pattern is `Liquid.<area>.<subarea>`.

## Unit tests

We use NUnit for all unit testing. Additionally, you can use NSubstitute for mocks and such, and leverage AutoFixture for anonymous instances.

## Code Style

### General

The most general guideline is that we use all the VS default settings in terms of code formatting, except that we put System namespaces before other namespaces (this used to be the default in VS, but it changed in a more recent version of VS). Also, we are leveraging StyleCop to add to code standardization. 

1. Use four spaces of indentation (no tabs)
1. Use `_camelCase` for private fields
1. Avoid `this`. unless absolutely necessary
1. Always specify member visibility, even if it's the default (i.e. `private string _foo;` not `string _foo;`)
1. Open-braces (`{`) go on a new line
1. Use any language features available to you (expression-bodied members, throw expressions, tuples, etc.) as long as they make for readable, manageable code.
This is pretty bad: `public (int, string) GetData(string filter) => (Data.Status, Data.GetWithFilter(filter ?? throw new ArgumentNullException(nameof(filter))));`

### Usage of the var keyword

The var keyword is to be used as much as the compiler will allow. For example, these are correct:

```csharp
var fruit = "Lychee";
var fruits = new List<Fruit>();
var flavor = fruit.GetFlavor();
string fruit = null; // can't use "var" because the type isn't known (though you could do (string)null, don't!)
const string expectedName = "name"; // can't use "var" with const
```

The following are incorrect:

```csharp
string fruit = "Lychee";
List<Fruit> fruits = new List<Fruit>();
FruitFlavor flavor = fruit.GetFlavor();
```

### Use C# type keywords in favor of .NET type names

When using a type that has a C# keyword the keyword is used in favor of the .NET type name. For example, these are correct:

```csharp
public string TrimString(string s) {
    return string.IsNullOrEmpty(s)
        ? null
        : s.Trim();
}


var intTypeName = nameof(Int32); // can't use C# type keywords with nameof
```

The following are incorrect:

```csharp
public String TrimString(String s) {
    return String.IsNullOrEmpty(s)
        ? null
        : s.Trim();
}
```

### Cross-platform coding

Our frameworks should work on CoreCLR, which supports multiple operating systems. Don't assume we only run (and develop) on Windows. Code should be sensitive to the differences between OS's. Here are some specifics to consider.

#### Line breaks

Windows uses \r\n, OS X and Linux uses \n. When it is important, use Environment.NewLine instead of hard-coding the line break.

Note: this may not always be possible or necessary.

Be aware that these line-endings may cause problems in code when using @"" text blocks with line breaks.

#### Environment Variables

OS's use different variable names to represent similar settings. Code should consider these differences.

For example, when looking for the user's home directory, on Windows the variable is USERPROFILE but on most Linux systems it is HOME.

```csharp
var homeDir = Environment.GetEnvironmentVariable("USERPROFILE") 
                  ?? Environment.GetEnvironmentVariable("HOME");
```

#### File path separators

Windows uses \ and OS X and Linux use / to separate directories. Instead of hard-coding either type of slash, use Path.Combine() or Path.DirectorySeparatorChar.

If this is not possible (such as in scripting), use a forward slash. Windows is more forgiving than Linux in this regard.

### When to use internals vs. public and when to use InternalsVisibleTo

As a modern set of frameworks, usage of internal types and members is allowed, but discouraged.

`InternalsVisibleTo` is used only to allow a unit test to test internal types and members of its runtime assembly. We do not use `InternalsVisibleTo` between two runtime assemblies.

If two runtime assemblies need to share common helpers then we will use a "shared source" solution with build-time only packages.

If two runtime assemblies need to call each other's APIs, the APIs must be public. If we need it, it is likely that our customers need it.

### Async method patterns

By default all async methods must have the Async suffix. There are some exceptional circumstances where a method name from a previous framework will be grandfathered in.

Passing cancellation tokens is done with an optional parameter with a value of `default(CancellationToken)`, which is equivalent to `CancellationToken.None` (one of the few places that we use optional parameters). The main exception to this is in web scenarios where there is already an `HttpContext` being passed around, in which case the context has its own cancellation token that can be used when needed.

Sample async method:

```csharp
public Task GetDataAsync(
    QueryParams query,
    int maxData,
    CancellationToken cancellationToken = default(CancellationToken))
{
    ...
}
```

### Extension method patterns

The general rule is: if a regular static method would suffice, avoid extension methods.

Extension methods are often useful to create chainable method calls, for example, when constructing complex objects, or creating queries.

Internal extension methods are allowed, but bear in mind the previous guideline: ask yourself if an extension method is truly the most appropriate pattern.

The namespace of the extension method class should generally be the namespace that represents the functionality of the extension method, as opposed to the namespace of the target type. One common exception to this is that the namespace for middleware extension methods is normally always the same is the namespace of IAppBuilder.

The class name of an extension method container (also known as a "sponsor type") should generally follow the pattern of `<Feature>Extensions`, `<Target><Feature>Extensions`, or `<Feature><Target>Extensions`. For example:

```csharp
namespace Food {
    class Fruit { ... }
}

namespace Fruit.Eating {
    class FruitExtensions { public static void Eat(this Fruit fruit); }
  OR
    class FruitEatingExtensions { public static void Eat(this Fruit fruit); }
  OR
    class EatingFruitExtensions { public static void Eat(this Fruit fruit); }
}
```

When writing extension methods for an interface the sponsor type name must not start with an I.

### Analyzers

Code style is usually enforced by Analyzers; any change to those rules must be discussed with the team before it's made. Also, any pull request that changes analyzers rules and commits code will be reproved immediately.

### Good practices

The items below point out the good practices that all code should follow.

#### Zero warnings

Compiler warnings should usually be dealt with by correcting the code. Only discussed warnings may be allowed to be marked as exceptions.

#### Inner documentation

All public members must be documented. Documentation should clarify the purpose and usage of code elements, so comments such as FooManager: "manages foo" will be rejected. Classes that implement interface may use comment inheritance `/// <inheritdoc/>`, but use it sparingly.

Try to use examples and such in classes to enable users to understand them more easily.

If you don't believe that a class or method deserves to be documents, ask yourself if it can be marked as non-public. 

If should comment every non-public class or member that is complex enough.

All comments should be read-proof.

> **Note**: Public means callable by a customer, so it includes protected APIs. However, some public APIs might still be "for internal use only" but need to be public for technical reasons. We will still have doc comments for these APIs but they will be documented as appropriate.

## Tests

- Tests need to be provided for every bug/feature that is completed.
- Tests only need to be present for issues that need to be verified by QA (for example, not tasks)
- Pull requests must strive to not reduce code coverage.
- If there is a scenario that is far too hard to test there does not need to be a test for it.
  - "Too hard" is determined by the team as a whole.

### Unit tests and functional tests

#### Assembly naming

The unit tests for the `Liquid.Fruit` assembly live in the `Liquid.Fruit.Tests` assembly.

The functional tests for the `Liquid.Fruit` assembly live in the `Liquid.Fruit.AcceptanceTests` assembly.

In general there should be exactly one unit test assembly for each product runtime assembly. In general there should be one functional test assembly per repo. Exceptions can be made for both.

#### Unit test class naming

Test class names end with Test and live in the same namespace as the class being tested. For example, the unit tests for the Liquid.Fruit.Banana class would be in a Liquid.Fruit.BananaTest class in the test assembly.

#### Unit test method naming

Unit test method names must be descriptive about what is being tested, under what conditions, and what the expectations are. Pascal casing and underscores can be used to improve readability. We will try to follow [Roy Osherove's guidelines](https://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html), therefore, the following test names are correct:
`GetAttachmentWhenItWasInsertedReturnsInsertedData`
`GetWhenAttachmentDoesntExistsThrows`

> Notice how we use When and Returns / Throws to split the name into recognizable parts.

The following test names are incorrect:

```csharp
Test1
Constructor
FormatString
GetData
```

#### Unit test structure

The contents of every unit test should be split into three distinct stages, optionally separated by these comments:

```csharp
// Arrange  
// Act  
// Assert 
```

The crucial thing here is that the Act stage is exactly one statement. That one statement is nothing more than a call to the one method that you are trying to test. Keeping that one statement as simple as possible is also very important. For example, this is not ideal:

```csharp
int result = myObj.CallSomeMethod(GetComplexParam1(), GetComplexParam2(), GetComplexParam3());
```

This style is not recommended because way too many things can go wrong in this one statement. All the `GetComplexParamN()` calls can throw for a variety of reasons unrelated to the test itself. It is thus unclear to someone running into a problem why the failure occurred.

The ideal pattern is to move the complex parameter building into the Arrange section:

```csharp
// Arrange
P1 p1 = GetComplexParam1();
P2 p2 = GetComplexParam2();
P3 p3 = GetComplexParam3();

// Act
int result = myObj.CallSomeMethod(p1, p2, p3);

// Assert
Assert.AreEqual(1234, result);
```

Now the only reason the line with `CallSomeMethod()` can fail is if the method itself blew up. This is especially important when you're using helpers such as ExceptionHelper, where the delegate you pass into it must fail for exactly one reason.

#### Use NUnit's plethora of built-in assertions

NUnit includes many kinds of assertions – please use the most appropriate one for your test. This will make the tests a lot more readable and also allow the test runner report the best possible errors (whether it's local or the CI machine). For example, these are bad:

```csharp
Assert.AreEqual(true, someBool);

Assert.True("abc123" == someString);

Assert.True(list1.Length == list2.Length);

for (int i = 0; i < list1.Length; i++) {
    Assert.True(
        String.Equals
            list1[i],
            list2[i],
            StringComparison.OrdinalIgnoreCase));
}
```

These are good:

```csharp
Assert.True(someBool);

Assert.AreEqual("abc123", someString);

// built-in collection assertions!
Assert.AreEqual(list1, list2, StringComparer.OrdinalIgnoreCase);
```

It's also possible (and recommended) to use the new ContraintModel of NUnit:

```csharp
Assert.That(condition, Is.True);

int[] array = new int[] { 1, 2, 3 };
Assert.That(array, Has.Exactly(1).EqualTo(3));
Assert.That(array, Has.Exactly(2).GreaterThan(1));
Assert.That(array, Has.Exactly(3).LessThan(100));
```

#### Parallel tests

By default all unit test assemblies should run in parallel mode, which is the default. Unit tests shouldn't depend on any shared state, and so should generally be runnable in parallel. If the tests fail in parallel, the first thing to do is to figure out why; do not just disable parallel tests!

For functional tests it is reasonable to disable parallel tests.

### Use only complete words or common/standard abbreviations in public APIs

Public namespaces, type names, member names, and parameter names must use complete words or common/standard abbreviations.

These are correct:

```csharp
public void AddReference(AssemblyReference reference);
public EcmaScriptObject SomeObject { get; }
```

These are incorrect:

```csharp
public void AddRef(AssemblyReference ref);
public EcmaScriptObject SomeObj { get; }
```

Note that abbreviations still follow camel/pascal casing. Use `Api` instead of `API` and `Ecma` instead of `ECMA`, to make your code more legible.

Avoid abbreviations with only two characters because of the rule above.
