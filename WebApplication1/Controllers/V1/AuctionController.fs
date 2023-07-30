namespace WebApplication1.Controllers.V1

open System
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Caching.Memory
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Core
open WebApplication1.Data
open WebApplication1.Models
open WebApplication1.Helper
open WebApplication1.Queries

[<ApiVersion("1")>]
[<ApiController>]
[<Route("api/v{version:apiVersion}/auctions")>]
type AuctionController(dbContext: AppDbContext, logger: ILogger<AuctionController>, memoryCache: IMemoryCache) =
    inherit ControllerBase()
    let LIMIT = 10_000
    let cacheKey = "AuctionsList"

    let GetAllAuctionsCached(query: AuctionQuery) =
        let query =
            dbContext.Auctions
            |> fun q ->
                match query.Name with
                | Some n -> q.Where(fun a -> a.Item.Name.Contains(n, StringComparison.OrdinalIgnoreCase))
                | None -> q
            |> fun q ->
                match statusParsed query.Status with
                | Some sp -> q.Where(fun a -> a.Status = sp)
                | None -> q
            |> fun q ->
                match (query.SortOrder, query.SortKey) with
                | Some "asc", Some key ->
                    let sortExpression = createSortExpression<Auction, obj> key
                    q.OrderBy(sortExpression).AsQueryable()
                | Some "desc", Some key ->
                    let sortExpression = createSortExpression<Auction, obj> key
                    q.OrderByDescending(sortExpression).AsQueryable()
                | _ -> q
            |> fun q ->
                match query.Skip with
                | Some skip -> q.Skip(skip)
                | _ -> q
            |> fun q ->
                match query.Limit with
                | Some limit -> q.Take(limit)
                | _ -> q.Take(LIMIT)

        let results = query.ToList()

        results

    [<HttpGet>]
    member self.GetAllAuctions([<FromQuery>] query: AuctionQuery) =
        logger.LogInformation("Auctions list retrieved from database.")
        GetAllAuctionsCached query


    [<HttpGet("{id}")>]
    member self.GetAuctionById(id: int) =
        match memoryCache.TryGetValue(cacheKey) with
        | true, auction ->
            logger.LogInformation("Auction retrieved from cache.")
            auction
        | _ ->
            let auction = dbContext.Auctions.FirstOrDefault(fun x -> x.Id = id)
            memoryCache.Set(cacheKey, auction, TimeSpan.FromMinutes 5.) |> ignore
            logger.LogInformation("Auction retrieved from database and cached.")
            auction
