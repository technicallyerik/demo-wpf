Currency Conversion WPF Demo
======================

I created this WPF application as a simple demonstration of the capabilities and flow of WPF applications.

3rd party dependencies aren't included in source control. To get this to build, after opening it in Visual Studio, you will want to right-click on the solution and select 'Enable NuGet Package Restore'

Let's Talk Design Patterns
---------------------------

This application uses a MVP (model-view-presenter) design pattern.  Most other WPF developers will tell you this is "wrong" and to use a MVVM (model-view-viewmodel) design pattern.  The primary difference is that the "presenter" in the MVP pattern has a reference to the view, while the MVVM's "view model" does not.

**So what?**

WPF has a very powerful binding system to bind a model to the view.  Using the MVP pattern can lead to developers starting to reference the view from the presentation layer when there is a perfectly good way to accomplish that task using binding.  It can start to muddy up your code and blur the lines of separation.

**So why MVP?**

Binding isn't an end-all solution.  When you want to start opening dialog or confirmation boxes, for example, you have to create an all-binding solution which can become hack-ey, or create a messaging system which can become difficult to test or maintain.  And inevitably when your application grows in complexity, there are some things that you won't be able to accomplish using binding.

**Addressing MVP Concerns**

To address concerns of overuse and testing, the view is passed into the presenter layer via an interface.  This allows it to be mocked out during testing, and make the developer aware that directly manipulating the view might not be the way to go, as by default most of it will be unexposed by the interface.  

The Files
---------
