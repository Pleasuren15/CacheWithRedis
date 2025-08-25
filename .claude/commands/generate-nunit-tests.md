### Generate NUnit Tests

Generate NUnit test cases for the following C# code. Ensure that the tests cover various scenarios, including edge cases and typical use cases. Use NUnit attributes such as [Test], [SetUp], and [TearDown] where appropriate. Provide meaningful names for the test methods to indicate what each test is verifying.

## Substitutes Set Up

1. A controller name will be passed ass argument {{ arg.0 }}, from that controller name you should read the flow from top down & get all the  interface dependencies.
2. Once you have gotten all the dependencies injected, only create a new class named `{{ arg.0 }}Substitutes.cs` int the NUnit test project folder name `Substitutes`, the class must initialise all the dependencies found using the latest version of NSubstitute package
3. An example of NSubsitute instatiation is below
```charp
   IEmailSender EmailSender = Substitute.For<IEmailSender>();
```
4. This must be recursive in a way that you dont only subsitute controller's interface, but drill down ot the child classes that all get hit during the controller execution.
5. Add all necessary nuget packages & project references. Remove all unnecessary methods you might've added & auto-implemented property syntax.

## Builder Set Up

1. For this step you are asked to use the builder pattern to create all the need components.
2. For all the dependencies that were created in the project, create build methods that return an instantiated object. Objects must initialised using the interfaces created in the `Substitutes Set Up`
3. For all the builder methods we must be able to pass in our object from the substitute.
4. No builders should be created for nuget packages & controllers.
5. Builders must be created in the NUnit test project folder named `Builders`

## Combining Substitutes & Builder Into System Under Test

1. Now you are going to create a system under test class for the controller, class will be named `{{ arg.0 }}Sut.cs`, Sut stands for `SystemUnderTest`
2. The class will have a static method called `CreateSystemUnderTest` that will take in the `{{ arg.0 }}Substitutes.cs` object as a parameter, the values of this parameter will be pass into the builders.
3. Inside the method we will bo overrding all the possible `{{ arg.0 }}Substitutes.cs` objects values using the builders we created in the `Builder Set Up` step.
4. The method should have a return type of `{{ arg.0 }}` controller.
5. At the end of the method instantiate the controller using the builder method added & return the controller.