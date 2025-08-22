### Generate NUnit Tests

Generate NUnit test cases for the following C# code. Ensure that the tests cover various scenarios, including edge cases and typical use cases. Use NUnit attributes such as [Test], [SetUp], and [TearDown] where appropriate. Provide meaningful names for the test methods to indicate what each test is verifying.

## Substitutes Set Up

1. A controller name will be passed ass argument {{ arg.0 }}, from that controller name you should read the flow from top down & get all the  interface dependencies.
2. Once you have gotten all the dependencies injected create a new static class named `{{ arg.0 }}Substitutes.cs` int the NUnit test project folder name `Substitutes`, the class must initialise all the dependencies found using the latest version of NSubstitute package
3. An example of NSubsitute instatiation is below
```charp
   IEmailSender EmailSender = Substitute.For<IEmailSender>();
```
4.  Add all necessary nuget packages & project references.