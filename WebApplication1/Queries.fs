module WebApplication1.Queries


type AuctionQuery() =  
    member val Name: string option = None with get, set  
    member val Status: string option = None with get, set  
    member val SortOrder: string option = None with get, set  
    member val SortKey: string option = None with get, set  
    member val Skip: int option = None with get, set  
    member val Limit: int option = None with get, set  


