﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.TestCommon;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Testing;
using Moq;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    public class PageTest
    {
        [Fact]
        public void PagePropertiesArePopulatedFromContext()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewDataDictionary = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = Mock.Of<ITempDataDictionary>();
            var pageContext = new PageContext(actionContext, viewDataDictionary, tempData, new HtmlHelperOptions());

            var page = new TestPage
            {
                PageContext = pageContext,
            };

            // Act & Assert
            Assert.Same(pageContext, page.ViewContext);
            Assert.Same(httpContext, page.HttpContext);
            Assert.Same(httpContext.Request, page.Request);
            Assert.Same(httpContext.Response, page.Response);
            Assert.Same(modelState, page.ModelState);
            Assert.Same(tempData, page.TempData);
        }

        [Fact]
        public void Redirect_WithParameterUrl_SetsRedirectResultSameUrl()
        {
            // Arrange
            var controller = new TestPage();
            var url = "/test/url";

            // Act
            var result = controller.Redirect(url);

            // Assert
            Assert.IsType<RedirectResult>(result);
            Assert.False(result.PreserveMethod);
            Assert.False(result.Permanent);
            Assert.Same(url, result.Url);
        }

        [Fact]
        public void RedirectPermanent_WithParameterUrl_SetsRedirectResultPermanentAndSameUrl()
        {
            // Arrange
            var controller = new TestPage();
            var url = "/test/url";

            // Act
            var result = controller.RedirectPermanent(url);

            // Assert
            Assert.IsType<RedirectResult>(result);
            Assert.False(result.PreserveMethod);
            Assert.True(result.Permanent);
            Assert.Same(url, result.Url);
        }

        [Fact]
        public void RedirectPermanent_WithParameterUrl_SetsRedirectResultPreserveMethodAndSameUrl()
        {
            // Arrange
            var controller = new TestPage();
            var url = "/test/url";

            // Act
            var result = controller.RedirectPreserveMethod(url);

            // Assert
            Assert.IsType<RedirectResult>(result);
            Assert.True(result.PreserveMethod);
            Assert.False(result.Permanent);
            Assert.Same(url, result.Url);
        }

        [Fact]
        public void RedirectPermanent_WithParameterUrl_SetsRedirectResultPermanentPreserveMethodAndSameUrl()
        {
            // Arrange
            var controller = new TestPage();
            var url = "/test/url";

            // Act
            var result = controller.RedirectPermanentPreserveMethod(url);

            // Assert
            Assert.IsType<RedirectResult>(result);
            Assert.True(result.PreserveMethod);
            Assert.True(result.Permanent);
            Assert.Same(url, result.Url);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Redirect_WithParameter_NullOrEmptyUrl_Throws(string url)
        {
            // Arrange
            var controller = new TestPage();

            // Act & Assert
            ExceptionAssert.ThrowsArgumentNullOrEmpty(
                () => controller.Redirect(url: url), "url");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RedirectPreserveMethod_WithParameter_NullOrEmptyUrl_Throws(string url)
        {
            // Arrange
            var controller = new TestPage();

            // Act & Assert
            ExceptionAssert.ThrowsArgumentNullOrEmpty(
                () => controller.RedirectPreserveMethod(url: url), "url");
        }

        [Fact]
        public void LocalRedirect_WithParameterUrl_SetsLocalRedirectResultWithSameUrl()
        {
            // Arrange
            var controller = new TestPage();
            var url = "/test/url";

            // Act
            var result = controller.LocalRedirect(url);

            // Assert
            Assert.IsType<LocalRedirectResult>(result);
            Assert.False(result.PreserveMethod);
            Assert.False(result.Permanent);
            Assert.Same(url, result.Url);
        }

        [Fact]
        public void LocalRedirectPermanent_WithParameterUrl_SetsLocalRedirectResultPermanentWithSameUrl()
        {
            // Arrange
            var controller = new TestPage();
            var url = "/test/url";

            // Act
            var result = controller.LocalRedirectPermanent(url);

            // Assert
            Assert.IsType<LocalRedirectResult>(result);
            Assert.False(result.PreserveMethod);
            Assert.True(result.Permanent);
            Assert.Same(url, result.Url);
        }

        [Fact]
        public void LocalRedirectPermanent_WithParameterUrl_SetsLocalRedirectResultPreserveMethodWithSameUrl()
        {
            // Arrange
            var controller = new TestPage();
            var url = "/test/url";

            // Act
            var result = controller.LocalRedirectPreserveMethod(url);

            // Assert
            Assert.IsType<LocalRedirectResult>(result);
            Assert.True(result.PreserveMethod);
            Assert.False(result.Permanent);
            Assert.Same(url, result.Url);
        }

        [Fact]
        public void LocalRedirectPermanent_WithParameterUrl_SetsLocalRedirectResultPermanentPreservesMethodWithSameUrl()
        {
            // Arrange
            var controller = new TestPage();
            var url = "/test/url";

            // Act
            var result = controller.LocalRedirectPermanentPreserveMethod(url);

            // Assert
            Assert.IsType<LocalRedirectResult>(result);
            Assert.True(result.PreserveMethod);
            Assert.True(result.Permanent);
            Assert.Same(url, result.Url);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void LocalRedirect_WithParameter_NullOrEmptyUrl_Throws(string url)
        {
            // Arrange
            var controller = new TestPage();

            // Act & Assert
            ExceptionAssert.ThrowsArgumentNullOrEmpty(
                () => controller.LocalRedirect(localUrl: url), "localUrl");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void LocalRedirectPreserveMethod_WithParameter_NullOrEmptyUrl_Throws(string url)
        {
            // Arrange
            var controller = new TestPage();

            // Act & Assert
            ExceptionAssert.ThrowsArgumentNullOrEmpty(
                () => controller.LocalRedirectPreserveMethod(localUrl: url), "localUrl");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void LocalRedirectPermanentPreserveMethod_WithParameter_NullOrEmptyUrl_Throws(string url)
        {
            // Arrange
            var controller = new TestPage();

            // Act & Assert
            ExceptionAssert.ThrowsArgumentNullOrEmpty(
                () => controller.LocalRedirectPermanentPreserveMethod(localUrl: url), "localUrl");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RedirectPermanent_WithParameter_NullOrEmptyUrl_Throws(string url)
        {
            // Arrange
            var controller = new TestPage();

            // Act & Assert
            ExceptionAssert.ThrowsArgumentNullOrEmpty(
                () => controller.RedirectPermanent(url: url), "url");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RedirectPermanentPreserveMethod_WithParameter_NullOrEmptyUrl_Throws(string url)
        {
            // Arrange
            var controller = new TestPage();

            // Act & Assert
            ExceptionAssert.ThrowsArgumentNullOrEmpty(
                () => controller.RedirectPermanentPreserveMethod(url: url), "url");
        }

        [Fact]
        public void RedirectToAction_WithParameterActionName_SetsResultActionName()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToAction("SampleAction");

            // Assert
            Assert.IsType<RedirectToActionResult>(resultTemporary);
            Assert.False(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Equal("SampleAction", resultTemporary.ActionName);
        }

        [Fact]
        public void RedirectToActionPreserveMethod_WithParameterActionName_SetsResultActionName()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToActionPreserveMethod(actionName: "SampleAction");

            // Assert
            Assert.IsType<RedirectToActionResult>(resultTemporary);
            Assert.True(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Equal("SampleAction", resultTemporary.ActionName);
        }

        [Fact]
        public void RedirectToActionPermanent_WithParameterActionName_SetsResultActionNameAndPermanent()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToActionPermanent("SampleAction");

            // Assert
            Assert.IsType<RedirectToActionResult>(resultPermanent);
            Assert.False(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Equal("SampleAction", resultPermanent.ActionName);
        }

        [Fact]
        public void RedirectToActionPermanentPreserveMethod_WithParameterActionName_SetsResultActionNameAndPermanent()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToActionPermanentPreserveMethod(actionName: "SampleAction");

            // Assert
            Assert.IsType<RedirectToActionResult>(resultPermanent);
            Assert.True(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Equal("SampleAction", resultPermanent.ActionName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("SampleController")]
        public void RedirectToAction_WithParameterActionAndControllerName_SetsEqualNames(string controllerName)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToAction("SampleAction", controllerName);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultTemporary);
            Assert.False(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Equal("SampleAction", resultTemporary.ActionName);
            Assert.Equal(controllerName, resultTemporary.ControllerName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("SampleController")]
        public void RedirectToActionPreserveMethod_WithParameterActionAndControllerName_SetsEqualNames(string controllerName)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToActionPreserveMethod(actionName: "SampleAction", controllerName: controllerName);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultTemporary);
            Assert.True(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Equal("SampleAction", resultTemporary.ActionName);
            Assert.Equal(controllerName, resultTemporary.ControllerName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("SampleController")]
        public void RedirectToActionPermanent_WithParameterActionAndControllerName_SetsEqualNames(string controllerName)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToActionPermanent("SampleAction", controllerName);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultPermanent);
            Assert.False(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Equal("SampleAction", resultPermanent.ActionName);
            Assert.Equal(controllerName, resultPermanent.ControllerName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("SampleController")]
        public void RedirectToActionPermanentPreserveMethod_WithParameterActionAndControllerName_SetsEqualNames(string controllerName)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToActionPermanentPreserveMethod(actionName: "SampleAction", controllerName: controllerName);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultPermanent);
            Assert.True(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Equal("SampleAction", resultPermanent.ActionName);
            Assert.Equal(controllerName, resultPermanent.ControllerName);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToAction_WithParameterActionControllerRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToAction("SampleAction", "SampleController", routeValues);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultTemporary);
            Assert.False(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Equal("SampleAction", resultTemporary.ActionName);
            Assert.Equal("SampleController", resultTemporary.ControllerName);
            Assert.Equal(expected, resultTemporary.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToActionPreserveMethod_WithParameterActionControllerRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToActionPreserveMethod(
                actionName: "SampleAction",
                controllerName: "SampleController",
                routeValues: routeValues);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultTemporary);
            Assert.True(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Equal("SampleAction", resultTemporary.ActionName);
            Assert.Equal("SampleController", resultTemporary.ControllerName);
            Assert.Equal(expected, resultTemporary.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToActionPermanent_WithParameterActionControllerRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToActionPermanent(
                "SampleAction",
                "SampleController",
                routeValues);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultPermanent);
            Assert.False(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Equal("SampleAction", resultPermanent.ActionName);
            Assert.Equal("SampleController", resultPermanent.ControllerName);
            Assert.Equal(expected, resultPermanent.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToActionPermanentPreserveMethod_WithParameterActionControllerRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToActionPermanentPreserveMethod(
                actionName: "SampleAction",
                controllerName: "SampleController",
                routeValues: routeValues);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultPermanent);
            Assert.True(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Equal("SampleAction", resultPermanent.ActionName);
            Assert.Equal("SampleController", resultPermanent.ControllerName);
            Assert.Equal(expected, resultPermanent.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToAction_WithParameterActionAndRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToAction(actionName: null, routeValues: routeValues);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultTemporary);
            Assert.False(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Null(resultTemporary.ActionName);
            Assert.Equal(expected, resultTemporary.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToActionPreserveMethod_WithParameterActionAndRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToActionPreserveMethod(actionName: null, routeValues: routeValues);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultTemporary);
            Assert.True(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Null(resultTemporary.ActionName);
            Assert.Equal(expected, resultTemporary.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToAction_WithParameterActionAndControllerAndRouteValuesAndFragment_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expectedRouteValues)
        {
            // Arrange
            var controller = new TestPage();
            var expectedAction = "Action";
            var expectedController = "Home";
            var expectedFragment = "test";

            // Act
            var result = controller.RedirectToAction("Action", "Home", routeValues, "test");

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.False(result.PreserveMethod);
            Assert.False(result.Permanent);
            Assert.Equal(expectedAction, result.ActionName);
            Assert.Equal(expectedRouteValues, result.RouteValues);
            Assert.Equal(expectedController, result.ControllerName);
            Assert.Equal(expectedFragment, result.Fragment);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToActionPreserveMethod_WithParameterActionAndControllerAndRouteValuesAndFragment_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expectedRouteValues)
        {
            // Arrange
            var controller = new TestPage();
            var expectedAction = "Action";
            var expectedController = "Home";
            var expectedFragment = "test";

            // Act
            var result = controller.RedirectToActionPreserveMethod("Action", "Home", routeValues, "test");

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(result.PreserveMethod);
            Assert.False(result.Permanent);
            Assert.Equal(expectedAction, result.ActionName);
            Assert.Equal(expectedRouteValues, result.RouteValues);
            Assert.Equal(expectedController, result.ControllerName);
            Assert.Equal(expectedFragment, result.Fragment);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToActionPermanent_WithParameterActionAndRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToActionPermanent(null, routeValues);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultPermanent);
            Assert.False(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Null(resultPermanent.ActionName);
            Assert.Equal(expected, resultPermanent.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToActionPermanentPreserveMethod_WithParameterActionAndRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToActionPermanentPreserveMethod(actionName: null, routeValues: routeValues);

            // Assert
            Assert.IsType<RedirectToActionResult>(resultPermanent);
            Assert.True(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Null(resultPermanent.ActionName);
            Assert.Equal(expected, resultPermanent.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToActionPermanent_WithParameterActionAndControllerAndRouteValuesAndFragment_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expectedRouteValues)
        {
            // Arrange
            var controller = new TestPage();
            var expectedAction = "Action";
            var expectedController = "Home";
            var expectedFragment = "test";

            // Act
            var result = controller.RedirectToActionPermanent("Action", "Home", routeValues, fragment: "test");

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.False(result.PreserveMethod);
            Assert.True(result.Permanent);
            Assert.Equal(expectedAction, result.ActionName);
            Assert.Equal(expectedRouteValues, result.RouteValues);
            Assert.Equal(expectedController, result.ControllerName);
            Assert.Equal(expectedFragment, result.Fragment);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToActionPermanentPreserveMethod_WithParameterActionAndControllerAndRouteValuesAndFragment_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expectedRouteValues)
        {
            // Arrange
            var controller = new TestPage();
            var expectedAction = "Action";
            var expectedController = "Home";
            var expectedFragment = "test";

            // Act
            var result = controller.RedirectToActionPermanentPreserveMethod(
                actionName: "Action",
                controllerName: "Home",
                routeValues: routeValues,
                fragment: "test");

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(result.PreserveMethod);
            Assert.True(result.Permanent);
            Assert.Equal(expectedAction, result.ActionName);
            Assert.Equal(expectedRouteValues, result.RouteValues);
            Assert.Equal(expectedController, result.ControllerName);
            Assert.Equal(expectedFragment, result.Fragment);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoute_WithParameterRouteValues_SetsResultEqualRouteValues(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToRoute(routeValues);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultTemporary);
            Assert.False(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Equal(expected, resultTemporary.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoutePreserveMethod_WithParameterRouteValues_SetsResultEqualRouteValues(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultTemporary = controller.RedirectToRoutePreserveMethod(routeValues: routeValues);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultTemporary);
            Assert.True(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Equal(expected, resultTemporary.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoute_WithParameterRouteNameAndRouteValuesAndFragment_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expectedRouteValues)
        {
            // Arrange
            var controller = new TestPage();
            var expectedRoute = "TestRoute";
            var expectedFragment = "test";

            // Act
            var result = controller.RedirectToRoute("TestRoute", routeValues, "test");

            // Assert
            Assert.IsType<RedirectToRouteResult>(result);
            Assert.False(result.PreserveMethod);
            Assert.False(result.Permanent);
            Assert.Equal(expectedRoute, result.RouteName);
            Assert.Equal(expectedRouteValues, result.RouteValues);
            Assert.Equal(expectedFragment, result.Fragment);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoutePreserveMethod_WithParameterRouteNameAndRouteValuesAndFragment_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expectedRouteValues)
        {
            // Arrange
            var controller = new TestPage();
            var expectedRoute = "TestRoute";
            var expectedFragment = "test";

            // Act
            var result = controller.RedirectToRoutePreserveMethod(routeName: "TestRoute", routeValues: routeValues, fragment: "test");

            // Assert
            Assert.IsType<RedirectToRouteResult>(result);
            Assert.True(result.PreserveMethod);
            Assert.False(result.Permanent);
            Assert.Equal(expectedRoute, result.RouteName);
            Assert.Equal(expectedRouteValues, result.RouteValues);
            Assert.Equal(expectedFragment, result.Fragment);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoutePermanent_WithParameterRouteValues_SetsResultEqualRouteValuesAndPermanent(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToRoutePermanent(routeValues);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultPermanent);
            Assert.False(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Equal(expected, resultPermanent.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoutePermanentPreserveMethod_WithParameterRouteValues_SetsResultEqualRouteValuesAndPermanent(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var resultPermanent = controller.RedirectToRoutePermanentPreserveMethod(routeValues: routeValues);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultPermanent);
            Assert.True(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Equal(expected, resultPermanent.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoutePermanent_WithParameterRouteNameAndRouteValuesAndFragment_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expectedRouteValues)
        {
            // Arrange
            var controller = new TestPage();
            var expectedRoute = "TestRoute";
            var expectedFragment = "test";

            // Act
            var result = controller.RedirectToRoutePermanent("TestRoute", routeValues, "test");

            // Assert
            Assert.IsType<RedirectToRouteResult>(result);
            Assert.False(result.PreserveMethod);
            Assert.True(result.Permanent);
            Assert.Equal(expectedRoute, result.RouteName);
            Assert.Equal(expectedRouteValues, result.RouteValues);
            Assert.Equal(expectedFragment, result.Fragment);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoutePermanentPreserveMethod_WithParameterRouteNameAndRouteValuesAndFragment_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expectedRouteValues)
        {
            // Arrange
            var controller = new TestPage();
            var expectedRoute = "TestRoute";
            var expectedFragment = "test";

            // Act
            var result = controller.RedirectToRoutePermanentPreserveMethod(routeName: "TestRoute", routeValues: routeValues, fragment: "test");

            // Assert
            Assert.IsType<RedirectToRouteResult>(result);
            Assert.True(result.PreserveMethod);
            Assert.True(result.Permanent);
            Assert.Equal(expectedRoute, result.RouteName);
            Assert.Equal(expectedRouteValues, result.RouteValues);
            Assert.Equal(expectedFragment, result.Fragment);
        }

        [Fact]
        public void RedirectToRoute_WithParameterRouteName_SetsResultSameRouteName()
        {
            // Arrange
            var controller = new TestPage();
            var routeName = "CustomRouteName";

            // Act
            var resultTemporary = controller.RedirectToRoute(routeName);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultTemporary);
            Assert.False(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Same(routeName, resultTemporary.RouteName);
        }

        [Fact]
        public void RedirectToRoutePreserveMethod_WithParameterRouteName_SetsResultSameRouteName()
        {
            // Arrange
            var controller = new TestPage();
            var routeName = "CustomRouteName";

            // Act;
            var resultTemporary = controller.RedirectToRoutePreserveMethod(routeName: routeName);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultTemporary);
            Assert.True(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Same(routeName, resultTemporary.RouteName);
        }

        [Fact]
        public void RedirectToRoutePermanent_WithParameterRouteName_SetsResultSameRouteNameAndPermanent()
        {
            // Arrange
            var controller = new TestPage();
            var routeName = "CustomRouteName";

            // Act
            var resultPermanent = controller.RedirectToRoutePermanent(routeName);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultPermanent);
            Assert.False(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Same(routeName, resultPermanent.RouteName);
        }

        [Fact]
        public void RedirectToRoutePermanentPreserveMethod_WithParameterRouteName_SetsResultSameRouteNameAndPermanent()
        {
            // Arrange
            var controller = new TestPage();
            var routeName = "CustomRouteName";

            // Act
            var resultPermanent = controller.RedirectToRoutePermanentPreserveMethod(routeName: routeName);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultPermanent);
            Assert.True(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Same(routeName, resultPermanent.RouteName);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoute_WithParameterRouteNameAndRouteValues_SetsResultSameRouteNameAndRouteValues(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();
            var routeName = "CustomRouteName";

            // Act
            var resultTemporary = controller.RedirectToRoute(routeName, routeValues);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultTemporary);
            Assert.False(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Same(routeName, resultTemporary.RouteName);
            Assert.Equal(expected, resultTemporary.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoutePreserveMethod_WithParameterRouteNameAndRouteValues_SetsResultSameRouteNameAndRouteValues(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();
            var routeName = "CustomRouteName";

            // Act
            var resultTemporary = controller.RedirectToRoutePreserveMethod(routeName: routeName, routeValues: routeValues);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultTemporary);
            Assert.True(resultTemporary.PreserveMethod);
            Assert.False(resultTemporary.Permanent);
            Assert.Same(routeName, resultTemporary.RouteName);
            Assert.Equal(expected, resultTemporary.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoutePermanent_WithParameterRouteNameAndRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();
            var routeName = "CustomRouteName";

            // Act
            var resultPermanent = controller.RedirectToRoutePermanent(routeName, routeValues);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultPermanent);
            Assert.False(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Same(routeName, resultPermanent.RouteName);
            Assert.Equal(expected, resultPermanent.RouteValues);
        }

        [Theory]
        [MemberData(nameof(RedirectTestData))]
        public void RedirectToRoutePermanentPreserveMethod_WithParameterRouteNameAndRouteValues_SetsResultProperties(
            object routeValues,
            IEnumerable<KeyValuePair<string, object>> expected)
        {
            // Arrange
            var controller = new TestPage();
            var routeName = "CustomRouteName";

            // Act
            var resultPermanent = controller.RedirectToRoutePermanentPreserveMethod(routeName: routeName, routeValues: routeValues);

            // Assert
            Assert.IsType<RedirectToRouteResult>(resultPermanent);
            Assert.True(resultPermanent.PreserveMethod);
            Assert.True(resultPermanent.Permanent);
            Assert.Same(routeName, resultPermanent.RouteName);
            Assert.Equal(expected, resultPermanent.RouteValues);
        }

        [Fact]
        public void File_WithContents()
        {
            // Arrange
            var controller = new TestPage();
            var fileContents = new byte[0];

            // Act
            var result = controller.File(fileContents, "application/pdf");

            // Assert
            Assert.NotNull(result);
            Assert.Same(fileContents, result.FileContents);
            Assert.Equal("application/pdf", result.ContentType.ToString());
            Assert.Equal(string.Empty, result.FileDownloadName);
        }

        [Fact]
        public void File_WithContentsAndFileDownloadName()
        {
            // Arrange
            var controller = new TestPage();
            var fileContents = new byte[0];

            // Act
            var result = controller.File(fileContents, "application/pdf", "someDownloadName");

            // Assert
            Assert.NotNull(result);
            Assert.Same(fileContents, result.FileContents);
            Assert.Equal("application/pdf", result.ContentType.ToString());
            Assert.Equal("someDownloadName", result.FileDownloadName);
        }

        [Fact]
        public void File_WithPath()
        {
            // Arrange
            var controller = new TestPage();
            var path = Path.GetFullPath("somepath");

            // Act
            var result = controller.File(path, "application/pdf");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(path, result.FileName);
            Assert.Equal("application/pdf", result.ContentType.ToString());
            Assert.Equal(string.Empty, result.FileDownloadName);
        }

        [Fact]
        public void File_WithPathAndFileDownloadName()
        {
            // Arrange
            var controller = new TestPage();
            var path = Path.GetFullPath("somepath");

            // Act
            var result = controller.File(path, "application/pdf", "someDownloadName");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(path, result.FileName);
            Assert.Equal("application/pdf", result.ContentType.ToString());
            Assert.Equal("someDownloadName", result.FileDownloadName);
        }

        [Fact]
        public void File_WithStream()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Response.RegisterForDispose(It.IsAny<IDisposable>()));

            var controller = new TestPage()
            {
                PageContext = new PageContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var fileStream = Stream.Null;

            // Act
            var result = controller.File(fileStream, "application/pdf");

            // Assert
            Assert.NotNull(result);
            Assert.Same(fileStream, result.FileStream);
            Assert.Equal("application/pdf", result.ContentType.ToString());
            Assert.Equal(string.Empty, result.FileDownloadName);
        }

        [Fact]
        public void File_WithStreamAndFileDownloadName()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();

            var controller = new TestPage()
            {
                PageContext = new PageContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var fileStream = Stream.Null;

            // Act
            var result = controller.File(fileStream, "application/pdf", "someDownloadName");

            // Assert
            Assert.NotNull(result);
            Assert.Same(fileStream, result.FileStream);
            Assert.Equal("application/pdf", result.ContentType.ToString());
            Assert.Equal("someDownloadName", result.FileDownloadName);
        }

        [Fact]
        public void Unauthorized_SetsStatusCode()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var result = controller.Unauthorized();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        }

        [Fact]
        public void NotFound_SetsStatusCode()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var result = controller.NotFound();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public void NotFound_SetsStatusCodeAndResponseContent()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var result = controller.NotFound("Test Content");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Equal("Test Content", result.Value);
        }

        [Fact]
        public void Content_WithParameterContentString_SetsResultContent()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var actualContentResult = controller.Content("TestContent");

            // Assert
            Assert.IsType<ContentResult>(actualContentResult);
            Assert.Equal("TestContent", actualContentResult.Content);
            Assert.Null(actualContentResult.ContentType);
        }

        [Fact]
        public void Content_WithParameterContentStringAndContentType_SetsResultContentAndContentType()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var actualContentResult = controller.Content("TestContent", "text/plain");

            // Assert
            Assert.IsType<ContentResult>(actualContentResult);
            Assert.Equal("TestContent", actualContentResult.Content);
            Assert.Null(MediaType.GetEncoding(actualContentResult.ContentType));
            Assert.Equal("text/plain", actualContentResult.ContentType.ToString());
        }

        [Fact]
        public void Content_WithParameterContentAndTypeAndEncoding_SetsResultContentAndTypeAndEncoding()
        {
            // Arrange
            var controller = new TestPage();

            // Act
            var actualContentResult = controller.Content("TestContent", "text/plain", Encoding.UTF8);

            // Assert
            Assert.IsType<ContentResult>(actualContentResult);
            Assert.Equal("TestContent", actualContentResult.Content);
            Assert.Same(Encoding.UTF8, MediaType.GetEncoding(actualContentResult.ContentType));
            Assert.Equal("text/plain; charset=utf-8", actualContentResult.ContentType.ToString());
        }

        [Fact]
        public void Content_NoContentType_DefaultEncodingIsUsed()
        {
            // Arrange
            var contentController = new ContentPage();

            // Act
            var contentResult = (ContentResult)contentController.Content_WithNoEncoding();

            // Assert
            // The default content type of ContentResult is used when the result is executed.
            Assert.Null(contentResult.ContentType);
        }

        [Fact]
        public void Content_InvalidCharset_DefaultEncodingIsUsed()
        {
            // Arrange
            var contentController = new ContentPage();
            var contentType = "text/xml; charset=invalid; p1=p1-value";

            // Act
            var contentResult = (ContentResult)contentController.Content_WithInvalidCharset();

            // Assert
            Assert.NotNull(contentResult.ContentType);
            Assert.Equal(contentType, contentResult.ContentType.ToString());
            // The default encoding of ContentResult is used when this result is executed.
            Assert.Null(MediaType.GetEncoding(contentResult.ContentType));
        }

        [Fact]
        public void Content_CharsetAndEncodingProvided_EncodingIsUsed()
        {
            // Arrange
            var contentController = new ContentPage();
            var contentType = "text/xml; charset=us-ascii; p1=p1-value";

            // Act
            var contentResult = (ContentResult)contentController.Content_WithEncodingInCharset_AndEncodingParameter();

            // Assert
            MediaTypeAssert.Equal(contentType, contentResult.ContentType);
        }

        [Fact]
        public void Content_CharsetInContentType_IsUsedForEncoding()
        {
            // Arrange
            var contentController = new ContentPage();
            var contentType = "text/xml; charset=us-ascii; p1=p1-value";

            // Act
            var contentResult = (ContentResult)contentController.Content_WithEncodingInCharset();

            // Assert
            Assert.Equal(contentType, contentResult.ContentType);
        }

        [Fact]
        public void StatusCode_SetObject()
        {
            // Arrange
            var statusCode = 204;
            var value = new { Value = 42 };

            var statusCodeController = new StatusCodePage();

            // Act
            var result = (ObjectResult)statusCodeController.StatusCode_Object(statusCode, value);

            // Assert
            Assert.Equal(statusCode, result.StatusCode);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void StatusCode_SetObjectNull()
        {
            // Arrange
            var statusCode = 204;
            object value = null;

            var statusCodeController = new StatusCodePage();

            // Act
            var result = statusCodeController.StatusCode_Object(statusCode, value);

            // Assert
            Assert.Equal(statusCode, result.StatusCode);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void StatusCode_SetsStatusCode()
        {
            // Arrange
            var statusCode = 205;
            var statusCodeController = new StatusCodePage();

            // Act
            var result = statusCodeController.StatusCode_Int(statusCode);

            // Assert
            Assert.Equal(statusCode, result.StatusCode);
        }

        public static IEnumerable<object[]> RedirectTestData
        {
            get
            {
                yield return new object[]
                {
                    null,
                    null,
                };

                yield return new object[]
                {
                    new Dictionary<string, object> { { "hello", "world" } },
                    new RouteValueDictionary() { { "hello", "world" } },
                };

                var expected2 = new Dictionary<string, object>
                {
                    { "test", "case" },
                    { "sample", "route" },
                };

                yield return new object[]
                {
                    new RouteValueDictionary(expected2),
                    new RouteValueDictionary(expected2),
                };
            }
        }

        private class ContentPage : Page
        {
            public IActionResult Content_WithNoEncoding()
            {
                return Content("Hello!!");
            }

            public IActionResult Content_WithEncodingInCharset()
            {
                return Content("Hello!!", "text/xml; charset=us-ascii; p1=p1-value");
            }

            public IActionResult Content_WithInvalidCharset()
            {
                return Content("Hello!!", "text/xml; charset=invalid; p1=p1-value");
            }

            public IActionResult Content_WithEncodingInCharset_AndEncodingParameter()
            {
                return Content("Hello!!", "text/xml; charset=invalid; p1=p1-value", Encoding.ASCII);
            }

            public override Task ExecuteAsync()
            {
                throw new NotImplementedException();
            }
        }

        private class StatusCodePage : Page
        {
            public override Task ExecuteAsync()
            {
                throw new NotImplementedException();
            }

            public StatusCodeResult StatusCode_Int(int statusCode)
            {
                return StatusCode(statusCode);
            }

            public ObjectResult StatusCode_Object(int statusCode, object value)
            {
                return StatusCode(statusCode, value);
            }
        }

        private class TestPage : Page
        {
            public override Task ExecuteAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}
