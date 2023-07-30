module WebApplication1.Models

open System

type MarketStatus =
    | None = 0
    | Canceled = 1
    | Finished = 2
    | Active = 3

[<CLIMutable>]
type Item =
    { Id: int
      Name: string
      Description: string
      Metadata: string
      Auction: Auction }

and [<CLIMutable>] Auction =
    { Id: int
      ItemId: int
      Item: Item
      CreatedDt: DateTime
      FinishedDt: DateTime
      Price: decimal
      Status: MarketStatus
      Seller: string
      Buyer: string }
