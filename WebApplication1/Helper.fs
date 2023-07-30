module WebApplication1.Helper

open System
open System.Linq.Expressions
open WebApplication1.Models

let createSortExpression<'TEntity, 'TKey> (property: string) =
    let entityType = typeof<'TEntity>
    let propertyInfo = entityType.GetProperty(property)

    if propertyInfo = null then
        raise (ArgumentException("No property found with the given name."))

    let paramExpr = Expression.Parameter(entityType, "x")
    let propertyExpr = Expression.Property(paramExpr, propertyInfo)

    Expression.Lambda<Func<'TEntity, 'TKey>>(propertyExpr, paramExpr)

let statusParsed (status: string option) =
    match status with
    | Some value ->
        let isParsed, status = Enum.TryParse<MarketStatus>(value, true)
        if isParsed then Some status else None
    | _ -> None
