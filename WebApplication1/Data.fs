module WebApplication1.Data

open Microsoft.EntityFrameworkCore
open WebApplication1.Models

type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    override _.OnModelCreating modelBuilder =
        modelBuilder.Entity<Auction>().HasKey(fun auction -> auction.Id :> obj)
        |> ignore

        modelBuilder
            .Entity<Auction>()
            .Property(fun label -> label.Id)
            .ValueGeneratedOnAdd()
        |> ignore

        modelBuilder.Entity<Item>().HasKey(fun item -> item.Id :> obj) |> ignore

        modelBuilder.Entity<Item>().Property(fun item -> item.Id).ValueGeneratedOnAdd
        |> ignore

        modelBuilder
            .Entity<Auction>()
            .HasOne(fun auction -> auction.Item)
            .WithOne(fun item -> item.Auction)
            .HasForeignKey<Auction>(fun x -> x.ItemId :> obj)
            .IsRequired(true)
        |> ignore
        
        modelBuilder.Entity<Auction>()
            .Property(fun (a: Auction) -> a.Price)  
            .HasColumnType("decimal(18, 2)")  |> ignore

    [<DefaultValue>]
    val mutable items: DbSet<Item>
     member x.Items 
        with get() = x.items 
        and set v = x.items <- v


    [<DefaultValue>]
    val mutable auctions: DbSet<Auction>
    member x.Auctions 
        with get() = x.auctions 
        and set v = x.auctions <- v
