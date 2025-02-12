' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.

Imports Microsoft.EntityFrameworkCore.Query
Imports Microsoft.EntityFrameworkCore.TestModels.Northwind
Imports Microsoft.EntityFrameworkCore.TestUtilities
Imports Xunit

Partial Public Class NorthwindQueryVisualBasicTest
    Inherits QueryTestBase(Of NorthwindQuerySqlServerFixture(Of NoopModelCustomizer))

    Public Sub New(fixture As NorthwindQuerySqlServerFixture(Of NoopModelCustomizer))
        MyBase.New(fixture)

        fixture.TestSqlLoggerFactory.Clear()
    End Sub

    <ConditionalTheory>
    <MemberData(NameOf(IsAsyncData))>
    Public Async Sub CompareString_Equals_Binary(async As Boolean)
        Await AssertQuery(
            async,
            Function(ss) ss.Set(Of Customer).Where(Function(c) c.CustomerID = "ALFKI"),
            entryCount:=1)

        AssertSql(
            "SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
WHERE [c].[CustomerID] = N'ALFKI'")
    End Sub

    <ConditionalTheory>
    <MemberData(NameOf(IsAsyncData))>
    Public Async Sub CompareString_LessThanOrEqual_Binary(async As Boolean)
        Await AssertQuery(
            async,
            Function(ss) ss.Set(Of Customer).Where(Function(c) c.CustomerID <= "ALFKI"),
            entryCount:=1)

        AssertSql(
            "SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
WHERE [c].[CustomerID] <= N'ALFKI'")
    End Sub

    <ConditionalTheory>
    <MemberData(NameOf(IsAsyncData))>
    Public Async Sub AddChecked(async As Boolean)
        Await AssertQuery(
            async,
            Function(ss) ss.Set(Of Product).Where(Function(p) p.UnitsInStock + 1 = 102),
            entryCount:=1)

        AssertSql(
            "SELECT [p].[ProductID], [p].[Discontinued], [p].[ProductName], [p].[SupplierID], [p].[UnitPrice], [p].[UnitsInStock]
FROM [Products] AS [p]
WHERE [p].[UnitsInStock] + CAST(1 AS smallint) = CAST(102 AS smallint)")
    End Sub

    <ConditionalTheory>
    <MemberData(NameOf(IsAsyncData))>
    Public Async Sub SubtractChecked(async As Boolean)
        Await AssertQuery(
            async,
            Function(ss) ss.Set(Of Product).Where(Function(p) p.UnitsInStock - 1 = 100),
            entryCount:=1)

        AssertSql(
            "SELECT [p].[ProductID], [p].[Discontinued], [p].[ProductName], [p].[SupplierID], [p].[UnitPrice], [p].[UnitsInStock]
FROM [Products] AS [p]
WHERE [p].[UnitsInStock] - CAST(1 AS smallint) = CAST(100 AS smallint)")
    End Sub

    <ConditionalTheory>
    <MemberData(NameOf(IsAsyncData))>
    Public Async Sub MultiplyChecked(async As Boolean)
        Await AssertQuery(
            async,
            Function(ss) ss.Set(Of Product).Where(Function(p) p.UnitsInStock * 1 = 101),
            entryCount:=1)

        AssertSql(
            "SELECT [p].[ProductID], [p].[Discontinued], [p].[ProductName], [p].[SupplierID], [p].[UnitPrice], [p].[UnitsInStock]
FROM [Products] AS [p]
WHERE [p].[UnitsInStock] * CAST(1 AS smallint) = CAST(101 AS smallint)")
    End Sub

    Protected Overrides Function CreateQueryAsserter(fixture As NorthwindQuerySqlServerFixture(Of NoopModelCustomizer)) As QueryAsserter
        Return New RelationalQueryAsserter(
            fixture, AddressOf RewriteExpectedQueryExpression, AddressOf RewriteServerQueryExpression, canExecuteQueryString:=True)
    End Function

    Private Sub AssertSql(ParamArray expected As String())
        Fixture.TestSqlLoggerFactory.AssertBaseline(expected)
    End Sub
End Class
