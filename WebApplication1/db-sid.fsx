#r "nuget: Microsoft.Data.SqlClient"
#r "nuget: FSharp.Data.SqlClient"
  
open System  
open System.Data.SqlClient  
open System.Text  
  
let connectionString = "data source=.;initial catalog=Marketplace;Encrypt=False;TrustServerCertificate=True;Integrated Security=true;MultipleActiveResultSets=True"  
let random = System.Random()  
  
let generateRandomString (length: int) =  
    let chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"  
    let sb = StringBuilder(length)  
    for _ in 1 .. length do  
        let index = random.Next(chars.Length)  
        sb.Append(chars.[index]) |> ignore  
    sb.ToString()  
let insertRandomItems (count: int) =  
    let insertSql = """  
        INSERT INTO Items (Id, Name, Description, Metadata)  
        VALUES (@Id, @Name, @Description, @Metadata)  
        """  
    use connection = new SqlConnection(connectionString)  
    connection.Open()  
  
    for id in 1 .. count do  
        let name = generateRandomString 10  
        let description = generateRandomString 50  
        let metadata = generateRandomString 100  
        use insertCommand = new SqlCommand(insertSql, connection)  
        insertCommand.Parameters.AddWithValue("@Id", id) |> ignore  
        insertCommand.Parameters.AddWithValue("@Name", name) |> ignore  
        insertCommand.Parameters.AddWithValue("@Description", description) |> ignore  
        insertCommand.Parameters.AddWithValue("@Metadata", metadata) |> ignore  
        insertCommand.ExecuteNonQuery() |> ignore  
    printfn $"Inserted {count} random records into 'Items'."  
  
insertRandomItems 1000  
  
let insertRandomAuctions (count: int) =  
    let insertSql = """  
        INSERT INTO Auctions (Id, ItemId, CreatedDt, FinishedDt, Price, Status, Seller, Buyer)  
        VALUES (@Id, @ItemId, @CreatedDt, @FinishedDt, @Price, @Status, @Seller, @Buyer)  
        """  
    use connection = new SqlConnection(connectionString)  
    connection.Open()  
  
    for id in 1 .. count do  
        let itemId = id  
        let createdDt = DateTime.Now.AddMinutes(-(random.NextDouble() * 10000.0))  
        let finishedDt = createdDt.AddMinutes(random.NextDouble() * 10000.0)  
        let price = decimal (random.NextDouble() * 1000.0)  
        let status = random.Next(1, 5)  
        let seller = generateRandomString 10  
        let buyer = if random.NextDouble() > 0.5 then generateRandomString 10 else null  
        use insertCommand = new SqlCommand(insertSql, connection)  
        insertCommand.Parameters.AddWithValue("@Id", id) |> ignore  
        insertCommand.Parameters.AddWithValue("@ItemId", itemId) |> ignore  
        insertCommand.Parameters.AddWithValue("@CreatedDt", createdDt) |> ignore  
        insertCommand.Parameters.AddWithValue("@FinishedDt", finishedDt) |> ignore  
        insertCommand.Parameters.AddWithValue("@Price", price) |> ignore  
        insertCommand.Parameters.AddWithValue("@Status", status) |> ignore  
        insertCommand.Parameters.AddWithValue("@Seller", seller) |> ignore  
        if buyer <> null then  
            insertCommand.Parameters.AddWithValue("@Buyer", buyer) |> ignore  
        else  
            insertCommand.Parameters.AddWithValue("@Buyer", DBNull.Value) |> ignore  
        insertCommand.ExecuteNonQuery() |> ignore  
    printfn $"Inserted {count} random records into 'Auctions'."  
  
insertRandomAuctions 1000  